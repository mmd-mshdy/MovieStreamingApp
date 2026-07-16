// src/types/api.ts

export interface CreateUserDto {
  name: string;
  email: string;
}

export interface CreateMovieDto {
  title: string;
  description: string;
  duration: string; // Mapped from .NET TimeSpan (e.g., "01:45:00")
}

export interface AddReviewDto {
  userId: string;
  rating: number; // 1 to 5 stars
  comment: string;
}

export interface ReviewDto {
  id: string;
  userId: string;
  rating: number;
  comment: string;
}

export interface MovieDto {
  id: string;
  title: string;
  description: string;
  duration: string;
  reviews?: ReviewDto[];
}

export interface WatchHistoryDto {
  movieId: string;
  title: string;
  posterUrl: string;
  positionSeconds: number;
  lastWatchedAt: string;
}

export interface ContinueWatchingDto {
  movieId: string;
  title: string;
  posterUrl: string;
  positionSeconds: number;
  durationSeconds: number;
  progressPercentage: number;
}
export interface UpdateProgressPayload {
  movieId: string;
  positionSeconds: number;
}