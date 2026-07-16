namespace MovieStreaming.Application.Models;

public sealed record RecommendationMovie(
    Guid MovieId,
    string Title,
    string Description,
    IReadOnlyList<string> Genres,
    IReadOnlyList<string> CastMembers,
    int ReleaseYear,
    double AverageRating);