// src/components/ProtectedRoute.tsx
import React from 'react';
import { Navigate, Outlet } from 'react-router-dom';

export const ProtectedRoute: React.FC = () => {
  // Check if our cryptographic token is cached in the browser's local storage
  const token = localStorage.getItem('token');

  // If there is no token stream, securely kick them back to the sign-in page
  if (!token) {
    return <Navigate to="/login" replace />;
  }

  // If authenticated, render the child route layout views via React Router's Outlet
  return <Outlet />;
};