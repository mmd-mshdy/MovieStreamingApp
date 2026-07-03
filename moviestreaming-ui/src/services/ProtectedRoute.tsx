// src/components/ProtectedRoute.tsx
import { Navigate, Outlet } from 'react-router-dom';

const ProtectedRoute = () => {
  const token = localStorage.getItem('token');

  // If there is no token, boot them back to the login page
  if (!token) {
    return <Navigate to="/login" replace />;
  }

  // If they are logged in, render the child pages
  return <Outlet />;
};

export default ProtectedRoute;