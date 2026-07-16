using MovieStreaming.Application.Models;

namespace MovieStreaming.Application.Interfaces;

public interface IRecommendationService
{
    Task<IReadOnlyList<RecommendationResult>>
        GetRecommendationsAsync(
            RecommendationRequest request,
            CancellationToken cancellationToken = default);

    Task<bool> IsEngineReadyAsync(
        CancellationToken cancellationToken = default);
}