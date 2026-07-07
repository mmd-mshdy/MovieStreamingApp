// src/services/watchHistoryService.ts
import apiClient from './apiClient';
import type { ContinueWatchingDto, WatchHistoryDto } from '../types/api';

export const watchHistoryService = {
  // GET api/watch-history
  getHistory: async (): Promise<WatchHistoryDto[]> => {
    const response = await apiClient.get<WatchHistoryDto[]>('/watch-history');
    return response.data;
  },

  // GET api/watch-history/continue-watching
  getContinueWatching: async (): Promise<ContinueWatchingDto[]> => {
    const response = await apiClient.get<ContinueWatchingDto[]>('/watch-history/continue-watching');
    return response.data;
  }
};