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
      const rawHistory = await watchHistoryService.getContinueWatching();
      const catalog = await movieService.getAllMovies();

      // 🚨 CRITICAL DEBUG LOGS — look at these in your browser console!
      console.log("=== WATCH HISTORY RAW DATA ===", rawHistory);
      console.log("=== MOVIE CATALOG SAMPLE ===", catalog?.[0]);

      if (!rawHistory || rawHistory.length === 0) {
        setItems([]);
        return;
      }

        const hydrated = rawHistory.map((historyItem: any) => {
  // 1. Resolve Movie ID with case-insensitivity
  const historyMovieId = historyItem.movieId ?? historyItem.MovieId;
  const matchedMovie = catalog.find((m: any) => (m.id ?? m.Id) === historyMovieId);

  // 2. Ultimate robust time string-to-seconds converter
  const parseTimeToSeconds = (timeInput: any): number => {
    if (!timeInput) return 0;
    if (typeof timeInput === 'number') return timeInput;
    
    // Fallback if parsed as a .NET serialized object structure
    if (typeof timeInput === 'object') {
      const h = timeInput.hours ?? timeInput.Hours ?? 0;
      const m = timeInput.minutes ?? timeInput.Minutes ?? 0;
      const s = timeInput.seconds ?? timeInput.Seconds ?? 0;
      return (h * 3600) + (m * 60) + s;
    }
    
    if (typeof timeInput === 'string') {
      const cleanStr = timeInput.trim();
      if (!cleanStr.includes(':')) return parseFloat(cleanStr) || 0;

      let timePart = cleanStr;
      let daySeconds = 0;

      // Strip .NET day prefix if present (e.g., "1.02:30:00")
      if (cleanStr.includes('.') && cleanStr.indexOf('.') < cleanStr.indexOf(':')) {
        const dayParts = cleanStr.split('.');
        const days = parseInt(dayParts[0], 10);
        if (!isNaN(days)) daySeconds = days * 86400;
        timePart = dayParts.slice(1).join('.');
      }

      // Drop fractional milliseconds if present (e.g., "02:30:00.1234567")
      if (timePart.includes('.') && timePart.indexOf('.') > timePart.indexOf(':')) {
        timePart = timePart.split('.')[0];
      }

      const parts = timePart.split(':').map(Number);
      if (parts.length === 3) {
        return daySeconds + (parts[0] * 3600) + (parts[1] * 60) + parts[2];
      } else if (parts.length === 2) {
        return daySeconds + (parts[0] * 60) + parts[1];
      }
    }
    return 0;
  };

  // 3. Convert fields to clean numeric seconds values
  const rawPosition = historyItem.lastPosition ?? historyItem.LastPosition;
  const rawDuration = matchedMovie?.duration ?? matchedMovie?.duration;

  const currentSeconds = parseTimeToSeconds(rawPosition);
  const totalSeconds = parseTimeToSeconds(rawDuration) || (120 * 60); // 120min fallback

  // 4. Calculate final percentage ratio
  const percentage = totalSeconds > 0 ? (currentSeconds / totalSeconds) * 100 : 0;

  // 🚨 MATH CHECK LOG - Look at this output in your console
  console.log(`Movie: ${matchedMovie?.title || 'Unknown'} -> Pos: ${currentSeconds}s / Total: ${totalSeconds}s -> Progress: ${percentage}%`);

  return {
    ...historyItem,
    movieTitle: matchedMovie?.title ?? historyItem.title ?? "Unknown Movie",
    posterUrl: matchedMovie?.posterUrl ?? historyItem.posterUrl ?? 'https://images.unsplash.com/photo-1440404653325-ab127d49abc1?q=80&w=400',
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