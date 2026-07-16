namespace MovieStreaming.Application.Models;

public sealed record RecommendationRequest
{
    public Guid UserId { get; init; }

    public IReadOnlyList<RecommendationInteraction> Interactions
    {
        get;
        init;
    } = [];

    public int TopN { get; init; } = 10;
}