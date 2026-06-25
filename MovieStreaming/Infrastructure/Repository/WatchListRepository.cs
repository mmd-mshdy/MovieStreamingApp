using Microsoft.EntityFrameworkCore;
using MovieStreaming.Application.Interfaces;
using MovieStreaming.Domain.Aggregates.Users;

namespace MovieStreaming.Infrastructure.Repositories
{
    public class WatchListRepository : IWatchListRepository
    {
        private readonly ApplicationDbContext _context; // Replace with your actual DbContext name

        public WatchListRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> ExistsAsync(Guid userId, Guid movieId)
        {
            return await _context.WatchLists
                .AnyAsync(w => w.UserId == userId && w.MovieId == movieId);
        }

        public async Task AddAsync(WatchList watchList)
        {
            await _context.WatchLists.AddAsync(watchList);
        }

        public async Task RemoveAsync(Guid userId, Guid movieId)
        {
            var item = await _context.WatchLists
                .FirstOrDefaultAsync(w => w.UserId == userId && w.MovieId == movieId);

            if (item != null)
            {
                _context.WatchLists.Remove(item);
            }
        }
    }
}