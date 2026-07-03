// src/pages/Register.tsx
import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';

export const Register: React.FC = () => {
  const navigate = useNavigate();
  const [name, setName] = useState('');
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState('');
  const [success, setSuccess] = useState(false);

  const handleRegister = async (e: React.FormEvent) => {
    e.preventDefault();
    setLoading(true);
    setError('');
    
    try {
      const response = await fetch('https://localhost:7049/api/users', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ name, email, password }),
      });

      // 1. If the response failed, read it safely as text first instead of parsing as JSON blindly!
      if (!response.ok) {
        const errorText = await response.text();
        
        try {
          // Check if the error body happens to be a JSON string from your Result.Error structure
          const parsedError = JSON.parse(errorText);
          throw new Error(parsedError?.message || parsedError?.Description || 'Registration pipeline rejected.');
        } catch {
          // If it's not JSON (e.g. plain text exception or blank status code), use the raw text fallback
          throw new Error(errorText || `Server returned error status code: ${response.status}`);
        }
      }

      setSuccess(true);
      setTimeout(() => {
        navigate('/login');
      }, 2000);

    } catch (err: any) {
      setError(err.message || 'Failed to create user channel.');
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="min-h-screen bg-zinc-950 flex flex-col items-center justify-center relative px-4 overflow-hidden font-sans">
      <div className="absolute top-1/4 right-1/4 w-80 h-80 rounded-full bg-rose-600/10 filter blur-3xl animate-pulse"></div>

      <div className="w-full max-w-md relative z-10">
        <div className="text-center mb-8">
          <div className="text-2xl font-black tracking-widest text-white">
            🎬 MOVIE<span className="text-rose-600">STREAM</span>
          </div>
          <p className="text-zinc-400 text-sm mt-1">Create your movie streaming account</p>
        </div>

        <div className="bg-zinc-900/60 backdrop-blur-xl border border-zinc-800 rounded-3xl p-8 shadow-2xl">
          <h2 className="text-xl font-bold text-white mb-6 tracking-tight">Sign Up For Free</h2>

          {error && <div className="mb-4 text-xs font-semibold text-rose-400 bg-rose-500/10 p-3 rounded-xl border border-rose-500/20">{error}</div>}
          {success && <div className="mb-4 text-xs font-semibold text-emerald-400 bg-emerald-500/10 p-3 rounded-xl border border-emerald-500/20">✨ Account initialized! Forwarding to sign in portal...</div>}

          <form onSubmit={handleRegister} className="space-y-5">
            <div>
              <label className="block text-xs font-bold uppercase tracking-wider text-zinc-400 mb-2">Full Name</label>
              <input 
                type="text"
                value={name}
                onChange={(e) => setName(e.target.value)}
                placeholder="John Doe"
                required
                className="w-full px-4 py-3 bg-zinc-950/60 text-white rounded-xl border border-zinc-800 focus:border-rose-600 focus:outline-none focus:ring-1 focus:ring-rose-600 transition-all text-sm"
              />
            </div>

            <div>
              <label className="block text-xs font-bold uppercase tracking-wider text-zinc-400 mb-2">Email Address</label>
              <input 
                type="email"
                value={email}
                onChange={(e) => setEmail(e.target.value)}
                placeholder="name@domain.com"
                required
                className="w-full px-4 py-3 bg-zinc-950/60 text-white rounded-xl border border-zinc-800 focus:border-rose-600 focus:outline-none focus:ring-1 focus:ring-rose-600 transition-all text-sm"
              />
            </div>

            <div>
              <label className="block text-xs font-bold uppercase tracking-wider text-zinc-400 mb-2">Password</label>
              <input 
                type="password"
                value={password}
                onChange={(e) => setPassword(e.target.value)}
                placeholder="••••••••"
                required
                className="w-full px-4 py-3 bg-zinc-950/60 text-white rounded-xl border border-zinc-800 focus:border-rose-600 focus:outline-none focus:ring-1 focus:ring-rose-600 transition-all text-sm"
              />
            </div>

            <button
              type="submit"
              disabled={loading || success}
              className="w-full mt-4 py-3 bg-rose-600 hover:bg-rose-700 disabled:opacity-50 text-white font-bold text-sm rounded-xl shadow-lg transform active:scale-[0.98] transition-all flex items-center justify-center gap-2 cursor-pointer"
            >
              {loading ? <div className="w-5 h-5 border-2 border-white/30 border-t-white rounded-full animate-spin"></div> : 'Create Account'}
            </button>
          </form>

          <div className="mt-6 pt-4 border-t border-zinc-800/60 text-center">
            <button 
              onClick={() => navigate('/login')} 
              className="text-xs text-zinc-400 hover:text-rose-500 transition cursor-pointer font-medium"
            >
              Already have an account? <span className="text-rose-600 font-bold hover:underline">Sign In</span>
            </button>
          </div>
        </div>
      </div>
    </div>
  );
};