from contextlib import asynccontextmanager

from fastapi import FastAPI, HTTPException, Query

from app.models import (
    EngineStatus,
    RecommendationRequest,
    RecommendationResponse,
    SimilarMovieResponse,
)
from app.recommender import recommendation_engine


@asynccontextmanager
async def lifespan(app: FastAPI):
    try:
        movie_count = await recommendation_engine.train()

        print(
            f"Recommendation engine trained with "
            f"{movie_count} movies."
        )
    except Exception as error:
        print(
            "Recommendation engine could not be trained "
            f"during startup: {error}"
        )

    yield


app = FastAPI(
    title="Movie Streaming Recommendation Service",
    version="1.0.0",
    lifespan=lifespan,
)


@app.get("/status", response_model=EngineStatus)
async def get_status() -> EngineStatus:
    return EngineStatus(
        trained=recommendation_engine.is_trained,
        movieCount=len(recommendation_engine.movies),
    )


@app.post("/train", response_model=EngineStatus)
async def train_engine() -> EngineStatus:
    try:
        movie_count = await recommendation_engine.train()

        return EngineStatus(
            trained=True,
            movieCount=movie_count,
        )
    except Exception as error:
        raise HTTPException(
            status_code=500,
            detail=f"Training failed: {error}",
        ) from error


@app.get(
    "/similar/{movie_id}",
    response_model=SimilarMovieResponse,
)
async def get_similar_movies(
    movie_id: str,
    top_n: int = Query(default=10, ge=1, le=50),
) -> SimilarMovieResponse:
    try:
        recommendations = recommendation_engine.recommend_similar(
            movie_id=movie_id,
            top_n=top_n,
        )

        return SimilarMovieResponse(
            sourceMovieId=movie_id,
            recommendations=recommendations,
        )
    except KeyError as error:
        raise HTTPException(
            status_code=404,
            detail=str(error),
        ) from error
    except RuntimeError as error:
        raise HTTPException(
            status_code=503,
            detail=str(error),
        ) from error


@app.post(
    "/recommend",
    response_model=RecommendationResponse,
)
async def recommend_for_user(
    request: RecommendationRequest,
) -> RecommendationResponse:
    try:
        recommendations = (
            recommendation_engine.recommend_for_user(
                interactions=request.interactions,
                top_n=request.topN,
            )
        )

        return RecommendationResponse(
            recommendations=recommendations
        )
    except RuntimeError as error:
        raise HTTPException(
            status_code=503,
            detail=str(error),
        ) from error