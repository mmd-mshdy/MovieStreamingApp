// src/services/watchHistoryService.ts
import apiClient from "./apiClient";

export interface ContinueWatchingDto {
  movieId: string;
  title: string;
  posterUrl: string;
  positionSeconds: number;
  durationSeconds: number;
  progressPercentage: number;
}

export interface WatchHistoryDto {
  movieId: string;
  movieTitle: string;
  watchedAt: string;
  positionSeconds: number;
  progressPercentage: number;
}

export interface UpdateProgressPayload {
  movieId: string;
  positionSeconds: number;
}

export const watchHistoryService = {
  // GET api/watch-history
  async getWatchHistory(): Promise<WatchHistoryDto[]> {
    const { data } = await apiClient.get<WatchHistoryDto[]>("/../api/watch-history");
    return data;
  },

  // GET api/watch-history/continue-watching
  async getContinueWatching(): Promise<ContinueWatchingDto[]> {
    const { data } = await apiClient.get<ContinueWatchingDto[]>("/../api/watch-history/continue-watching");
    return data;
  },

  // POST api/watch-history/progress
  async updateWatchProgress(payload: UpdateProgressPayload): Promise<void> {
    await apiClient.post("/../api/watch-history/progress", payload);
  }
};