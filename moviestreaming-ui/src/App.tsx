import React from 'react';
import { AuthProvider, useAuth } from './context/authContext';
import { Home } from './pages/Home';
import { Login } from './pages/Login';

const MainLayout: React.FC = () => {
  const { isAuthenticated } = useAuth();

  // For a basic layout switch without full routing package overhead yet:
  const path = window.location.pathname;

  if (!isAuthenticated && path !== '/login') {
    // Quick fallthrough safety lock to enforce authentication flow
    window.history.replaceState({}, '', '/login');
    return <Login />;
  }

  if (path === '/login') return <Login />;
  return <Home />;
};

function App() {
  return (
    <AuthProvider>
      <MainLayout />
    </AuthProvider>
  );
}

export default App;