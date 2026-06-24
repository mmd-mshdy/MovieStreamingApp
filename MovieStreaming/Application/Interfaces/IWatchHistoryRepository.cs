using MovieStreaming.Domain.Aggregates.Users;

namespace MovieStreaming.Application.Interfaces
{
    public interface IWatchHistoryRepository
    {
        Task<WatchHistory?> GetAsync(Guid userId, Guid movieId);

        Task<List<WatchHistory>> GetByUserAsync(Guid userId);

        Task AddAsync(WatchHistory watchHistory);

        Task UpdateAsync(WatchHistory watchHistory);
    }
}
