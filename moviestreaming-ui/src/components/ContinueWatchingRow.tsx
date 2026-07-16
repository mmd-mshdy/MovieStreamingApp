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
  const [items, setItems] = useState<ContinueWatchingDto[]>([]);
  const [loading, setLoading] = useState(true);
  const navigate = useNavigate();

  useEffect(() => {
    let isMounted = true;

    const loadContinueWatching = async () => {
      try {
        setLoading(true);

        const data = await watchHistoryService.getContinueWatching();

        console.log("Continue-watching response:", data);

        if (isMounted) {
          setItems(Array.isArray(data) ? data : []);
        }
      } catch (error) {
        console.error(
          "Failed to load continue-watching movies:",
          error
        );

        if (isMounted) {
          setItems([]);
        }
      } finally {
        if (isMounted) {
          setLoading(false);
        }
      }
    };

    void loadContinueWatching();

    return () => {
      isMounted = false;
    };
  }, []);

  if (loading || items.length === 0) {
    return null;
  }

  return (
    <section className="mb-10">
      <h2 className="mb-6 border-l-4 border-amber-500 pl-3 text-xl font-black tracking-tight text-white md:text-2xl">
        Continue Watching
      </h2>

      <div className="grid grid-cols-2 gap-6 sm:grid-cols-3 md:grid-cols-4 lg:grid-cols-5">
        {items.map((item) => {
          const progressPercentage = Number.isFinite(
            item.progressPercentage
          )
            ? Math.min(
                Math.max(item.progressPercentage, 0),
                100
              )
            : 0;

          return (
            <article
              key={item.movieId}
              onClick={() =>
                navigate(`/watch/${item.movieId}`)
              }
              className="group relative flex cursor-pointer flex-col overflow-hidden rounded-2xl border border-slate-800/80 bg-slate-900 transition duration-200 hover:scale-[1.02]"
            >
              <div className="relative aspect-video w-full overflow-hidden bg-slate-950">
                <img
                  src={item.posterUrl || fallbackPoster}
                  alt={item.title}
                  className="h-full w-full object-cover brightness-75 transition duration-300 group-hover:brightness-90"
                  onError={(event) => {
                    event.currentTarget.src = fallbackPoster;
                  }}
                />

                <div className="absolute inset-0 flex items-center justify-center bg-black/40 opacity-0 transition duration-200 group-hover:opacity-100">
                  <div className="rounded-full bg-amber-500 p-2.5 text-black shadow-lg">
                    <Play className="ml-0.5 h-4 w-4 fill-black" />
                  </div>
                </div>

                <div className="absolute bottom-0 left-0 h-1.5 w-full bg-slate-800">
                  <div
                    className="h-full bg-gradient-to-r from-amber-500 to-yellow-400 transition-all duration-300"
                    style={{
                      width: `${progressPercentage}%`,
                    }}
                  />
                </div>
              </div>

              <div className="flex items-center justify-between p-3">
                <h3 className="max-w-[70%] truncate text-xs font-bold text-white">
                  {item.title || "Unknown Movie"}
                </h3>

                <span className="text-[10px] font-bold text-slate-400">
                  {Math.round(progressPercentage)}%
                </span>
              </div>
            </article>
          );
        })}
      </div>
    </section>
  );
};