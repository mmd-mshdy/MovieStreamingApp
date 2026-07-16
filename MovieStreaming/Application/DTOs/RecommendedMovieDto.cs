namespace MovieStreaming.Application.DTOs;

public sealed record RecommendedMovieDto(
    MovieDto Movie,
    double Score,
    string Reason);