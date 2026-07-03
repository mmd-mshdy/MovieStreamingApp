// src/services/apiClient.ts
import axios from 'axios';

const apiClient = axios.create({
  baseURL: 'https://localhost:7049/api', // Match your C# WebApi port
  headers: {
    'Content-Type': 'application/json',
  },
});

// Automatically inject the token into every backend request
apiClient.interceptors.request.use((config) => {
  const token = localStorage.getItem('token');
  if (token && config.headers) {
    config.headers.Authorization = `Bearer ${token}`;
  }
  return config;
}, (error) => {
  return Promise.reject(error);
});

export default apiClient;