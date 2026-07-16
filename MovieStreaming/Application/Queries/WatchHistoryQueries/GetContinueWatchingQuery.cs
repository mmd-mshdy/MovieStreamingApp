using MediatR;

namespace MovieStreaming.Application.Queries.WatchHistoryQueries;

public sealed record GetContinueWatchingQuery
    : IRequest<List<ContinueWatchingDto>>;