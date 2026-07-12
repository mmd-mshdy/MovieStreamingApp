// src/App.tsx
import React from 'react';
import { BrowserRouter as Router, Routes, Route, Navigate } from 'react-router-dom';
import { Home } from './pages/Home';
import { Login } from './pages/Login';
import { Register } from './pages/Register';
import { ProtectedRoute } from './components/ProtectedRoute'; 
import { AuthProvider } from './context/authContext';
import { MovieDetails } from './pages/MovieDetails';
import { WatchPlayer } from './pages/WatchPlayer';
import { WatchHistory } from './pages/WatchHistory';

function App() {
  return (
    <AuthProvider>
      <Router>
        <div className="min-h-screen bg-zinc-950 text-slate-100">
          <Routes>
            {/* 2. PROTECTED ROUTES GROUP */}
            {/* Any route placed inside this block will require a valid login token! */}
            <Route element={<ProtectedRoute />}>
              <Route path="/" element={<Home />} />
        {/* Dynamic parametric route capturing the backend Guid string */}
        <Route path="/movies/:id" element={<MovieDetails />} />
        <Route path="/watch/:id" element={<WatchPlayer />} />
        <Route path="/history" element={<WatchHistory />} />
            </Route>
            
            {/* 3. PUBLIC ROUTES */}
            <Route path="/login" element={<Login />} />
            <Route path="/register" element={<Register />} />
            
            {/* Catch-all safety fallback */}
            <Route path="*" element={<Navigate to="/" replace />} />
          </Routes>
        </div>
      </Router>
    </AuthProvider>
  );
}

export default App;