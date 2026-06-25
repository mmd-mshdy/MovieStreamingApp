using Microsoft.EntityFrameworkCore;
using MovieStreaming.Application.DTOs;
using MovieStreaming.Application.Interfaces;

namespace MovieStreaming.Infrastructure.Queries
{
    public class WatchListQueries : IWatchListQueries
    {
        private readonly ApplicationDbContext _context; 

        public WatchListQueries(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<WatchListDto>> GetWatchListByUserId(Guid userId)
        {
            return await _context.WatchLists
                .Where(w => w.UserId == userId)
                .Join(
                    _context.Movies, // Joining with the Movies table
                    watchlist => watchlist.MovieId,
                    movie => movie.Id,
                    (watchlist, movie) => new WatchListDto
                    {
                        MovieId = movie.Id,
                        Title = movie.Title,
                        Description = movie.Description,
                        VideoUrl = movie.VideoUrl,
                        PosterUrl = movie.PosterUrl,
                        Rating = movie.AverageRating, // Map whatever fields exist on your Movie aggregate
                        AddedAt = watchlist.AddedAt
                    }
                )
                .OrderByDescending(dto => dto.AddedAt)
                .ToListAsync();
        }
    }
}