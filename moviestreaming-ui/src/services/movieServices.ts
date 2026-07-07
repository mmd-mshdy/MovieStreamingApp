// src/services/movieService.ts
import apiClient from "./apiClient";

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
  duration: string; // Comes from backend TimeSpan string
  releaseDate?: string;
  posterUrl?: string;
  videoUrl?: string;
  reviews?: ReviewDto[]; // Included when fetching single movie details
}

export interface AddReviewDto {
  userId: string;
  rating: number;
  comment: string;
}

export const movieService = {
  /**
   * Fetch all movies (Optional fallback if implemented in backend)
   */
  async getAllMovies(): Promise<MovieDto[]> {
    const response = await apiClient.get<MovieDto[]>("/Movie");
    return response.data;
  },

  /**
   * Fetch a single movie including its reviews by its Guid ID
   * Maps directly to backend: [HttpGet("{id:Guid}")] GetMovieById
   */
  async getMovieById(id: string): Promise<MovieDto> {
    const response = await apiClient.get<MovieDto>(`/Movie/${id}`);
    return response.data;
  },

  /**
   * Create a new movie entry
   * Maps directly to backend: [HttpPost("Add Movie")] CreateMovie
   */
  async createMovie(movieData: Omit<MovieDto, "id" | "reviews">): Promise<MovieDto> {
    const response = await apiClient.post<MovieDto>("/Movie/Add Movie", movieData);
    return response.data;
  },

  /**
   * Post a user critique review onto a specific movie
   * Maps directly to backend: [HttpPost("Add Review")] AddReview([FromBody]AddReviewDto dto, Guid movieId)
   */
  async addReview(movieId: string, reviewData: AddReviewDto): Promise<any> {
    const response = await apiClient.post("/Movie/Add Review", reviewData, {
      params: { movieId }, // Appends ?movieId=guid to the query string
    });
    return response.data;
  },
};