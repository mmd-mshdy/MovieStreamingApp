using MediatR;
using MovieStreaming.Application.DTOs;

namespace MovieStreaming.Application.Queries.RecommendationQueries;

public sealed record GetRecommendationsQuery(int Count = 10) : IRequest<IReadOnlyList<RecommendedMovieDto>>;