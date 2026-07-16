using Microsoft.EntityFrameworkCore;
using MovieStreaming.Application.Interfaces;
using MovieStreaming.Application.Models;

namespace MovieStreaming.Infrastructure.Queries;

public sealed class RecommendationInteractionQueries
    : IRecommendationInteractionQueries
{
    private readonly ApplicationDbContext _context;

    public RecommendationInteractionQueries(
        ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyList<RecommendationInteraction>>
        GetUserInteractionsAsync(
            Guid userId,
            CancellationToken cancellationToken = default)
    {
        /*
         * We load all three interaction sources independently:
         *
         * 1. Watch history
         * 2. Reviews/ratings
         * 3. Watchlist entries
         *
         * Then we merge them by MovieId.
         */

        var watchHistory = await _context.WatchHistories
            .AsNoTracking()
            .Where(history => history.UserId == userId)
            .Select(history => new
            {
                history.MovieId,
                history.LastPosition,
                history.Completed
            })
            .ToListAsync(cancellationToken);

        var ratings = await _context.Reviews
            .AsNoTracking()
            .Where(review => review.UserId == userId)
            .Select(review => new
            {
                review.MovieId,
                review.Rating
            })
            .ToListAsync(cancellationToken);

        var watchlistMovieIds = await _context.WatchLists
            .AsNoTracking()
            .Where(item => item.UserId == userId)
            .Select(item => item.MovieId)
            .ToListAsync(cancellationToken);

        var allMovieIds = watchHistory
            .Select(history => history.MovieId)
            .Concat(ratings.Select(rating => rating.MovieId))
            .Concat(watchlistMovieIds)
            .Distinct()
            .ToList();

        if (allMovieIds.Count == 0)
        {
            return [];
        }

        /*
         * Duration is needed to calculate:
         *
         * watched seconds / total seconds × 100
         */

        var movieDurations = await _context.Movies
            .AsNoTracking()
            .Where(movie => allMovieIds.Contains(movie.Id))
            .Select(movie => new
            {
                movie.Id,
                movie.Duration
            })
            .ToDictionaryAsync(
                movie => movie.Id,
                movie => movie.Duration,
                cancellationToken);

        var watchHistoryByMovie = watchHistory
            .GroupBy(history => history.MovieId)
            .ToDictionary(
                group => group.Key,
                group => group.First());

        var ratingsByMovie = ratings
            .GroupBy(rating => rating.MovieId)
            .ToDictionary(
                group => group.Key,
                group => (int?)group
                    .OrderByDescending(rating => rating.Rating)
                    .First()
                    .Rating);

        var watchlistSet = watchlistMovieIds.ToHashSet();

        var interactions = new List<RecommendationInteraction>();

        foreach (var movieId in allMovieIds)
        {
            watchHistoryByMovie.TryGetValue(
                movieId,
                out var history);

            ratingsByMovie.TryGetValue(
                movieId,
                out var rating);

            movieDurations.TryGetValue(
                movieId,
                out var duration);

            var watchPercentage = CalculateWatchPercentage(
                history?.LastPosition ?? TimeSpan.Zero,
                duration);

            interactions.Add(
                new RecommendationInteraction(
                    MovieId: movieId,
                    WatchPercentage: watchPercentage,
                    Completed: history?.Completed ?? false,
                    Rating: rating,
                    InWatchlist: watchlistSet.Contains(movieId)));
        }

        return interactions;
    }

    private static double CalculateWatchPercentage(
        TimeSpan watchedPosition,
        TimeSpan movieDuration)
    {
        if (movieDuration.TotalSeconds <= 0)
        {
            return 0;
        }

        var percentage =
            watchedPosition.TotalSeconds
            / movieDuration.TotalSeconds
            * 100.0;

        return Math.Round(
            Math.Clamp(percentage, 0, 100),
            2);
    }
}