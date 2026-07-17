import apiClient from "./apiClient";
import type { MovieDto } from "./movieService";

export interface RecommendedMovieDto {
  movie: MovieDto;
  score: number;
  reason: string;
}

export interface RecommendationStatus {
  ready: boolean;
}

export const recommendationService = {
  async getRecommendations(
    count = 10
  ): Promise<RecommendedMovieDto[]> {
    const response = await apiClient.get<
      RecommendedMovieDto[]
    >("/recommendations", {
      params: {
        count,
      },
    });

    return response.data;
  },

  async getStatus():
    Promise<RecommendationStatus> {
    const response =
      await apiClient.get<RecommendationStatus>(
        "/recommendations/status"
      );

    return response.data;
  },
};