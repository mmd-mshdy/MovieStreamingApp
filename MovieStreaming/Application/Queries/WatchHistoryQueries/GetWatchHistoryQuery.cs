using MediatR;

public record GetWatchHistoryQuery
    : IRequest<List<WatchHistoryDto>>;