import { authService } from "./authService";

const API_URL = "https://localhost:7049/Api/Movie";

export interface MovieDto {
  id: string;
  title: string;
  description: string;
  duration: string;
  releaseDate: string;
  posterUrl: string;
  videoUrl: string;
}

export const movieService = {
  async getAllMovies(): Promise<MovieDto[]> {
    const response = await fetch(API_URL);
    if (!response.ok) throw new Error("Failed to load movies");
    return response.json();
  },

  async createMovie(movieData: Omit<MovieDto, 'id'>): Promise<MovieDto> {
    const response = await fetch(`${API_URL}/Add Movie`, {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
        ...authService.getAuthHeader()
      },
      body: JSON.stringify(movieData),
    });

    if (!response.ok) throw new Error("Unauthorized or invalid movie structure");
    return response.json();
  }
};