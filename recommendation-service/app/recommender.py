from __future__ import annotations

from dataclasses import dataclass

import httpx
import numpy as np
from scipy.sparse import csr_matrix, vstack
from sklearn.feature_extraction.text import TfidfVectorizer
from sklearn.metrics.pairwise import cosine_similarity

from app.models import (
    MovieCatalogItem,
    RecommendationInteraction,
    RecommendationResult,
)
from app.settings import settings


@dataclass
class RecommendationEngine:
    movies: list[MovieCatalogItem]
    movie_index: dict[str, int]
    vectorizer: TfidfVectorizer | None
    movie_vectors: csr_matrix | None

    def __init__(self) -> None:
        self.movies = []
        self.movie_index = {}
        self.vectorizer = None
        self.movie_vectors = None

    @property
    def is_trained(self) -> bool:
        return (
            self.vectorizer is not None
            and self.movie_vectors is not None
            and len(self.movies) > 0
        )

    async def load_catalog(self) -> list[MovieCatalogItem]:
        async with httpx.AsyncClient(
            verify=settings.verify_ssl,
            timeout=20.0,
        ) as client:
            response = await client.get(settings.movie_catalog_url)
            response.raise_for_status()

        raw_movies = response.json()

        return [
            MovieCatalogItem.model_validate(movie)
            for movie in raw_movies
        ]

    @staticmethod
    def _build_movie_text(movie: MovieCatalogItem) -> str:
        genres = " ".join(movie.genres)
        cast_members = " ".join(movie.castMembers)

        # Repeating genres gives them slightly more importance in TF-IDF.
        return " ".join(
            [
                movie.title,
                movie.description,
                genres,
                genres,
                cast_members,
                str(movie.releaseYear),
            ]
        ).strip()

    async def train(self) -> int:
        movies = await self.load_catalog()

        if not movies:
            raise ValueError("The movie catalog is empty.")

        movie_texts = [
            self._build_movie_text(movie)
            for movie in movies
        ]

        vectorizer = TfidfVectorizer(
            stop_words="english",
            max_features=5000,
            ngram_range=(1, 2),
            min_df=1,
            sublinear_tf=True,
        )

        movie_vectors = vectorizer.fit_transform(movie_texts)

        self.movies = movies
        self.movie_index = {
            movie.movieId: index
            for index, movie in enumerate(movies)
        }
        self.vectorizer = vectorizer
        self.movie_vectors = movie_vectors.tocsr()

        return len(movies)

    def recommend_similar(
        self,
        movie_id: str,
        top_n: int = 10,
    ) -> list[RecommendationResult]:
        self._ensure_trained()

        source_index = self.movie_index.get(movie_id)

        if source_index is None:
            raise KeyError(f"Movie '{movie_id}' was not found.")

        similarities = cosine_similarity(
            self.movie_vectors[source_index],
            self.movie_vectors,
        ).flatten()

        similarities[source_index] = -1

        ranked_indices = np.argsort(similarities)[::-1]

        results: list[RecommendationResult] = []

        for index in ranked_indices:
            score = float(similarities[index])

            if score <= 0:
                continue

            movie = self.movies[index]

            results.append(
                RecommendationResult(
                    movieId=movie.movieId,
                    score=round(score, 4),
                    reason=(
                        f"Similar to {self.movies[source_index].title}"
                    ),
                )
            )

            if len(results) >= top_n:
                break

        return results

    def recommend_for_user(
        self,
        interactions: list[RecommendationInteraction],
        top_n: int = 10,
    ) -> list[RecommendationResult]:
        self._ensure_trained()

        known_interactions = [
            interaction
            for interaction in interactions
            if interaction.movieId in self.movie_index
        ]

        if not known_interactions:
            return self._popular_fallback(top_n)

        weighted_vectors: list[csr_matrix] = []
        weights: list[float] = []
        interacted_movie_ids: set[str] = set()

        for interaction in known_interactions:
            movie_index = self.movie_index[interaction.movieId]
            weight = self._calculate_interaction_weight(interaction)

            interacted_movie_ids.add(interaction.movieId)

            if weight <= 0:
                continue

            weighted_vectors.append(
                self.movie_vectors[movie_index] * weight
            )
            weights.append(weight)

        if not weighted_vectors or sum(weights) <= 0:
            return self._popular_fallback(
                top_n,
                excluded_movie_ids=interacted_movie_ids,
            )

        user_vector = vstack(weighted_vectors).sum(axis=0)
        user_vector = csr_matrix(user_vector / sum(weights))

        similarities = cosine_similarity(
            user_vector,
            self.movie_vectors,
        ).flatten()

        results: list[RecommendationResult] = []

        for index in np.argsort(similarities)[::-1]:
            movie = self.movies[index]

            if movie.movieId in interacted_movie_ids:
                continue

            content_score = float(similarities[index])
            rating_score = min(max(movie.averageRating / 5.0, 0), 1)

            final_score = (
                content_score * 0.85
                + rating_score * 0.15
            )

            if final_score <= 0:
                continue

            results.append(
                RecommendationResult(
                    movieId=movie.movieId,
                    score=round(final_score, 4),
                    reason="Based on your watch history and ratings",
                )
            )

            if len(results) >= top_n:
                break

        return results

    @staticmethod
    def _calculate_interaction_weight(
        interaction: RecommendationInteraction,
    ) -> float:
        weight = 0.0

        if interaction.watchPercentage >= 90:
            weight += 3.0
        elif interaction.watchPercentage >= 50:
            weight += 2.0
        elif interaction.watchPercentage >= 10:
            weight += 0.5

        if interaction.completed:
            weight += 1.0

        if interaction.inWatchlist:
            weight += 1.5

        if interaction.rating is not None:
            rating_weights = {
                1: -2.0,
                2: -1.0,
                3: 0.5,
                4: 2.5,
                5: 4.0,
            }

            weight += rating_weights[interaction.rating]

        return max(weight, 0.0)

    def _popular_fallback(
        self,
        top_n: int,
        excluded_movie_ids: set[str] | None = None,
    ) -> list[RecommendationResult]:
        excluded_movie_ids = excluded_movie_ids or set()

        available_movies = [
            movie
            for movie in self.movies
            if movie.movieId not in excluded_movie_ids
        ]

        ranked_movies = sorted(
            available_movies,
            key=lambda movie: (
                movie.averageRating,
                movie.releaseYear,
            ),
            reverse=True,
        )

        return [
            RecommendationResult(
                movieId=movie.movieId,
                score=round(
                    min(max(movie.averageRating / 5.0, 0), 1),
                    4,
                ),
                reason="Popular and highly rated",
            )
            for movie in ranked_movies[:top_n]
        ]

    def _ensure_trained(self) -> None:
        if not self.is_trained:
            raise RuntimeError(
                "The recommendation engine has not been trained."
            )


recommendation_engine = RecommendationEngine()