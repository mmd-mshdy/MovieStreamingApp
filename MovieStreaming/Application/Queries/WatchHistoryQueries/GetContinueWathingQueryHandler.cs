using MediatR;
using MovieStreaming.Application.Interfaces;

namespace MovieStreaming.Application.Queries.WatchHistoryQueries;

public sealed class GetContinueWatchingQueryHandler
    : IRequestHandler<GetContinueWatchingQuery, List<ContinueWatchingDto>>
{
    private readonly IWatchHistoryQueries _queries;
    private readonly ICurrentUserService _currentUserService;

    public GetContinueWatchingQueryHandler(
        IWatchHistoryQueries queries,
        ICurrentUserService currentUserService)
    {
        _queries = queries;
        _currentUserService = currentUserService;
    }

    public async Task<List<ContinueWatchingDto>> Handle(
        GetContinueWatchingQuery request,
        CancellationToken cancellationToken)
    {
        return await _queries.GetContinueWatching(
            _currentUserService.UserId,
            cancellationToken);
    }
}