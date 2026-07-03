// src/components/MovieCard.tsx
import React from 'react';

export interface MovieCardProps {
  id: string;
  title: string;
  posterUrl: string;
  releaseYear?: number;
  rating?: number;
}

const MovieCard: React.FC<MovieCardProps> = ({ title, posterUrl, releaseYear, rating }) => {
  return (
    <div className="group relative min-w-[180px] w-48 h-72 rounded-lg overflow-hidden bg-zinc-900 cursor-pointer transition-all duration-300 ease-out transform hover:scale-105 hover:z-10 shadow-md hover:shadow-2xl">
      {/* Movie Poster image */}
      <img 
        src={posterUrl || 'https://via.placeholder.com/192x288?text=No+Poster'} 
        alt={title} 
        className="w-full h-full object-cover transition-transform duration-300 group-hover:opacity-40"
      />

      {/* Hover Overlay Details */}
      <div className="absolute inset-0 p-4 flex flex-col justify-end opacity-0 group-hover:opacity-100 transition-opacity duration-300 bg-gradient-to-t from-black via-black/70 to-transparent">
        <h3 className="text-white font-bold text-sm leading-tight line-clamp-2 mb-1">
          {title}
        </h3>
        
        <div className="flex items-center justify-between text-xs text-zinc-400">
          {releaseYear && <span>{releaseYear}</span>}
          {rating && (
            <span className="flex items-center text-amber-400 font-semibold">
              ⭐ {rating.toFixed(1)}
            </span>
          )}
        </div>

        {/* Quick Play Action Button */}
        <button className="mt-3 w-full py-1.5 bg-red-600 hover:bg-red-700 text-white font-medium text-xs rounded transition-colors">
          ▶ Play Now
        </button>
      </div>
    </div>
  );
};

export default MovieCard;