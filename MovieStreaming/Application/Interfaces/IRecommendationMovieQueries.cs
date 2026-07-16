using MovieStreaming.Application.Models;

namespace MovieStreaming.Application.Interfaces;

public interface IRecommendationMovieQueries
{
    Task<IReadOnlyList<RecommendationMovie>> GetCatalogAsync(
        CancellationToken cancellationToken = default);
}
