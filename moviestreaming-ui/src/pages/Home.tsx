// src/pages/Home.tsx
import React, { useEffect, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { movieService, type MovieDto } from '../services/movieService';
import { useAuth } from '../context/authContext';

// Clean, high-res fallback assets to ensure the UI looks incredible during local testing
const mockMovies = [
  { id: '1', title: 'Inception', posterUrl: 'https://image.tmdb.org/t/p/w500/9gk7adHY9CjST6Y99PaIQpSRfsQ.jpg', rating: 8.8, releaseYear: 2010, duration: '2h 28m' },
  { id: '2', title: 'Interstellar', posterUrl: 'https://image.tmdb.org/t/p/w500/gEU2QniE6E77NIvKCUgCYJu7stg.jpg', rating: 8.6, releaseYear: 2014, duration: '2h 49m' },
  { id: '3', title: 'The Dark Knight', posterUrl: 'https://image.tmdb.org/t/p/w500/qJ2tWGB2mS6tC86m1Xw3gIuK6Y7.jpg', rating: 9.0, releaseYear: 2008, duration: '2h 32m' },
  { id: '4', title: 'Blade Runner 2048', posterUrl: 'https://image.tmdb.org/t/p/w500/gajva2L0vI4Z6wXSg6z6w66Z67f.jpg', rating: 8.0, releaseYear: 2017, duration: '2h 44m' },
  { id: '5', title: 'Mad Max: Fury Road', posterUrl: 'https://image.tmdb.org/t/p/w500/8tZYtuWeox6Jb8clvc4gY07U69R.jpg', rating: 8.1, releaseYear: 2015, duration: '2h 00m' }
];

export const Home: React.FC = () => {
  const { logout, user } = useAuth();
  const navigate = useNavigate(); // React Router programmatic navigation engine
  const [movies, setMovies] = useState<any[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState('');

  useEffect(() => {
    async function loadMovies() {
      try {
        setLoading(true);
        setError('');
        const data = await movieService.getAllMovies();
        
        if (data && data.length > 0) {
          // Map backend data to UI-friendly format safely
          const formatted = data.map((m: MovieDto) => ({
            ...m,
            rating: 8.5, // Placeholder value if not exposed on this DTO row layer yet
            releaseYear: m.releaseDate ? new Date(m.releaseDate).getFullYear() : 'N/A',
            duration: m.duration || 'N/A'
          }));
          setMovies(formatted);
        } else {
          setMovies(mockMovies);
        }
      } catch (err) {
        console.error("Fetch failed, reverting to resilient mock values:", err);
        setMovies(mockMovies);
        setError("Unable to connect to the backend movie catalog service.");
      } finally {
        setLoading(false);
      }
    }
    loadMovies();
  }, []);

  // Set up a marquee movie to display on the massive hero spotlight billboard
  const spotlightMovie = movies[0] || mockMovies[0];

  // Structural handler pushing user context dynamically into the router path matrix
  const handleMovieNavigation = (movieId: string) => {
    navigate(`/movies/${movieId}`);
  };

  return (
    <div className="min-h-screen bg-brandDark text-slate-100 font-sans antialiased selection:bg-brandAccent selection:text-white">
      {/* Premium Header Navigation */}
      <nav className="border-b border-slate-800/60 bg-slate-950/80 backdrop-blur-md sticky top-0 z-50 transition-all">
        <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 h-16 flex items-center justify-between">
          <div 
            onClick={() => navigate('/')}
            className="text-xl font-black tracking-widest text-white cursor-pointer hover:opacity-95"
          >
            🎬 MOVIE<span className="text-brandAccent">STREAM</span>
          </div>
          <div className="flex items-center gap-6">
            <span className="text-sm font-medium text-slate-400 hidden sm:inline">Hi, {user?.name || 'Guest'}</span>
            {user ? (
              <button 
                onClick={logout}
                className="px-4 py-2 text-sm font-semibold bg-slate-800 hover:bg-slate-700 rounded-xl text-white shadow-sm active:scale-95 transition duration-200 cursor-pointer"
              >
                Log Out
              </button>
            ) : (
              <a href="/login" className="px-5 py-2 text-sm font-bold bg-brandAccent hover:bg-rose-700 rounded-xl text-white shadow-md shadow-rose-950 transition duration-200">Sign In</a>
            )}
          </div>
        </div>
      </nav>

      {/* Hero Spotlight Billboard */}
      <div className="relative w-full h-[60vh] md:h-[75vh] flex items-end border-b border-slate-900/40">
        <div className="absolute inset-0 z-0">
          <img 
            src="https://images.unsplash.com/photo-1536440136628-849c177e76a1?q=80&w=1600" 
            alt="Cinematic Backdrop" 
            className="w-full h-full object-cover opacity-25 filter blur-[0.5px]"
          />
          {/* Layered vignette masking to dissolve into the page body */}
          <div className="absolute inset-0 bg-gradient-to-t from-brandDark via-brandDark/50 to-transparent"></div>
          <div className="absolute inset-0 bg-gradient-to-r from-brandDark via-transparent to-transparent"></div>
        </div>

        <div className="relative z-10 max-w-7xl mx-auto w-full px-4 sm:px-6 lg:px-8 pb-14 md:pb-24">
          <span className="inline-flex items-center gap-1.5 px-3 py-1 rounded-full text-xs font-bold uppercase tracking-widest bg-brandAccent/10 text-brandAccent border border-brandAccent/20">
            🎬 Talk Feature Presentation
          </span>
          <h1 className="text-4xl md:text-6xl font-black text-white mt-4 mb-4 tracking-tight max-w-3xl drop-shadow-lg">
            {spotlightMovie?.title}
          </h1>
          <p className="text-slate-300 text-base md:text-lg max-w-2xl font-medium leading-relaxed mb-6 drop-shadow-sm">
            Stream your absolute favorite movies and originals directly through your high-concurrency .NET Clean Architecture rendering pipeline.
          </p>
          <div className="flex flex-wrap gap-4">
            <button 
              onClick={() => spotlightMovie && handleMovieNavigation(spotlightMovie.id)}
              className="px-6 py-3 bg-brandAccent hover:bg-rose-700 text-white font-bold text-sm md:text-base rounded-xl shadow-lg shadow-rose-900/30 transform active:scale-95 transition duration-200 cursor-pointer"
            >
              ▶ Play Now
            </button>
            <button 
              onClick={() => spotlightMovie && handleMovieNavigation(spotlightMovie.id)}
              className="px-6 py-3 bg-slate-800/80 hover:bg-slate-700 text-white font-bold text-sm md:text-base rounded-xl border border-slate-700 transition duration-200 cursor-pointer"
            >
              ℹ Details
            </button>
          </div>
        </div>
      </div>

      {/* Main Content Sections */}
      <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 py-12 relative z-20">
        
        {/* Loading Skeletons */}
        {loading && (
          <div>
            <div className="h-7 w-40 bg-slate-800 animate-pulse rounded-lg mb-6"></div>
            <div className="grid grid-cols-2 sm:grid-cols-3 md:grid-cols-4 lg:grid-cols-5 gap-6">
              {[...Array(5)].map((_, i) => (
                <div key={i} className="animate-pulse bg-slate-800 aspect-[2/3] rounded-2xl"></div>
              ))}
            </div>
          </div>
        )}

        {error && <p className="text-rose-400 bg-rose-500/10 p-4 rounded-xl border border-rose-500/20 max-w-xl mx-auto text-center font-medium mb-8">{error}</p>}

        {!loading && (
          <div className="space-y-14">
            {/* Category 1: Trending Grid */}
            <div>
              <h2 className="text-xl md:text-2xl font-black tracking-tight text-white mb-6 border-l-4 border-brandAccent pl-3">
                Trending Now
              </h2>
              <div className="grid grid-cols-2 sm:grid-cols-3 md:grid-cols-4 lg:grid-cols-5 gap-6">
                {movies.map((movie) => (
                  <div 
                    key={movie.id} 
                    onClick={() => handleMovieNavigation(movie.id)}
                    className="group relative bg-slate-900 rounded-2xl overflow-hidden border border-slate-800 hover:border-slate-700/80 hover:scale-[1.03] transition-all duration-300 shadow-md hover:shadow-2xl flex flex-col cursor-pointer"
                  >
                    <div className="aspect-[2/3] w-full bg-slate-950 overflow-hidden relative">
                      <img 
                        src={movie.posterUrl || 'https://images.unsplash.com/photo-1440404653325-ab127d49abc1?q=80&w=400'} 
                        alt={movie.title}
                        className="w-full h-full object-cover group-hover:scale-105 transition duration-500"
                      />
                      <div className="absolute inset-0 bg-gradient-to-t from-slate-950 via-transparent opacity-0 group-hover:opacity-100 transition duration-300 flex items-end p-4">
                        <button className="w-full py-2 bg-brandAccent hover:bg-rose-700 text-white rounded-xl text-xs font-bold shadow-md shadow-rose-900/40 cursor-pointer">
                          Watch Stream
                        </button>
                      </div>
                    </div>
                    <div className="p-4 flex-grow flex flex-col justify-between">
                      <h3 className="font-bold text-white text-sm line-clamp-1 group-hover:text-brandAccent transition duration-150">{movie.title}</h3>
                      <div className="flex items-center justify-between text-xs text-slate-400 mt-2">
                        <span className="flex items-center text-amber-400 font-bold gap-1">⭐ {movie.rating?.toFixed(1) || '8.5'}</span>
                        <span>{movie.releaseYear}</span>
                        <span className="px-1.5 py-0.5 rounded bg-slate-800 text-slate-300 font-mono text-[10px]">{movie.duration}</span>
                      </div>
                    </div>
                  </div>
                ))}
              </div>
            </div>

            {/* Category 2: Recommended Row Preview */}
            <div>
              <h2 className="text-xl md:text-2xl font-black tracking-tight text-white mb-6 border-l-4 border-slate-700 pl-3">
                Recommended For You
              </h2>
              <div className="grid grid-cols-2 sm:grid-cols-3 md:grid-cols-4 lg:grid-cols-5 gap-6 opacity-85 hover:opacity-100 transition duration-300">
                {[...movies].reverse().map((movie) => (
                  <div 
                    key={`rec-${movie.id}`} 
                    onClick={() => handleMovieNavigation(movie.id)}
                    className="group relative bg-slate-900 rounded-2xl overflow-hidden border border-slate-800/80 flex flex-col cursor-pointer hover:scale-102 transition duration-200"
                  >
                    <div className="aspect-[2/3] w-full bg-slate-950 overflow-hidden relative">
                      <img src={movie.posterUrl} alt={movie.title} className="w-full h-full object-cover brightness-90 group-hover:brightness-100 transition" />
                    </div>
                  </div>
                ))}
              </div>
            </div>
          </div>
        )}
      </div>
    </div>
  );
};