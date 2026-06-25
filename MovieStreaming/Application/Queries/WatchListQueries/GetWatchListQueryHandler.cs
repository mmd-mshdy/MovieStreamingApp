using MediatR;
using MovieStreaming.Application.Interfaces;
using MovieStreaming.Application.DTOs;

namespace MovieStreaming.Application.Queries.WatchListQueries
{
    public class GetWatchListQueryHandler : IRequestHandler<GetWatchListQuery, List<WatchListDto>>
    {
        private readonly IWatchListQueries _queries;
        private readonly ICurrentUserService _currentUser;

        public GetWatchListQueryHandler(IWatchListQueries queries, ICurrentUserService currentUser)
        {
            _queries = queries;
            _currentUser = currentUser;
        }

        public async Task<List<WatchListDto>> Handle(GetWatchListQuery request, CancellationToken cancellationToken)
        {
            return await _queries.GetWatchListByUserId(_currentUser.UserId);
        }
    }
}