using MediatR;
using MovieStreaming.Application.Interfaces;

public class GetContinueWatchingQueryHandler
    : IRequestHandler<
        GetContinueWatchingQuery,
        List<ContinueWatchingDto>>
{
    private readonly IWatchHistoryQueries _queries;
    private readonly ICurrentUserService _currentUser;

    public GetContinueWatchingQueryHandler(
        IWatchHistoryQueries queries,
        ICurrentUserService currentUser)
    {
        _queries = queries;
        _currentUser = currentUser;
    }

    public async Task<List<ContinueWatchingDto>> Handle(
        GetContinueWatchingQuery request,
        CancellationToken cancellationToken)
    {
        return await _queries.GetContinueWatching(
            _currentUser.UserId);
    }
}