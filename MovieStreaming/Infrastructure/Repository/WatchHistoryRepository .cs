using Microsoft.EntityFrameworkCore;
using MovieStreaming.Application.Interfaces;
using MovieStreaming.Domain.Aggregates.Users;

namespace MovieStreaming.Infrastructure.Repository
{
    public class WatchHistoryRepository : IWatchHistoryRepository
    {
        private readonly ApplicationDbContext _context;

        public WatchHistoryRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<WatchHistory?> GetAsync(Guid userId, Guid movieId)
        {
            return await _context.WatchHistories
                .FirstOrDefaultAsync(x =>
                    x.UserId == userId &&
                    x.MovieId == movieId);
        }

        public async Task<List<WatchHistory>> GetByUserAsync(Guid userId)
        {
            return await _context.WatchHistories
                .Where(x => x.UserId == userId)
                .OrderByDescending(x => x.LastWatchedAt)
                .ToListAsync();
        }

        public async Task AddAsync(WatchHistory watchHistory)
        {
            await _context.WatchHistories.AddAsync(watchHistory);
        }

        public Task UpdateAsync(WatchHistory watchHistory)
        {
            _context.WatchHistories.Update(watchHistory);
            return Task.CompletedTask;
        }
    }
}
