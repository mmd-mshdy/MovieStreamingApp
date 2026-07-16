using MediatR;
using MovieStreaming.Application.Models;

namespace MovieStreaming.Application.Queries.RecommendationQueries;

public sealed record GetRecommendationCatalogQuery
    : IRequest<IReadOnlyList<RecommendationMovie>>;
