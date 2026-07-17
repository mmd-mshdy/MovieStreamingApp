namespace MovieStreaming.Application.Models;

public sealed record RecommendationMovie(
    Guid MovieId,
    string Title,
    string Description,
    IReadOnlyList<string> Genres,
    int ReleaseYear,
    double AverageRating);