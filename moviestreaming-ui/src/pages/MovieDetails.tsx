// src/pages/MovieDetails.tsx
import React, { useEffect, useState } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import { movieService, type MovieDto, type ReviewDto } from '../services/movieService';
import { useAuth } from '../context/authContext';

export const MovieDetails: React.FC = () => {
  const { id } = useParams<{ id: string }>();
  const navigate = useNavigate();
  const { user } = useAuth();

  const [movie, setMovie] = useState<MovieDto | null>(null);
  const [loading, setLoading] = useState<boolean>(true);
  const [error, setError] = useState<string | null>(null);

  // Form State for local critique submission
  const [rating, setRating] = useState<number>(5);
  const [comment, setComment] = useState<string>('');
  const [submittingReview, setSubmittingReview] = useState<boolean>(false);
  const [reviewError, setReviewError] = useState<string | null>(null);

  useEffect(() => {
    async function fetchMovieDetails() {
      if (!id) return;
      try {
        setLoading(true);
        setError(null);
        const data = await movieService.getMovieById(id);
        setMovie(data);
      } catch (err) {
        console.error("Failed to map movie query contract", err);
        setError("The cinema asset you are targeting could not be fetched from the database pipeline.");
      } finally {
        setLoading(false);
      }
    }

    fetchMovieDetails();
  }, [id]);

  const handleReviewSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    if (!id || !user) return;

    try {
      setSubmittingReview(true);
      setReviewError(null);

      const reviewPayload = {
        userId: user.id || "", // Fallback if context has alternate key names
        rating,
        comment
      };

      await movieService.addReview(id, reviewPayload);

      // Optimistically append the review locally so the user sees it immediately
      const newLocalReview: ReviewDto = {
        id: crypto.randomUUID(), // Temp unique key
        userId: user.id || "",
        userName: user.name || "You",
        rating,
        comment
      };

      setMovie(prev => {
        if (!prev) return null;
        return {
          ...prev,
          reviews: prev.reviews ? [newLocalReview, ...prev.reviews] : [newLocalReview]
        };
      });

      // Reset Form fields
      setComment('');
      setRating(5);
    } catch (err) {
      console.error("Critique submission failed", err);
      setReviewError("Server rejected review payload structure. Verify token validation.");
    } finally {
      setSubmittingReview(false);
    }
  };

  if (loading) {
    return (
      <div className="flex min-h-screen items-center justify-center bg-slate-950 text-slate-100">
        <div className="text-center space-y-3">
          <div className="h-8 w-8 animate-spin rounded-full border-4 border-rose-500 border-t-transparent mx-auto"></div>
          <p className="text-sm tracking-wider text-slate-400">De-serializing stream configuration...</p>
        </div>
      </div>
    );
  }

  if (error || !movie) {
    return (
      <div className="flex min-h-screen items-center justify-center bg-slate-950 text-rose-400 p-6">
        <div className="max-w-md text-center p-6 bg-slate-900 border border-slate-800 rounded-2xl shadow-xl">
          <p className="font-bold mb-2">Streaming Asset Exception</p>
          <p className="text-sm text-slate-400">{error || "Movie file metadata is empty."}</p>
          <button 
            onClick={() => navigate('/')} 
            className="mt-6 px-4 py-2 bg-slate-800 hover:bg-slate-700 text-white rounded-xl text-xs font-bold transition"
          >
            Return to Core Catalog
          </button>
        </div>
      </div>
    );
  }

  return (
    <div className="min-h-screen bg-slate-950 text-slate-100 antialiased font-sans">
      
      {/* Cinematic Backdrop Banner */}
      <div className="relative w-full h-[45vh] md:h-[60vh] overflow-hidden flex items-end">
        <div className="absolute inset-0 z-0">
          <img 
            src={movie.posterUrl || 'https://images.unsplash.com/photo-1440404653325-ab127d49abc1?q=80&w=1200'} 
            alt={movie.title} 
            className="w-full h-full object-cover opacity-30 filter blur-xs scale-105"
          />
          <div className="absolute inset-0 bg-gradient-to-t from-slate-950 via-slate-950/40 to-transparent" />
        </div>

        <div className="relative z-10 max-w-6xl mx-auto w-full px-4 sm:px-6 lg:px-8 pb-8 flex flex-col md:flex-row gap-6 items-center md:items-end">
          {/* High Res Floating Poster Art Card */}
          <div className="w-40 md:w-56 aspect-[2/3] bg-slate-900 rounded-2xl overflow-hidden shadow-2xl border border-slate-800 self-center md:self-auto translate-y-6 md:translate-y-12">
            <img 
              src={movie.posterUrl || 'https://images.unsplash.com/photo-1440404653325-ab127d49abc1?q=80&w=400'} 
              alt={movie.title} 
              className="w-full h-full object-cover"
            />
          </div>

          {/* Core Core Header Information */}
          <div className="flex-1 text-center md:text-left space-y-3">
            <h1 className="text-3xl md:text-5xl font-black text-white tracking-tight drop-shadow-md">
              {movie.title}
            </h1>
            <div className="flex flex-wrap items-center justify-center md:justify-start gap-4 text-xs font-semibold text-slate-400">
              <span className="bg-slate-900 border border-slate-800 px-2.5 py-1 rounded-md text-amber-400 font-bold">
                TimeSpan: {movie.duration}
              </span>
              <span>•</span>
              <span>Released: {movie.releaseDate ? new Date(movie.releaseDate).toLocaleDateString() : 'N/A'}</span>
            </div>
          </div>
        </div>
      </div>

      {/* Main Structural Metadata Columns Layout */}
      <div className="max-w-6xl mx-auto px-4 sm:px-6 lg:px-8 pt-16 pb-24 grid grid-cols-1 lg:grid-cols-3 gap-12">
        
        {/* Left Aspect: Descriptions and Stream Media Accessors */}
        <div className="lg:col-span-2 space-y-8">
          <div className="space-y-4">
            <h2 className="text-xl font-bold border-l-4 border-rose-500 pl-3">Synopsis Overview</h2>
            <p className="text-slate-400 leading-relaxed text-sm md:text-base">
              {movie.description || "No descriptive baseline logs have been configured for this presentation artifact inside the Domain Layer domain schema layer."}
            </p>
          </div>

          {/* Trigger Play action context wrapping the videoUrl link row */}
<div className="p-6 bg-slate-900 border border-slate-800 rounded-2xl flex flex-wrap items-center justify-between gap-4">
  <div className="space-y-1">
    <p className="font-bold text-white text-sm">Ready to stream presentation file?</p>
    <p className="text-xs text-slate-500">pipeline connects securely to media servers</p>
  </div>
  <button 
    onClick={() => navigate(`/watch/${movie.id}`)} // 🚀 Updates path matrix to launch video dashboard player
    className="px-6 py-3 bg-rose-600 hover:bg-rose-700 text-white text-sm font-bold rounded-xl shadow-lg shadow-rose-950/40 transition duration-200 cursor-pointer"
  >
    🚀 Initialize Stream Engine
  </button>
</div>

          {/* Reviews/Critiques Render Engine Log Stack */}
          <div className="space-y-6">
            <h2 className="text-xl font-bold border-l-4 border-slate-700 pl-3">User Feedback Row Logs</h2>
            
            {!movie.reviews || movie.reviews.length === 0 ? (
              <p className="text-slate-500 italic text-sm p-6 bg-slate-900/30 border border-slate-900 rounded-xl">
                No critiques have been synchronized to this domain aggregate root row yet. Be the first to append an entry!
              </p>
            ) : (
              <div className="space-y-4">
                {/* Find this section inside your existing movie.reviews.map(...) loop: */}

{movie.reviews.map((rev) => (
  <div key={rev.id} className="p-5 bg-slate-900/60 border border-slate-900 rounded-xl space-y-2">
    <div className="flex justify-between items-center">
      
      {/* 🛑 CHANGE THIS LINE: Swap rev.userId to rev.userName */}
      {/* Before: <span className="text-xs font-mono tracking-wider font-bold text-slate-400">{rev.userId}</span> */}
      <span className="text-sm font-semibold tracking-wide text-slate-300">
        {rev.userName}
      </span>
      
      <span className="text-xs font-bold text-amber-400 bg-amber-400/10 px-2 py-0.5 rounded-sm">
        ⭐ {rev.rating}/5
      </span>
    </div>
    <p className="text-sm text-slate-300 leading-relaxed">{rev.comment}</p>
  </div>
))}
              </div>
            )}
          </div>
        </div>

        {/* Right Sidebar Aspect: Append Interactive Evaluation Logs Form */}
        <div className="space-y-6">
          <div className="p-6 bg-slate-900 border border-slate-800 rounded-2xl shadow-xl space-y-4">
            <h3 className="font-bold text-md text-white tracking-wide">Publish Critique Logs</h3>
            <p className="text-xs text-slate-500">Inject tracking row directly into the WebApi critique endpoints.</p>
            
            {user ? (
              <form onSubmit={handleReviewSubmit} className="space-y-4">
                <div className="space-y-2">
                  <label className="block text-xs font-bold uppercase tracking-wider text-slate-400">Score Rating</label>
                  <select 
                    value={rating} 
                    onChange={(e) => setRating(Number(e.target.value))}
                    className="w-full bg-slate-950 border border-slate-800 text-slate-200 text-sm p-3 rounded-xl focus:border-rose-500 focus:outline-none"
                  >
                    <option value={5}>⭐⭐⭐⭐⭐ (Excellent Presentation)</option>
                    <option value={4}>⭐⭐⭐⭐ (Great Resolution)</option>
                    <option value={3}>⭐⭐⭐ (Acceptable Quality)</option>
                    <option value={2}>⭐⭐ (Low Performance Profile)</option>
                    <option value={1}>⭐ (Defective Domain Structure)</option>
                  </select>
                </div>

                <div className="space-y-2">
                  <label className="block text-xs font-bold uppercase tracking-wider text-slate-400">Evaluation Commentary</label>
                  <textarea 
                    value={comment}
                    onChange={(e) => setComment(e.target.value)}
                    required
                    rows={4}
                    placeholder="Provide string content text feedback regarding this movie object configuration..."
                    className="w-full bg-slate-950 border border-slate-800 text-slate-200 text-sm p-3 rounded-xl focus:border-rose-500 focus:outline-none placeholder:text-slate-700 leading-relaxed resize-none"
                  />
                </div>

                {reviewError && <p className="text-xs text-rose-400 bg-rose-500/10 p-2 border border-rose-500/20 rounded-md">{reviewError}</p>}

                <button 
                  type="submit" 
                  disabled={submittingReview}
                  className="w-full py-3 bg-linear-to-r from-rose-600 to-amber-600 hover:opacity-90 disabled:opacity-50 text-white font-bold text-sm rounded-xl transition shadow-md"
                >
                  {submittingReview ? "Processing Command Structure..." : "Publish Review Node"}
                </button>
              </form>
            ) : (
              <div className="text-center p-4 bg-slate-950/50 rounded-xl border border-dashed border-slate-800">
                <p className="text-xs text-slate-400 mb-3">Authentication context required to publish reviews.</p>
                <a href="/login" className="inline-block text-xs font-bold text-rose-400 hover:underline">Sign In Profile</a>
              </div>
            )}
          </div>
        </div>

      </div>
    </div>
  );
};