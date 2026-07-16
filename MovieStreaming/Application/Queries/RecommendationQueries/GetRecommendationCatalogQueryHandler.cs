using MediatR;
using MovieStreaming.Application.Interfaces;
using MovieStreaming.Application.Models;

namespace MovieStreaming.Application.Queries.RecommendationQueries;

public sealed class GetRecommendationCatalogQueryHandler
    : IRequestHandler<GetRecommendationCatalogQuery, IReadOnlyList<RecommendationMovie>>
{
    private readonly IRecommendationMovieQueries _queries;

    public GetRecommendationCatalogQueryHandler(
        IRecommendationMovieQueries queries)
    {
        _queries = queries;
    }

    public Task<IReadOnlyList<RecommendationMovie>> Handle(
        GetRecommendationCatalogQuery request,
        CancellationToken cancellationToken)
    {
        return _queries.GetCatalogAsync(cancellationToken);
    }
}
