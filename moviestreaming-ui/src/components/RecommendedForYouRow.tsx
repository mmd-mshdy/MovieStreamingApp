import { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import {
  AlertCircle,
  Play,
  RefreshCw,
  Sparkles,
} from "lucide-react";
import {
  recommendationService,
  type RecommendedMovieDto,
} from "../services/recommendationService";

const fallbackPoster =
  "https://images.unsplash.com/photo-1440404653325-ab127d49abc1?q=80&w=600";

export const RecommendedForYouRow = () => {
  const [recommendations, setRecommendations] = useState<
    RecommendedMovieDto[]
  >([]);

  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  const navigate = useNavigate();

  const loadRecommendations = async () => {
    try {
      setLoading(true);
      setError(null);

      const data =
        await recommendationService.getRecommendations(10);

      console.log("Recommendation response:", data);

      setRecommendations(
        Array.isArray(data) ? data : []
      );
    } catch (error) {
      console.error(
        "Failed to load recommendations:",
        error
      );

      setError(
        "Recommendations are temporarily unavailable."
      );

      setRecommendations([]);
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    void loadRecommendations();
  }, []);

  if (loading) {
    return <RecommendationSkeleton />;
  }

  if (error) {
    return (
      <section className="mb-12">
        <SectionHeading />

        <div className="flex items-center justify-between rounded-2xl border border-red-500/20 bg-red-500/5 px-5 py-4">
          <div className="flex items-center gap-3">
            <AlertCircle className="h-5 w-5 text-red-400" />

            <p className="text-sm text-slate-300">
              {error}
            </p>
          </div>

          <button
            type="button"
            onClick={() =>
              void loadRecommendations()
            }
            className="flex items-center gap-2 rounded-lg bg-slate-800 px-3 py-2 text-xs font-bold text-white transition hover:bg-slate-700"
          >
            <RefreshCw className="h-3.5 w-3.5" />
            Retry
          </button>
        </div>
      </section>
    );
  }

  if (recommendations.length === 0) {
    return null;
  }

  return (
    <section className="mb-12">
      <SectionHeading />

      <div className="grid grid-cols-2 gap-5 sm:grid-cols-3 md:grid-cols-4 lg:grid-cols-5 xl:grid-cols-6">
        {recommendations.map(
          (recommendation) => {
            const { movie, score, reason } =
              recommendation;

            const matchPercentage = Math.round(
              Math.min(
                Math.max(score * 100, 0),
                100
              )
            );

            return (
              <article
                key={movie.id}
                onClick={() =>
                  navigate(`/movies/${movie.id}`)
                }
                className="group relative cursor-pointer overflow-hidden rounded-2xl border border-slate-800 bg-slate-900 shadow-xl transition duration-300 hover:-translate-y-1 hover:scale-[1.02] hover:border-amber-500/50"
              >
                <div className="relative aspect-[2/3] overflow-hidden bg-slate-950">
                  <img
                    src={
                      movie.posterUrl ||
                      fallbackPoster
                    }
                    alt={movie.title}
                    className="h-full w-full object-cover transition duration-500 group-hover:scale-105 group-hover:brightness-75"
                    onError={(event) => {
                      event.currentTarget.src =
                        fallbackPoster;
                    }}
                  />

                  <div className="absolute inset-0 bg-gradient-to-t from-black via-black/10 to-transparent" />

                  <div className="absolute left-3 top-3 rounded-full border border-emerald-400/30 bg-black/70 px-2.5 py-1 text-[10px] font-black text-emerald-400 backdrop-blur">
                    {matchPercentage}% match
                  </div>

                  <div className="absolute inset-0 flex items-center justify-center opacity-0 transition duration-300 group-hover:opacity-100">
                    <button
                      type="button"
                      aria-label={`Open ${movie.title}`}
                      className="flex h-12 w-12 items-center justify-center rounded-full bg-amber-500 text-black shadow-2xl transition hover:scale-110 hover:bg-amber-400"
                    >
                      <Play className="ml-0.5 h-5 w-5 fill-black" />
                    </button>
                  </div>

                  <div className="absolute bottom-0 left-0 right-0 p-3">
                    <h3 className="line-clamp-2 text-sm font-black text-white">
                      {movie.title}
                    </h3>
                  </div>
                </div>

                <div className="space-y-2 p-3">
                  {movie.genres &&
                    movie.genres.length > 0 && (
                      <p className="truncate text-[10px] font-semibold uppercase tracking-wide text-amber-400">
                        {movie.genres
                          .slice(0, 2)
                          .join(" • ")}
                      </p>
                    )}

                  <p className="line-clamp-2 min-h-8 text-[11px] leading-4 text-slate-400">
                    {reason ||
                      "Recommended based on your activity"}
                  </p>
                </div>
              </article>
            );
          }
        )}
      </div>
    </section>
  );
};

const SectionHeading = () => {
  return (
    <div className="mb-6 flex items-center gap-3">
      <div className="flex h-9 w-9 items-center justify-center rounded-xl bg-amber-500/10">
        <Sparkles className="h-5 w-5 text-amber-400" />
      </div>

      <div>
        <h2 className="text-xl font-black tracking-tight text-white md:text-2xl">
          Recommended for You
        </h2>

        <p className="mt-0.5 text-xs text-slate-500">
          Personalized using your watch history and ratings
        </p>
      </div>
    </div>
  );
};

const RecommendationSkeleton = () => {
  return (
    <section className="mb-12">
      <SectionHeading />

      <div className="grid grid-cols-2 gap-5 sm:grid-cols-3 md:grid-cols-4 lg:grid-cols-5 xl:grid-cols-6">
        {Array.from({ length: 6 }).map(
          (_, index) => (
            <div
              key={index}
              className="overflow-hidden rounded-2xl border border-slate-800 bg-slate-900"
            >
              <div className="aspect-[2/3] animate-pulse bg-slate-800" />

              <div className="space-y-3 p-3">
                <div className="h-3 w-2/3 animate-pulse rounded bg-slate-800" />
                <div className="h-2 w-full animate-pulse rounded bg-slate-800" />
                <div className="h-2 w-4/5 animate-pulse rounded bg-slate-800" />
              </div>
            </div>
          )
        )}
      </div>
    </section>
  );
};