// src/services/watchService.ts
import apiClient from './apiClient';

export interface ContinueWatchingDto {
  movieId: string;
  movieTitle: string;
  progressPercentage: number;
  lastWatchedAt: string;
}

export interface UpdateProgressDto {
  movieId: string;
  watchedDurationInSeconds: number;
}

export const watchService = {
  // Connects to your GetContinueWatchingQuery pipeline
  getContinueWatching: async (): Promise<ContinueWatchingDto[]> => {
    const response = await apiClient.get<ContinueWatchingDto[]>('/WatchHistory/continue-watching');
    return response.data;
  },

  // Fires your UpdateWatchProgressCommand pipeline
  updateProgress: async (progress: UpdateProgressDto): Promise<void> => {
    await apiClient.post('/WatchHistory/progress', progress);
  }
};