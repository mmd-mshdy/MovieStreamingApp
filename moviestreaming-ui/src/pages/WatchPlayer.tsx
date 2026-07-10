// src/pages/WatchPlayer.tsx
import React, { useEffect, useRef, useState } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import { movieService, type MovieDto } from '../services/movieService';
import { watchHistoryService } from '../services/watchHistoryService';
import { ArrowLeft, RefreshCw } from 'lucide-react';

export const WatchPlayer: React.FC = () => {
  const { id } = useParams<{ id: string }>();
  const navigate = useNavigate();
  const videoRef = useRef<HTMLVideoElement | null>(null);

  const [movie, setMovie] = useState<MovieDto | null>(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState('');

  // 1. Ingest Movie details configuration
  useEffect(() => {
    async function loadPlaybackData() {
      if (!id) return;
      try {
        setLoading(true);
        const data = await movieService.getMovieById(id);
        setMovie(data);
      } catch (err) {
        console.error("Playback configuration error:", err);
        setError("Could not resolve streaming server descriptors.");
      } finally {
        setLoading(false);
      }
    }
    loadPlaybackData();
  }, [id]);

  // 2. Automated Heartbeat Telemetry Pipeline
  useEffect(() => {
    if (!movie || !id) return;

    // Heartbeat reporting function
    const reportProgressHeartbeat = async () => {
      if (!videoRef.current) return;
      
      const currentSeconds = Math.floor(videoRef.current.currentTime);
      
      // Prevent reporting empty frames/unstarted video sequences
      if (currentSeconds <= 0) return;

      try {
        await watchHistoryService.updateWatchProgress({
          movieId: id,
          positionSeconds: currentSeconds
        });
        console.log(`Telemetry synced: ${currentSeconds}s for asset ${movie.title}`);
      } catch (err) {
        console.warn("Heartbeat connection frame dropped:", err);
      }
    };

    // Spin up an interval logging telemetry metrics every 10 seconds
    const heartbeatInterval = setInterval(reportProgressHeartbeat, 10000);

    // Clean up timer and fire a final positional update when leaving the page
    return () => {
      clearInterval(heartbeatInterval);
      reportProgressHeartbeat();
    };
  }, [movie, id]);

  if (loading) {
    return (
      <div className="flex h-screen items-center justify-center bg-slate-950 text-slate-100">
        <div className="text-center space-y-3">
          <RefreshCw className="h-8 w-8 animate-spin text-brandAccent mx-auto" />
          <p className="text-xs font-mono tracking-widest text-slate-500 uppercase">Allocating streaming channel...</p>
        </div>
      </div>
    );
  }

  return (
    <div className="h-screen w-screen bg-black relative overflow-hidden group select-none">
      
      {/* Cinematic HUD Overlay Top Bar Controls */}
      <div className="absolute top-0 left-0 w-full p-6 bg-gradient-to-b from-black/80 to-transparent z-50 opacity-0 group-hover:opacity-100 transition-opacity duration-300 flex items-center gap-4">
        <button 
          onClick={() => navigate(`/movies/${id}`)}
          className="p-3 bg-slate-900/60 hover:bg-slate-800 border border-slate-800 rounded-xl text-white transition cursor-pointer"
        >
          <ArrowLeft className="h-5 w-5" />
        </button>
        <div>
          <h1 className="text-lg font-black text-white tracking-wide">{movie?.title || 'Streaming Asset'}</h1>
          <p className="text-xs text-slate-400 font-mono">Secure .NET Media Delivery Stream Active</p>
        </div>
      </div>

      {/* Primary Video Canvas Core */}
      <video 
        ref={videoRef}
        src={movie?.videoUrl || "https://commondatastorage.googleapis.com/gtv-videos-bucket/sample/BigBuckBunny.mp4"} 
        controls
        autoPlay
        className="h-full w-full object-contain"
      />

    </div>
  );
};