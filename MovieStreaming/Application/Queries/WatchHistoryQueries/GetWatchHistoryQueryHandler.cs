using MediatR;
using MovieStreaming.Application.Interfaces;

public class GetWatchHistoryQueryHandler
    : IRequestHandler<
        GetWatchHistoryQuery,
        List<WatchHistoryDto>>
{
    private readonly IWatchHistoryQueries _queries;
    private readonly ICurrentUserService _currentUser;

    public GetWatchHistoryQueryHandler(
        IWatchHistoryQueries queries,
        ICurrentUserService currentUser)
    {
        _queries = queries;
        _currentUser = currentUser;
    }

    public async Task<List<WatchHistoryDto>> Handle(
        GetWatchHistoryQuery request,
        CancellationToken cancellationToken)
    {
        return await _queries.GetWatchHistory(
            _currentUser.UserId);
    }
}