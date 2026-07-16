using MovieStreaming.Application.Models;

namespace MovieStreaming.Application.Interfaces;

public interface IRecommendationInteractionQueries
{
    Task<IReadOnlyList<RecommendationInteraction>>
        GetUserInteractionsAsync(
            Guid userId,
            CancellationToken cancellationToken = default);
}