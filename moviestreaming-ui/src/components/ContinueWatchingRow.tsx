// src/components/ContinueWatchingRow.tsx

import { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import { Play } from "lucide-react";
import {
  watchHistoryService,
  type ContinueWatchingDto,
} from "../services/watchHistoryService";

const fallbackPoster =
  "https://images.unsplash.com/photo-1440404653325-ab127d49abc1?q=80&w=400";

export const ContinueWatchingRow = () => {
  const [items, setItems] = useState<
    ContinueWatchingDto[]
  >([]);

  const [loading, setLoading] =
    useState(true);

  const [error, setError] =
    useState<string | null>(null);

  const navigate = useNavigate();

  useEffect(() => {
    let isMounted = true;

    async function loadContinueWatching() {
      try {
        setLoading(true);
        setError(null);

        const data =
          await watchHistoryService
            .getContinueWatching();

        console.table(
          data.map((item) => ({
            title: item.title,
            positionSeconds:
              item.positionSeconds,
            durationSeconds:
              item.durationSeconds,
            progressPercentage:
              item.progressPercentage,
          }))
        );

        if (isMounted) {
          setItems(
            Array.isArray(data)
              ? data
              : []
          );
        }
      } catch (loadError) {
        console.error(
          "Failed to load continue-watching movies:",
          loadError
        );

        if (isMounted) {
          setItems([]);
          setError(
            "Continue watching could not be loaded."
          );
        }
      } finally {
        if (isMounted) {
          setLoading(false);
        }
      }
    }

    void loadContinueWatching();

    return () => {
      isMounted = false;
    };
  }, []);

  if (loading) {
    return (
      <section className="mb-10">
        <h2 className="mb-6 border-l-4 border-amber-500 pl-3 text-xl font-black tracking-tight text-white md:text-2xl">
          Continue Watching
        </h2>

        <div className="grid grid-cols-2 gap-6 sm:grid-cols-3 md:grid-cols-4 lg:grid-cols-5">
          {Array.from({
            length: 5,
          }).map((_, index) => (
            <div
              key={index}
              className="aspect-video animate-pulse rounded-2xl bg-slate-800"
            />
          ))}
        </div>
      </section>
    );
  }

  if (error || items.length === 0) {
    return null;
  }

  return (
    <section className="mb-10">
      <h2 className="mb-6 border-l-4 border-amber-500 pl-3 text-xl font-black tracking-tight text-white md:text-2xl">
        Continue Watching
      </h2>

      <div className="grid grid-cols-2 gap-6 sm:grid-cols-3 md:grid-cols-4 lg:grid-cols-5">
        {items.map((item) => {
          const progress =
            Number.isFinite(
              item.progressPercentage
            )
              ? Math.min(
                  Math.max(
                    item.progressPercentage,
                    0
                  ),
                  100
                )
              : 0;

          return (
            <article
              key={item.movieId}
              onClick={() =>
                navigate(
                  `/watch/${item.movieId}`
                )
              }
              className="group relative flex cursor-pointer flex-col overflow-hidden rounded-2xl border border-slate-800/80 bg-slate-900 transition duration-200 hover:scale-[1.02]"
            >
              <div className="relative aspect-video w-full overflow-hidden bg-slate-950">
                <img
                  src={
                    item.posterUrl ||
                    fallbackPoster
                  }
                  alt={item.title}
                  className="h-full w-full object-cover brightness-75 transition duration-300 group-hover:brightness-90"
                  onError={(event) => {
                    event.currentTarget.src =
                      fallbackPoster;
                  }}
                />

                <div className="absolute inset-0 flex items-center justify-center bg-black/40 opacity-0 transition duration-200 group-hover:opacity-100">
                  <div className="rounded-full bg-amber-500 p-2.5 text-black shadow-lg">
                    <Play className="ml-0.5 h-4 w-4 fill-black" />
                  </div>
                </div>

                <div className="absolute bottom-0 left-0 h-2 w-full bg-slate-800">
                  <div
                    className="h-full bg-gradient-to-r from-amber-500 to-yellow-400 transition-all duration-300"
                    style={{
                      width: `${progress}%`,
                    }}
                  />
                </div>
              </div>

              <div className="flex items-center justify-between gap-3 p-3">
                <div className="min-w-0">
                  <h3 className="truncate text-xs font-bold text-white">
                    {item.title}
                  </h3>

                  <p className="mt-1 text-[10px] text-slate-500">
                    {item.positionSeconds}s of{" "}
                    {item.durationSeconds}s
                  </p>
                </div>

                <span className="shrink-0 text-xs font-bold text-amber-400">
                  {progress.toFixed(1)}%
                </span>
              </div>
            </article>
          );
        })}
      </div>
    </section>
  );
};