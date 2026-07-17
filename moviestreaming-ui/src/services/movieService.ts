// src/services/movieService.ts
import apiClient from "./apiClient";

export interface ReviewDto {
  id: string;
  userId: string;
  userName: string;
  rating: number;
  comment: string;
}

export interface AddReviewDto {
  userId: string;
  rating: number;
  comment: string;
}

export interface MovieDto {
  id: string;
  title: string;
  description: string;
  duration: string;
  releaseDate: string;
  posterUrl: string;
  videoUrl: string;
  reviews: ReviewDto[];
  genres: string[];
}

export const movieService = {
  // Using the apiClient instance ensures JWT tokens are attached automatically
  async getAllMovies(): Promise<MovieDto[]> {
  const response =
    await apiClient.get<MovieDto[]>("/movies");

  return response.data;
  console.log("Movie API raw response:", response.data);
},

  // GET Api/Movie/{id:Guid} - Maps to your backend GetMovieById action
  async getMovieById(id: string): Promise<MovieDto> {
    const { data } = await apiClient.get<MovieDto>(`/movies/${id}`);
    return data;
  },

  async createMovie(movieData: Omit<MovieDto, 'id' | 'reviews'>): Promise<MovieDto> {
    const { data } = await apiClient.post<MovieDto>("/movies/Add Movie", movieData);
    return data;
  },

  // POST Api/Movie/Add Review?movieId={movieId} - Maps to your backend AddReview action
  async addReview(movieId: string, reviewData: AddReviewDto): Promise<any> {
    // Inject the movieId parameter directly into the path segment string
    const { data } = await apiClient.post(`/movies/${movieId}/reviews`, reviewData);
    return data;
  },
  async searchMovies(
  searchTerm: string
): Promise<MovieDto[]> {
  const normalizedSearchTerm =
    searchTerm.trim();

  if (!normalizedSearchTerm) {
    return [];
  }

  const { data } =
    await apiClient.get<MovieDto[]>(
      "/movies/search",
      {
        params: {
          query: normalizedSearchTerm,
        },
      }
    );

  return data;
},
};