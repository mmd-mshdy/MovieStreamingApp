// src/pages/Home.tsx

import { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import {
  movieService,
  type MovieDto,
} from "../services/movieService";
import { useAuth } from "../context/authContext";
import { ContinueWatchingRow } from "../components/ContinueWatchingRow";
import { RecommendedForYouRow } from "../components/RecommendedForYouRow";
import { MovieSearchBar } from "../components/MovieSearchBar";

const fallbackPoster =
  "https://images.unsplash.com/photo-1440404653325-ab127d49abc1?q=80&w=400";

interface HomeMovie extends MovieDto {
  rating: number;
  releaseYear: number | string;
}

const mockMovies: HomeMovie[] = [
  {
    id: "9b1deb4d-3b7d-4bad-9bdd-2b0d7b3dcb6d",
    title: "Inception",
    description:
      "A skilled thief enters people's dreams to steal secrets and plant an idea.",
    posterUrl:
      "https://image.tmdb.org/t/p/w500/9gk7adHY9CjST6Y99PaIQpSRfsQ.jpg",
    videoUrl: "",
    rating: 4.8,
    releaseYear: 2010,
    releaseDate: "2010-07-16",
    duration: "02:28:00",
    reviews: [],
    genres: ["Science Fiction", "Thriller"],
  },
  {
    id: "1a2b3c4d-5e6f-7a8b-9c0d-1e2f3a4b5c6d",
    title: "Interstellar",
    description:
      "A team of explorers travels through a wormhole in space to ensure humanity's survival.",
    posterUrl:
      "https://image.tmdb.org/t/p/w500/gEU2QniE6E77NIvKCUgCYJu7stg.jpg",
    videoUrl: "",
    rating: 4.7,
    releaseYear: 2014,
    releaseDate: "2014-11-07",
    duration: "02:49:00",
    reviews: [],
    genres: ["Science Fiction", "Drama"],
  },
  {
    id: "8f7e6d5c-4b3a-2b1a-0f9e-8d7c6b5a4c3b",
    title: "The Dark Knight",
    description:
      "Batman faces a criminal mastermind who plunges Gotham into chaos.",
    posterUrl:
      "https://image.tmdb.org/t/p/w500/qJ2tWGB2mS6tC86m1Xw3gIuK6Y7.jpg",
    videoUrl: "",
    rating: 4.9,
    releaseYear: 2008,
    releaseDate: "2008-07-18",
    duration: "02:32:00",
    reviews: [],
    genres: ["Action", "Crime", "Drama"],
  },
  {
    id: "7c6b5a4c-3b2a-1a0f-9e8d-7c6b5a4c3b2a",
    title: "Blade Runner 2049",
    description:
      "A young blade runner uncovers a secret that could destabilize society.",
    posterUrl:
      "https://image.tmdb.org/t/p/w500/gajva2L0vI4Z6wXSg6z6w66Z67f.jpg",
    videoUrl: "",
    rating: 4.4,
    releaseYear: 2017,
    releaseDate: "2017-10-06",
    duration: "02:44:00",
    reviews: [],
    genres: ["Science Fiction", "Mystery"],
  },
  {
    id: "5a4c3b2a-1a0f-9e8d-7c6b-5a4c3b2a1a0f",
    title: "Mad Max: Fury Road",
    description:
      "A rebel warrior joins a group escaping a tyrant across a ruined wasteland.",
    posterUrl:
      "https://image.tmdb.org/t/p/w500/8tZYtuWeox6Jb8clvc4gY07U69R.jpg",
    videoUrl: "",
    rating: 4.5,
    releaseYear: 2015,
    releaseDate: "2015-05-15",
    duration: "02:00:00",
    reviews: [],
    genres: ["Action", "Adventure"],
  },
];

function calculateAverageRating(movie: MovieDto): number {
  const ratings =
    movie.reviews
      ?.map((review) => review.rating)
      .filter((rating) => Number.isFinite(rating)) ?? [];

  if (ratings.length === 0) {
    return 0;
  }

  const total = ratings.reduce(
    (sum, rating) => sum + rating,
    0
  );

  return total / ratings.length;
}

export const Home = () => {
  const { logout, user } = useAuth();
  const navigate = useNavigate();

  const [movies, setMovies] =
    useState<HomeMovie[]>(mockMovies);

  const [loading, setLoading] = useState(true);
  const [error, setError] = useState("");

  useEffect(() => {
    let isMounted = true;

    async function loadMovies() {
      try {
        setLoading(true);
        setError("");

        const data = await movieService.getAllMovies();

        if (!isMounted) {
          return;
        }

        if (Array.isArray(data) && data.length > 0) {
          const formattedMovies: HomeMovie[] =
            data.map((movie: MovieDto) => ({
              ...movie,

              rating:
                calculateAverageRating(movie),

              releaseYear: movie.releaseDate
                ? new Date(
                    movie.releaseDate
                  ).getFullYear()
                : "N/A",

              duration:
                movie.duration || "N/A",

              reviews:
                movie.reviews ?? [],

              genres:
                movie.genres ?? [],
            }));
            console.log(
  "Formatted movies:",
  formattedMovies.map((movie) => ({
    title: movie.title,
    genres: movie.genres,
  }))
);

          setMovies(formattedMovies);
        } else {
          setMovies(mockMovies);
        }
      } catch (loadError) {
        console.warn(
          "Could not load movies from the backend. Using fallback movies.",
          loadError
        );

        if (isMounted) {
          setMovies(mockMovies);
          setError(
            "The movie catalog could not be refreshed. Showing fallback content."
          );
        }
      } finally {
        if (isMounted) {
          setLoading(false);
        }
      }
    }

    void loadMovies();

    return () => {
      isMounted = false;
    };
  }, []);

  const spotlightMovie =
    movies[0] ?? mockMovies[0];

  const handleMovieNavigation = (
    movieId: string
  ) => {
    navigate(`/movies/${movieId}`);
  };

  const handleWatchNavigation = (
    movieId: string
  ) => {
    navigate(`/watch/${movieId}`);
  };

  const handleLogout = () => {
    logout();
    navigate("/login");
  };

  return (
    <div className="min-h-screen bg-brandDark font-sans text-slate-100 antialiased selection:bg-brandAccent selection:text-white">
      {/* Header */}
      <nav className="sticky top-0 z-50 border-b border-slate-800/60 bg-slate-950/80 backdrop-blur-md">
        <div className="mx-auto flex h-16 max-w-7xl items-center gap-4 px-4 sm:px-6 lg:px-8">
  <button
    type="button"
    onClick={() => navigate("/")}
    className="shrink-0 cursor-pointer text-xl font-black tracking-widest text-white transition hover:opacity-95"
  >
    🎬 MOVIE
    <span className="text-brandAccent">
      STREAM
    </span>
  </button>

  <div className="hidden flex-1 justify-center md:flex">
    <MovieSearchBar />
  </div>

  <div className="ml-auto flex shrink-0 items-center gap-4 sm:gap-6">
    <span className="hidden text-sm font-medium text-slate-400 lg:inline">
      Hi, {user?.name || "Guest"}
    </span>

    {user && (
      <button
        type="button"
        onClick={() =>
          navigate("/history")
        }
        className="hidden cursor-pointer border-r border-slate-800 pr-6 font-mono text-xs font-bold uppercase tracking-wider text-slate-400 transition hover:text-amber-400 lg:inline"
      >
        🕒 Watch History
      </button>
    )}

    {user ? (
      <button
        type="button"
        onClick={logout}
        className="cursor-pointer rounded-xl bg-slate-800 px-4 py-2 text-sm font-semibold text-white shadow-sm transition duration-200 hover:bg-slate-700 active:scale-95"
      >
        Log Out
      </button>
    ) : (
      <button
        type="button"
        onClick={() =>
          navigate("/login")
        }
        className="rounded-xl bg-brandAccent px-5 py-2 text-sm font-bold text-white shadow-md shadow-rose-950 transition duration-200 hover:bg-rose-700"
      >
        Sign In
      </button>
    )}
  </div>
</div>
<div className="border-b border-slate-800 bg-slate-950 px-4 py-3 md:hidden">
  <MovieSearchBar />
</div>
      </nav>

      {/* Hero */}
      <section className="relative flex h-[60vh] w-full items-end border-b border-slate-900/40 md:h-[75vh]">
        <div className="absolute inset-0 z-0">
          <img
            src={
              spotlightMovie.posterUrl ||
              fallbackPoster
            }
            alt={spotlightMovie.title}
            className="h-full w-full object-cover opacity-30 blur-[0.5px]"
            onError={(event) => {
              event.currentTarget.src =
                fallbackPoster;
            }}
          />

          <div className="absolute inset-0 bg-gradient-to-t from-brandDark via-brandDark/60 to-transparent" />

          <div className="absolute inset-0 bg-gradient-to-r from-brandDark via-brandDark/40 to-transparent" />
        </div>

        <div className="relative z-10 mx-auto w-full max-w-7xl px-4 pb-14 sm:px-6 md:pb-24 lg:px-8">
          <span className="inline-flex items-center gap-1.5 rounded-full border border-brandAccent/20 bg-brandAccent/10 px-3 py-1 text-xs font-bold uppercase tracking-widest text-brandAccent">
            🎬 Featured Presentation
          </span>

          <h1 className="mt-4 mb-4 max-w-3xl text-4xl font-black tracking-tight text-white drop-shadow-lg md:text-6xl">
            {spotlightMovie.title}
          </h1>

          <p className="mb-3 max-w-2xl text-base font-medium leading-relaxed text-slate-300 drop-shadow-sm md:text-lg">
            {spotlightMovie.description ||
              "Discover movies selected from our streaming catalog."}
          </p>

          <div className="mb-6 flex flex-wrap items-center gap-3 text-sm text-slate-300">
            <span>
              {spotlightMovie.releaseYear}
            </span>

            <span className="text-slate-600">
              •
            </span>

            <span>
              {spotlightMovie.duration}
            </span>

            {spotlightMovie.rating > 0 && (
              <>
                <span className="text-slate-600">
                  •
                </span>

                <span className="font-bold text-amber-400">
                  ⭐{" "}
                  {spotlightMovie.rating.toFixed(
                    1
                  )}
                </span>
              </>
            )}

            {spotlightMovie.genres.length >
              0 && (
              <>
                <span className="text-slate-600">
                  •
                </span>

                <span>
                  {spotlightMovie.genres
                    .slice(0, 3)
                    .join(" · ")}
                </span>
              </>
            )}
          </div>

          <div className="flex flex-wrap gap-4">
            <button
              type="button"
              onClick={() =>
                handleWatchNavigation(
                  spotlightMovie.id
                )
              }
              className="cursor-pointer rounded-xl bg-brandAccent px-6 py-3 text-sm font-bold text-white shadow-lg shadow-rose-900/30 transition duration-200 hover:bg-rose-700 active:scale-95 md:text-base"
            >
              ▶ Play Now
            </button>

            <button
              type="button"
              onClick={() =>
                handleMovieNavigation(
                  spotlightMovie.id
                )
              }
              className="cursor-pointer rounded-xl border border-slate-700 bg-slate-800/80 px-6 py-3 text-sm font-bold text-white transition duration-200 hover:bg-slate-700 md:text-base"
            >
              ℹ Details
            </button>
          </div>
        </div>
      </section>

      {/* Main Content */}
      <main className="relative z-20 mx-auto max-w-7xl px-4 py-12 sm:px-6 lg:px-8">
        {loading && (
          <div>
            <div className="mb-6 h-7 w-40 animate-pulse rounded-lg bg-slate-800" />

            <div className="grid grid-cols-2 gap-6 sm:grid-cols-3 md:grid-cols-4 lg:grid-cols-5">
              {Array.from({
                length: 5,
              }).map((_, index) => (
                <div
                  key={index}
                  className="aspect-[2/3] animate-pulse rounded-2xl bg-slate-800"
                />
              ))}
            </div>
          </div>
        )}

        {error && !loading && (
          <p className="mx-auto mb-8 max-w-xl rounded-xl border border-amber-500/20 bg-amber-500/10 p-4 text-center text-sm font-medium text-amber-300">
            {error}
          </p>
        )}

        {!loading && (
          <div className="space-y-14">
            {/* User-specific content */}
            {user && <ContinueWatchingRow />}

            {user && <RecommendedForYouRow />}

            {/* Trending */}
            <section>
              <h2 className="mb-6 border-l-4 border-brandAccent pl-3 text-xl font-black tracking-tight text-white md:text-2xl">
                Trending Now
              </h2>

              <div className="grid grid-cols-2 gap-6 sm:grid-cols-3 md:grid-cols-4 lg:grid-cols-5">
                {movies.map((movie) => (
                  <article
                    key={movie.id}
                    onClick={() =>
                      handleMovieNavigation(
                        movie.id
                      )
                    }
                    className="group relative flex cursor-pointer flex-col overflow-hidden rounded-2xl border border-slate-800 bg-slate-900 shadow-md transition-all duration-300 hover:scale-[1.03] hover:border-slate-700/80 hover:shadow-2xl"
                  >
                    <div className="relative aspect-[2/3] w-full overflow-hidden bg-slate-950">
                      <img
                        src={
                          movie.posterUrl ||
                          fallbackPoster
                        }
                        alt={movie.title}
                        className="h-full w-full object-cover transition duration-500 group-hover:scale-105"
                        onError={(event) => {
                          event.currentTarget.src =
                            fallbackPoster;
                        }}
                      />

                      <div className="absolute inset-0 flex items-end bg-gradient-to-t from-slate-950 via-transparent p-4 opacity-0 transition duration-300 group-hover:opacity-100">
                        <button
                          type="button"
                          onClick={(event) => {
                            event.stopPropagation();

                            handleWatchNavigation(
                              movie.id
                            );
                          }}
                          className="w-full cursor-pointer rounded-xl bg-brandAccent py-2 text-xs font-bold text-white shadow-md shadow-rose-900/40 hover:bg-rose-700"
                        >
                          Watch Stream
                        </button>
                      </div>
                    </div>

                    <div className="flex flex-grow flex-col justify-between p-4">
                      <div>
                        <h3 className="line-clamp-1 text-sm font-bold text-white transition duration-150 group-hover:text-brandAccent">
                          {movie.title}
                        </h3>

                        {movie.genres.length >
                          0 && (
                          <p className="mt-1 truncate text-[10px] font-semibold uppercase tracking-wide text-slate-500">
                            {movie.genres
                              .slice(0, 2)
                              .join(" • ")}
                          </p>
                        )}
                      </div>

                      <div className="mt-3 flex items-center justify-between gap-2 text-xs text-slate-400">
                        <span className="flex items-center gap-1 font-bold text-amber-400">
                          ⭐{" "}
                          {movie.rating > 0
                            ? movie.rating.toFixed(
                                1
                              )
                            : "N/A"}
                        </span>

                        <span>
                          {movie.releaseYear}
                        </span>

                        <span className="rounded bg-slate-800 px-1.5 py-0.5 font-mono text-[10px] text-slate-300">
                          {movie.duration}
                        </span>
                      </div>
                    </div>
                  </article>
                ))}
              </div>
            </section>
          </div>
        )}
      </main>
    </div>
  );
};