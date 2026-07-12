// src/components/ContinueWatchingRow.tsx
import React, { useEffect, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { watchHistoryService } from '../services/watchHistoryService';
import { movieService, type MovieDto } from '../services/movieService';
import { Play } from 'lucide-react';

export const ContinueWatchingRow: React.FC = () => {
  const [items, setItems] = useState<any[]>([]);
  const [loading, setLoading] = useState(true);
  const navigate = useNavigate();

  useEffect(() => {
    async function hydrateProgressFeed() {
      try {
        setLoading(true);
        // 1. Fetch raw tracking data from backend
        const rawHistory = await watchHistoryService.getContinueWatching();
        // 2. Fetch complete catalog to map metadata
        const catalog = await movieService.getAllMovies();

        if (!rawHistory || rawHistory.length === 0) {
          setItems([]);
          return;
        }

        // 3. Hydrate and match the missing Titles, Posters, and Percentages
        const hydrated = rawHistory.map((historyItem: any) => {
  const matchedMovie = catalog.find((m: MovieDto) => m.id === historyItem.movieId);

  // 1. Convert the backend LastPosition string (e.g., "00:15:30") into raw seconds
  let currentSeconds = 0;
  const rawPosition = historyItem.lastPosition || historyItem.LastPosition;
  
  if (typeof rawPosition === 'string' && rawPosition.includes(':')) {
    const parts = rawPosition.split(':').map(Number);
    if (parts.length === 3) {
      currentSeconds = (parts[0] * 3600) + (parts[1] * 60) + parts[2];
    } else if (parts.length === 2) {
      currentSeconds = (parts[0] * 60) + parts[1];
    }
  }

  // 2. Parse the Movie total duration string ("02:15:00") into seconds
  let totalSeconds = 0;
  if (matchedMovie && typeof matchedMovie.duration === 'string' && matchedMovie.duration.includes(':')) {
    const parts = matchedMovie.duration.split(':').map(Number);
    if (parts.length === 3) {
      totalSeconds = (parts[0] * 3600) + (parts[1] * 60) + parts[2];
    } else if (parts.length === 2) {
      totalSeconds = (parts[0] * 60) + parts[1];
    }
  } else {
    totalSeconds = parseFloat(matchedMovie?.duration || '120') * 60;
  }

  // 3. Compute structural progress percentage
  const percentage = totalSeconds > 0 ? (currentSeconds / totalSeconds) * 100 : 0;

  return {
    ...historyItem,
    movieTitle: matchedMovie?.title || historyItem.title || "Unknown Movie",
    posterUrl: matchedMovie?.posterUrl || historyItem.posterUrl || 'https://images.unsplash.com/photo-1440404653325-ab127d49abc1?q=80&w=400',
    displayPercentage: isNaN(percentage) ? 0 : Math.min(Math.max(percentage, 0), 100)
  };
});

        setItems(hydrated);
      } catch (err) {
        console.warn("Could not synchronize historical stream logs.", err);
      } finally {
        setLoading(false);
      }
    }
    hydrateProgressFeed();
  }, []);

  if (loading || items.length === 0) return null;

  return (
    <div className="mb-10">
      <h2 className="text-xl md:text-2xl font-black tracking-tight text-white mb-6 border-l-4 border-amber-500 pl-3">
        Continue Watching
      </h2>
      <div className="grid grid-cols-2 sm:grid-cols-3 md:grid-cols-4 lg:grid-cols-5 gap-6">
        {items.map((item) => (
          <div 
            key={item.movieId}
            onClick={() => navigate(`/watch/${item.movieId}`)}
            className="group relative bg-slate-900 rounded-2xl overflow-hidden border border-slate-800/80 cursor-pointer hover:scale-[1.02] transition duration-200 flex flex-col"
          >
            <div className="aspect-video w-full bg-slate-950 overflow-hidden relative">
              <img 
                src={item.posterUrl} 
                alt={item.movieTitle} 
                className="w-full h-full object-cover brightness-75 group-hover:brightness-90 transition duration-300" 
              />
              <div className="absolute inset-0 flex items-center justify-center opacity-0 group-hover:opacity-100 transition duration-200 bg-black/40">
                <div className="p-2.5 bg-amber-500 rounded-full text-black shadow-lg">
                  <Play className="h-4 w-4 fill-black ml-0.5" />
                </div>
              </div>
              
              <div className="absolute bottom-0 left-0 w-full h-1.5 bg-slate-800">
                <div 
                  className="h-full bg-gradient-to-r from-amber-500 to-yellow-400 transition-all duration-300"
                  style={{ width: `${item.displayPercentage}%` }}
                />
              </div>
            </div>
            <div className="p-3 flex items-center justify-between">
              <h3 className="font-bold text-white text-xs truncate max-w-[70%]">{item.movieTitle}</h3>
              <span className="text-[10px] font-mono text-slate-400 font-bold">
                {Math.round(item.displayPercentage)}%
              </span>
            </div>
          </div>
        ))}
      </div>
    </div>
  );
};