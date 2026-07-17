// src/pages/MovieDetails.tsx

import {
  useEffect,
  useMemo,
  useState,
  type FormEvent,
} from "react";
import {
  useNavigate,
  useParams,
} from "react-router-dom";
import {
  movieService,
  type MovieDto,
  type ReviewDto,
} from "../services/movieService";
import { useAuth } from "../context/authContext";

const fallbackPoster =
  "https://images.unsplash.com/photo-1440404653325-ab127d49abc1?q=80&w=1200";

export const MovieDetails = () => {
  const { id } = useParams<{ id: string }>();
  const navigate = useNavigate();
  const { user } = useAuth();

  const [movie, setMovie] =
    useState<MovieDto | null>(null);

  const [loading, setLoading] =
    useState(true);

  const [error, setError] =
    useState<string | null>(null);

  const [rating, setRating] =
    useState(5);

  const [comment, setComment] =
    useState("");

  const [submittingReview, setSubmittingReview] =
    useState(false);

  const [reviewError, setReviewError] =
    useState<string | null>(null);

  useEffect(() => {
    let isMounted = true;

    async function fetchMovieDetails() {
      if (!id) {
        setError(
          "The movie identifier is missing."
        );
        setLoading(false);
        return;
      }

      try {
        setLoading(true);
        setError(null);

        const data =
          await movieService.getMovieById(id);

        if (isMounted) {
          setMovie(data);
        }
      } catch (fetchError) {
        console.error(
          "Failed to fetch movie details:",
          fetchError
        );

        if (isMounted) {
          setError(
            "The requested movie could not be loaded."
          );
        }
      } finally {
        if (isMounted) {
          setLoading(false);
        }
      }
    }

    void fetchMovieDetails();

    return () => {
      isMounted = false;
    };
  }, [id]);

  const averageRating = useMemo(() => {
    if (
      !movie?.reviews ||
      movie.reviews.length === 0
    ) {
      return 0;
    }

    const validRatings = movie.reviews
      .map((review) =>
        Number(review.rating)
      )
      .filter(
        (reviewRating) =>
          Number.isFinite(reviewRating) &&
          reviewRating > 0
      );

    if (validRatings.length === 0) {
      return 0;
    }

    return (
      validRatings.reduce(
        (total, reviewRating) =>
          total + reviewRating,
        0
      ) / validRatings.length
    );
  }, [movie?.reviews]);

  const handleReviewSubmit = async (
    event: FormEvent<HTMLFormElement>
  ) => {
    event.preventDefault();

    if (!id || !user) {
      return;
    }

    const trimmedComment =
      comment.trim();

    if (!trimmedComment) {
      setReviewError(
        "Please enter a review comment."
      );
      return;
    }

    if (rating < 1 || rating > 5) {
      setReviewError(
        "The rating must be between 1 and 5."
      );
      return;
    }

    try {
      setSubmittingReview(true);
      setReviewError(null);

      const reviewPayload = {
        rating,
        comment: trimmedComment,
      };

      await movieService.addReview(
        id,
        reviewPayload
      );

      const newLocalReview: ReviewDto = {
        id: crypto.randomUUID(),
        userId: user.id,
        userName:
          user.name || "You",
        rating,
        comment: trimmedComment,
      };

      setMovie((previousMovie) => {
        if (!previousMovie) {
          return null;
        }

        return {
          ...previousMovie,
          reviews: [
            newLocalReview,
            ...(previousMovie.reviews ?? []),
          ],
        };
      });

      setComment("");
      setRating(5);
    } catch (submitError) {
      console.error(
        "Review submission failed:",
        submitError
      );

      setReviewError(
        "The review could not be submitted. Please try again."
      );
    } finally {
      setSubmittingReview(false);
    }
  };

  if (loading) {
    return (
      <div className="flex min-h-screen items-center justify-center bg-slate-950 text-slate-100">
        <div className="space-y-3 text-center">
          <div className="mx-auto h-8 w-8 animate-spin rounded-full border-4 border-rose-500 border-t-transparent" />

          <p className="text-sm tracking-wider text-slate-400">
            Loading movie details...
          </p>
        </div>
      </div>
    );
  }

  if (error || !movie) {
    return (
      <div className="flex min-h-screen items-center justify-center bg-slate-950 p-6 text-rose-400">
        <div className="max-w-md rounded-2xl border border-slate-800 bg-slate-900 p-6 text-center shadow-xl">
          <p className="mb-2 font-bold">
            Movie unavailable
          </p>

          <p className="text-sm text-slate-400">
            {error ||
              "The movie metadata is empty."}
          </p>

          <button
            type="button"
            onClick={() =>
              navigate("/")
            }
            className="mt-6 rounded-xl bg-slate-800 px-4 py-2 text-xs font-bold text-white transition hover:bg-slate-700"
          >
            Return to catalog
          </button>
        </div>
      </div>
    );
  }

  const genres =
    movie.genres ?? [];

  const reviews =
    movie.reviews ?? [];

  return (
    <div className="min-h-screen bg-slate-950 font-sans text-slate-100 antialiased">
      {/* Cinematic backdrop */}
      <section className="relative flex h-[50vh] w-full items-end overflow-hidden md:h-[65vh]">
        <div className="absolute inset-0 z-0">
          <img
            src={
              movie.posterUrl ||
              fallbackPoster
            }
            alt={movie.title}
            className="h-full w-full scale-105 object-cover opacity-30 blur-sm"
            onError={(event) => {
              event.currentTarget.src =
                fallbackPoster;
            }}
          />

          <div className="absolute inset-0 bg-gradient-to-t from-slate-950 via-slate-950/50 to-transparent" />

          <div className="absolute inset-0 bg-gradient-to-r from-slate-950 via-transparent to-slate-950/20" />
        </div>

        <div className="relative z-10 mx-auto flex w-full max-w-6xl flex-col items-center gap-6 px-4 pb-8 sm:px-6 md:flex-row md:items-end lg:px-8">
          {/* Poster */}
          <div className="aspect-[2/3] w-40 translate-y-6 self-center overflow-hidden rounded-2xl border border-slate-800 bg-slate-900 shadow-2xl md:w-56 md:translate-y-12 md:self-auto">
            <img
              src={
                movie.posterUrl ||
                fallbackPoster
              }
              alt={movie.title}
              className="h-full w-full object-cover"
              onError={(event) => {
                event.currentTarget.src =
                  fallbackPoster;
              }}
            />
          </div>

          {/* Header information */}
          <div className="flex-1 space-y-4 text-center md:text-left">
            <h1 className="text-3xl font-black tracking-tight text-white drop-shadow-md md:text-5xl">
              {movie.title}
            </h1>

            <div className="flex flex-wrap items-center justify-center gap-3 text-xs font-semibold text-slate-400 md:justify-start">
              <span className="rounded-md border border-slate-800 bg-slate-900 px-2.5 py-1 font-bold text-amber-400">
                Duration:{" "}
                {movie.duration ||
                  "N/A"}
              </span>

              <span className="text-slate-600">
                •
              </span>

              <span>
                Released:{" "}
                {movie.releaseDate
                  ? new Date(
                      movie.releaseDate
                    ).toLocaleDateString()
                  : "N/A"}
              </span>

              {averageRating > 0 && (
                <>
                  <span className="text-slate-600">
                    •
                  </span>

                  <span className="font-bold text-amber-400">
                    ⭐{" "}
                    {averageRating.toFixed(
                      1
                    )}{" "}
                    / 5
                  </span>
                </>
              )}
            </div>

            {genres.length > 0 && (
              <div className="flex flex-wrap justify-center gap-2 md:justify-start">
                {genres.map((genre) => (
                  <span
                    key={genre}
                    className="rounded-full border border-rose-500/20 bg-rose-500/10 px-3 py-1 text-xs font-semibold text-rose-300"
                  >
                    {genre}
                  </span>
                ))}
              </div>
            )}
          </div>
        </div>
      </section>

      {/* Main content */}
      <main className="mx-auto grid max-w-6xl grid-cols-1 gap-12 px-4 pt-16 pb-24 sm:px-6 lg:grid-cols-3 lg:px-8">
        {/* Left column */}
        <div className="space-y-8 lg:col-span-2">
          {/* Synopsis */}
          <section className="space-y-4">
            <h2 className="border-l-4 border-rose-500 pl-3 text-xl font-bold">
              Synopsis
            </h2>

            <p className="text-sm leading-relaxed text-slate-400 md:text-base">
              {movie.description ||
                "No description is available for this movie."}
            </p>
          </section>

          {/* Play section */}
          <section className="flex flex-wrap items-center justify-between gap-4 rounded-2xl border border-slate-800 bg-slate-900 p-6">
            <div className="space-y-1">
              <p className="text-sm font-bold text-white">
                Ready to watch?
              </p>

              <p className="text-xs text-slate-500">
                Start or continue streaming this movie.
              </p>
            </div>

            <button
              type="button"
              onClick={() =>
                navigate(
                  `/watch/${movie.id}`
                )
              }
              className="cursor-pointer rounded-xl bg-rose-600 px-6 py-3 text-sm font-bold text-white shadow-lg shadow-rose-950/40 transition duration-200 hover:bg-rose-700"
            >
              ▶ Play movie
            </button>
          </section>

          {/* Reviews */}
          <section className="space-y-6">
            <div className="flex flex-wrap items-center justify-between gap-3">
              <h2 className="border-l-4 border-slate-700 pl-3 text-xl font-bold">
                User Reviews
              </h2>

              <span className="text-xs text-slate-500">
                {reviews.length}{" "}
                {reviews.length === 1
                  ? "review"
                  : "reviews"}
              </span>
            </div>

            {reviews.length === 0 ? (
              <p className="rounded-xl border border-slate-900 bg-slate-900/30 p-6 text-sm italic text-slate-500">
                No reviews have been submitted yet. Be the first to add one.
              </p>
            ) : (
              <div className="space-y-4">
                {reviews.map((review) => (
                  <article
                    key={review.id}
                    className="space-y-3 rounded-xl border border-slate-800 bg-slate-900/60 p-5"
                  >
                    <div className="flex items-center justify-between gap-4">
                      <span className="text-sm font-semibold tracking-wide text-slate-300">
                        {review.userName ||
                          "Anonymous"}
                      </span>

                      <span className="rounded-md bg-amber-400/10 px-2 py-1 text-xs font-bold text-amber-400">
                        ⭐{" "}
                        {review.rating}/5
                      </span>
                    </div>

                    <p className="text-sm leading-relaxed text-slate-300">
                      {review.comment}
                    </p>
                  </article>
                ))}
              </div>
            )}
          </section>
        </div>

        {/* Review sidebar */}
        <aside className="space-y-6">
          <div className="space-y-4 rounded-2xl border border-slate-800 bg-slate-900 p-6 shadow-xl">
            <h3 className="text-base font-bold tracking-wide text-white">
              Write a Review
            </h3>

            <p className="text-xs leading-relaxed text-slate-500">
              Share your opinion and help improve future recommendations.
            </p>

            {user ? (
              <form
                onSubmit={
                  handleReviewSubmit
                }
                className="space-y-4"
              >
                <div className="space-y-2">
                  <label
                    htmlFor="review-rating"
                    className="block text-xs font-bold uppercase tracking-wider text-slate-400"
                  >
                    Rating
                  </label>

                  <select
                    id="review-rating"
                    value={rating}
                    onChange={(event) =>
                      setRating(
                        Number(
                          event.target
                            .value
                        )
                      )
                    }
                    className="w-full rounded-xl border border-slate-800 bg-slate-950 p-3 text-sm text-slate-200 focus:border-rose-500 focus:outline-none"
                  >
                    <option value={5}>
                      ⭐⭐⭐⭐⭐ Excellent
                    </option>

                    <option value={4}>
                      ⭐⭐⭐⭐ Very good
                    </option>

                    <option value={3}>
                      ⭐⭐⭐ Good
                    </option>

                    <option value={2}>
                      ⭐⭐ Fair
                    </option>

                    <option value={1}>
                      ⭐ Poor
                    </option>
                  </select>
                </div>

                <div className="space-y-2">
                  <label
                    htmlFor="review-comment"
                    className="block text-xs font-bold uppercase tracking-wider text-slate-400"
                  >
                    Comment
                  </label>

                  <textarea
                    id="review-comment"
                    value={comment}
                    onChange={(event) =>
                      setComment(
                        event.target.value
                      )
                    }
                    required
                    maxLength={1000}
                    rows={5}
                    placeholder="What did you think about this movie?"
                    className="w-full resize-none rounded-xl border border-slate-800 bg-slate-950 p-3 text-sm leading-relaxed text-slate-200 placeholder:text-slate-700 focus:border-rose-500 focus:outline-none"
                  />

                  <div className="text-right text-[10px] text-slate-600">
                    {comment.length}/1000
                  </div>
                </div>

                {reviewError && (
                  <p className="rounded-md border border-rose-500/20 bg-rose-500/10 p-3 text-xs text-rose-400">
                    {reviewError}
                  </p>
                )}

                <button
                  type="submit"
                  disabled={
                    submittingReview ||
                    !comment.trim()
                  }
                  className="w-full rounded-xl bg-gradient-to-r from-rose-600 to-amber-600 py-3 text-sm font-bold text-white shadow-md transition hover:opacity-90 disabled:cursor-not-allowed disabled:opacity-50"
                >
                  {submittingReview
                    ? "Publishing..."
                    : "Publish Review"}
                </button>
              </form>
            ) : (
              <div className="rounded-xl border border-dashed border-slate-800 bg-slate-950/50 p-4 text-center">
                <p className="mb-3 text-xs text-slate-400">
                  Sign in to publish a review.
                </p>

                <button
                  type="button"
                  onClick={() =>
                    navigate("/login")
                  }
                  className="text-xs font-bold text-rose-400 hover:underline"
                >
                  Sign In
                </button>
              </div>
            )}
          </div>
        </aside>
      </main>
    </div>
  );
};