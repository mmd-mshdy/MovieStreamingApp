import React, { useEffect, useState } from 'react';
import { movieService, type MovieDto } from '../services/movieService';
import { useAuth } from '../context/authContext';

export const Home: React.FC = () => {
  const { logout, user } = useAuth();
  const [movies, setMovies] = useState<MovieDto[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState('');

  useEffect(() => {
    const fetchMovies = async () => {
      try {
        const data = await movieService.getAllMovies();
        setMovies(data);
      } catch (err) {
        setError('Failed to fetch the catalog stream.');
      } finally {
        setLoading(false);
      }
    };
    fetchMovies();
  }, []);

  return (
    <div className="min-h-screen bg-brandDark text-slate-100">
      {/* Sleek Navigation Header */}
      <nav className="border-b border-slate-800 bg-slate-900/50 backdrop-blur-md sticky top-0 z-50">
        <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 h-16 flex items-center justify-between">
          <div className="text-xl font-bold tracking-wider text-white">
            🎬 MOVIE<span className="text-brandAccent">STREAM</span>
          </div>
          <div className="flex items-center gap-4">
            <span className="text-sm text-slate-400 hidden sm:inline">Hi, {user?.name || 'Guest'}</span>
            {user ? (
              <button 
                onClick={logout}
                className="px-4 py-2 text-sm font-medium bg-slate-800 hover:bg-slate-700 rounded-lg text-white transition"
              >
                Log Out
              </button>
            ) : (
              <a href="/login" className="px-4 py-2 text-sm font-medium bg-brandAccent hover:bg-rose-700 rounded-lg text-white transition">Sign In</a>
            )}
          </div>
        </div>
      </nav>

      {/* Hero Interactive Space */}
      <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 py-8">
        <div className="mb-10 p-8 md:p-12 rounded-3xl bg-gradient-to-r from-rose-900/40 to-slate-900 border border-slate-800 relative overflow-hidden">
          <div className="relative z-10 max-w-2xl">
            <span className="text-brandAccent uppercase tracking-widest text-xs font-bold">Featured Stream</span>
            <h1 className="text-4xl md:text-5xl font-extrabold text-white mt-2 mb-4">Unlimited Movies, TV Shows & More</h1>
            <p className="text-slate-300 text-lg">Stream your favorite titles instantly powered by high-concurrency .NET 10 micro-kernels.</p>
          </div>
        </div>

        {/* Content Stream Grids */}
        <h2 className="text-2xl font-bold tracking-tight text-white mb-6">Trending Now</h2>
        
        {loading && (
          <div className="grid grid-cols-2 md:grid-cols-4 lg:grid-cols-5 gap-6">
            {[...Array(5)].map((_, i) => (
              <div key={i} className="animate-pulse bg-slate-800 aspect-[2/3] rounded-2xl"></div>
            ))}
          </div>
        )}

        {error && <p className="text-rose-400 bg-rose-500/10 p-4 rounded-xl border border-rose-500/20">{error}</p>}

        {!loading && !error && (
          <div className="grid grid-cols-2 sm:grid-cols-3 md:grid-cols-4 lg:grid-cols-5 gap-6">
            {movies.map((movie) => (
              <div 
                key={movie.id} 
                className="group relative bg-slate-900 rounded-2xl overflow-hidden shadow-md hover:shadow-xl hover:scale-105 border border-slate-800 hover:border-slate-700 transition duration-300 flex flex-col cursor-pointer"
              >
                <div className="aspect-[2/3] w-full bg-slate-950 overflow-hidden relative">
                  <img 
                    src={movie.posterUrl || 'https://images.unsplash.com/photo-1440404653325-ab127d49abc1?q=80&w=400'} 
                    alt={movie.title}
                    className="w-full h-full object-cover group-hover:opacity-80 transition"
                  />
                  <div className="absolute inset-0 bg-gradient-to-t from-slate-950 via-transparent opacity-0 group-hover:opacity-100 transition flex items-end p-4">
                    <button className="w-full py-2 bg-brandAccent text-white rounded-xl text-sm font-semibold shadow shadow-rose-600">Watch Now</button>
                  </div>
                </div>
                <div className="p-4 flex-grow flex flex-col justify-between">
                  <h3 className="font-semibold text-white text-sm line-clamp-1 group-hover:text-brandAccent transition">{movie.title}</h3>
                  <div className="flex items-center justify-between text-xs text-slate-400 mt-1">
                    <span>{movie.releaseDate ? new Date(movie.releaseDate).getFullYear() : 'N/A'}</span>
                    <span className="px-1.5 py-0.5 rounded bg-slate-800 text-slate-300 font-mono">{movie.duration}</span>
                  </div>
                </div>
              </div>
            ))}
          </div>
        )}
      </div>
    </div>
  );
};