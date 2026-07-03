// src/components/MovieRow.tsx
import React from 'react';
import MovieCard, {type MovieCardProps } from './MovieCard';

interface MovieRowProps {
  title: string;
  movies: MovieCardProps[];
}

const MovieRow: React.FC<MovieRowProps> = ({ title, movies }) => {
  return (
    <div className="my-8 px-8">
      <h2 className="text-white text-xl font-semibold mb-4 tracking-wide">{title}</h2>
      
      {/* Scrollable Container */}
      <div className="flex space-x-4 overflow-x-auto pb-4 scrollbar-hide scroll-smooth">
        {movies.map((movie) => (
          <MovieCard key={movie.id} {...movie} />
        ))}
      </div>
    </div>
  );
};

export default MovieRow;