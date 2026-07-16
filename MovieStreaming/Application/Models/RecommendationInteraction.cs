namespace MovieStreaming.Application.Models;

public sealed record RecommendationInteraction(
    Guid MovieId,
    double WatchPercentage,
    bool Completed,
    int? Rating,
    bool InWatchlist);