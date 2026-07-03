// src/pages/Login.tsx
import React, { useState } from 'react';
import { useAuth } from '../context/authContext';
import { useNavigate } from 'react-router-dom';

export const Login: React.FC = () => {
  const { login } = useAuth();
  const navigate = useNavigate();
  
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState('');

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setLoading(true);
    setError('');
    try {
      await login(email, password);
      navigate('/'); 
    } catch (err) {
      setError('Invalid credentials. Please verify your login token stream.');
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="min-h-screen bg-zinc-950 flex flex-col items-center justify-center relative px-4 overflow-hidden font-sans">
      <div className="absolute top-1/4 left-1/4 w-80 h-80 rounded-full bg-rose-600/10 filter blur-3xl animate-pulse"></div>
      <div className="absolute bottom-1/4 right-1/4 w-96 h-96 rounded-full bg-blue-900/10 filter blur-3xl animate-pulse delay-700"></div>

      <div className="w-full max-w-md relative z-10">
        <div className="text-center mb-8">
          <div className="text-2xl font-black tracking-widest text-white">
            🎬 MOVIE<span className="text-rose-600">STREAM</span>
          </div>
          <p className="text-zinc-400 text-sm mt-1">Access your clean architecture streaming portal</p>
        </div>

        <div className="bg-zinc-900/60 backdrop-blur-xl border border-zinc-800 rounded-3xl p-8 shadow-2xl">
          <h2 className="text-xl font-bold text-white mb-6 tracking-tight">Sign In to Your Account</h2>

          {error && (
            <div className="mb-4 text-xs font-semibold text-rose-400 bg-rose-500/10 p-3 rounded-xl border border-rose-500/20">
              {error}
            </div>
          )}

          <form onSubmit={handleSubmit} className="space-y-5">
            <div>
              <label className="block text-xs font-bold uppercase tracking-wider text-zinc-400 mb-2">
                Email Address
              </label>
              <input 
                type="email"
                value={email}
                onChange={(e) => setEmail(e.target.value)}
                placeholder="name@domain.com"
                required
                className="w-full px-4 py-3 bg-zinc-950/60 text-white rounded-xl border border-zinc-800 focus:border-rose-600 focus:outline-none focus:ring-1 focus:ring-rose-600 transition-all placeholder:text-zinc-600 text-sm"
              />
            </div>

            <div>
              <div className="flex justify-between items-center mb-2">
                <label className="block text-xs font-bold uppercase tracking-wider text-zinc-400">
                  Password
                </label>
              </div>
              <input 
                type="password"
                value={password}
                onChange={(e) => setPassword(e.target.value)}
                placeholder="••••••••"
                required
                className="w-full px-4 py-3 bg-zinc-950/60 text-white rounded-xl border border-zinc-800 focus:border-rose-600 focus:outline-none focus:ring-1 focus:ring-rose-600 transition-all placeholder:text-zinc-600 text-sm"
              />
            </div>

            <button
              type="submit"
              disabled={loading}
              className="w-full mt-4 py-3 bg-rose-600 hover:bg-rose-700 disabled:opacity-50 text-white font-bold text-sm rounded-xl shadow-lg shadow-rose-950/50 transform active:scale-[0.98] transition-all duration-150 flex items-center justify-center gap-2 cursor-pointer"
            >
              {loading ? <div className="w-5 h-5 border-2 border-white/30 border-t-white rounded-full animate-spin"></div> : 'Sign In'}
            </button>
          </form>

          {/* Interactive Navigation Toggle Link */}
          <div className="mt-6 pt-4 border-t border-zinc-800/60 text-center">
            <button 
              onClick={() => navigate('/register')} 
              className="text-xs text-zinc-400 hover:text-rose-500 transition cursor-pointer font-medium"
            >
              Don't have an account? <span className="text-rose-600 font-bold hover:underline">Create one</span>
            </button>
          </div>
        </div>
      </div>
    </div>
  );
};