// src/pages/WatchHistory.tsx
import React, { useEffect, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { watchHistoryService } from '../services/watchHistoryService';
import { movieService, type MovieDto } from '../services/movieService';
import { Film, Calendar, Clapperboard, RefreshCw } from 'lucide-react';

export const WatchHistory: React.FC = () => {
  const [history, setHistory] = useState<any[]>([]);
  const [loading, setLoading] = useState(true);
  const navigate = useNavigate();

  useEffect(() => {
    async function loadAndStitchData() {
      try {
        setLoading(true);
        const log = await watchHistoryService.getWatchHistory();
        const catalog = await movieService.getAllMovies();

        if (!log || log.length === 0) {
          setHistory([]);
          return;
        }

        const hydratedLog = log.map((logItem: any) => {
          // Inside the log.map loop in src/pages/WatchHistory.tsx

const matchedMovie = catalog.find((m: MovieDto) => m.id === logItem.movieId);
let percentage = logItem.progressPercentage;

if (!percentage && matchedMovie) {
  const currentSeconds = logItem.positionSeconds || 0;
  let totalSeconds = 0;

  if (typeof matchedMovie.duration === 'string' && matchedMovie.duration.includes(':')) {
    const parts = matchedMovie.duration.split(':').map(Number);
    if (parts.length === 3) {
      totalSeconds = (parts[0] * 3600) + (parts[1] * 60) + parts[2];
    } else if (parts.length === 2) {
      totalSeconds = (parts[0] * 60) + parts[1];
    }
  } else {
    totalSeconds = parseFloat(matchedMovie.duration || '120') * 60;
  }

  percentage = totalSeconds > 0 ? (currentSeconds / totalSeconds) * 100 : 0;
}

return {
  ...logItem,
  movieTitle: matchedMovie?.title || logItem.movieTitle || "Unknown Movie",
  posterUrl: matchedMovie?.posterUrl || 'https://images.unsplash.com/photo-1440404653325-ab127d49abc1?q=80&w=150',
  displayPercentage: isNaN(percentage) ? 0 : Math.min(Math.max(percentage, 0), 100)
};
        });

        setHistory(hydratedLog);
      } catch (err) {
        console.error("Failed to map user audit sequence.", err);
      } finally {
        setLoading(false);
      }
    }
    loadAndStitchData();
  }, []);

  return (
    <div className="min-h-screen bg-slate-950 text-slate-100 py-12 px-4 sm:px-6 lg:px-8">
      <div className="max-w-4xl mx-auto space-y-8">
        <div>
          <h1 className="text-3xl font-black text-white tracking-tight flex items-center gap-3">
            <Clapperboard className="text-amber-500 h-8 w-8" /> Stream Logs Archive
          </h1>
          <p className="text-xs text-slate-500 mt-1 font-mono">Chronological backend tracking records auto-hydrated</p>
        </div>

        {loading ? (
          <div className="flex justify-center py-12">
            <RefreshCw className="h-8 w-8 animate-spin text-amber-500" />
          </div>
        ) : history.length === 0 ? (
          <div className="text-center p-12 bg-slate-900/30 border border-slate-900 rounded-2xl">
            <Film className="h-12 w-12 text-slate-700 mx-auto mb-3" />
            <p className="text-sm text-slate-400 font-medium">No streaming nodes logged under this authenticated session.</p>
          </div>
        ) : (
          <div className="space-y-3">
            {history.map((log, index) => (
              <div 
                key={`${log.movieId}-${index}`}
                onClick={() => navigate(`/movies/${log.movieId}`)}
                className="p-4 bg-slate-900/60 border border-slate-900 hover:border-slate-800 rounded-xl flex items-center justify-between gap-4 cursor-pointer transition duration-150 group"
              >
                <div className="flex items-center gap-4">
                  <div className="h-12 w-16 bg-slate-950 rounded-lg overflow-hidden border border-slate-800 flex-shrink-0">
                    <img src={log.posterUrl} alt="" className="w-full h-full object-cover opacity-80 group-hover:opacity-100 transition" />
                  </div>
                  <div>
                    <h3 className="font-bold text-sm text-white group-hover:text-amber-500 transition">{log.movieTitle}</h3>
                    <p className="text-xs text-slate-500 flex items-center gap-1 mt-1">
                      <Calendar className="h-3 w-3" /> {log.watchedAt ? new Date(log.watchedAt).toLocaleDateString() : 'Recent'}
                    </p>
                  </div>
                </div>
                <div className="text-right">
                  <span className="inline-block px-2.5 py-1 rounded bg-slate-950 border border-slate-800 text-xs font-mono text-slate-300">
                    {log.displayPercentage >= 95 ? "Completed" : `${Math.round(log.displayPercentage)}% Streamed`}
                  </span>
                </div>
              </div>
            ))}
          </div>
        )}
      </div>
    </div>
  );
};