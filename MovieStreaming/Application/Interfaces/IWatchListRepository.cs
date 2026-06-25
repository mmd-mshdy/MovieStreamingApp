using MovieStreaming.Domain.Aggregates.Users;

namespace MovieStreaming.Application.Interfaces
{
    public interface IWatchListRepository
    {
        Task<bool> ExistsAsync(Guid userId, Guid movieId);
        Task AddAsync(WatchList watchList);
        Task RemoveAsync(Guid userId, Guid movieId);
    }
}
