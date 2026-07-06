// src/services/movieService.ts

// Define the backend URL pointing to your C# Movie Controller
const API_URL = "https://localhost:7049/api/movies";

export interface MovieDto {
  id: string;
  title: string;
  description?: string;
  duration?: string;
  releaseDate?: string;
  posterUrl: string;
  videoUrl?: string;
  genreId?: string;
}

export const movieService = {
  // 1. Fetches all movies from [HttpGet] Api/Movie
  async getAllMovies(): Promise<MovieDto[]> {
    const response = await fetch(API_URL, {
      method: "GET",
      headers: {
        "Content-Type": "application/json",
        // Attach your authorization token automatically so secure routes recognize the user!
        ...authHeaderHelper() 
      },
    });

    if (!response.ok) {
      throw new Error(`Catalog stream failed with status: ${response.status}`);
    }

    return await response.json();
  },

  // 2. Fetches a single movie detail block from [HttpGet] Api/Movie/{id}
  async getMovieById(id: string): Promise<MovieDto> {
    const response = await fetch(`${API_URL}/${id}`, {
      method: "GET",
      headers: { "Content-Type": "application/json", ...authHeaderHelper() },
    });

    if (!response.ok) {
      throw new Error("Failed to retrieve movie details.");
    }

    return await response.json();
  }
};

// Helper function to safely pull your JWT token from local storage
function authHeaderHelper(): Record<string, string> {
  const token = localStorage.getItem("token");
  return token ? { Authorization: `Bearer ${token}` } : {};
}