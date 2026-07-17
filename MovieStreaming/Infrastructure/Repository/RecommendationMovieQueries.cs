using Microsoft.EntityFrameworkCore;
using MovieStreaming.Application.Interfaces;
using MovieStreaming.Application.Models;

namespace MovieStreaming.Infrastructure.Repository;

public sealed class RecommendationMovieQueries : IRecommendationMovieQueries
{
    private readonly ApplicationDbContext _context;

    public RecommendationMovieQueries(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyList<RecommendationMovie>> GetCatalogAsync(
        CancellationToken cancellationToken = default)
    {
        // Loading the small catalog as entities keeps the projection simple and
        // lets EF materialize the many-to-many genre/cast collections correctly.
        var movies = await _context.Movies
            .AsNoTracking()
            .Include(movie => movie.Genres)
            .OrderBy(movie => movie.Title)
            .ToListAsync(cancellationToken);

        return movies
            .Select(movie => new RecommendationMovie(
                MovieId: movie.Id,
                Title: movie.Title,
                Description: movie.Description,
                Genres: movie.Genres
                    .Select(genre => genre.Name)
                    .Where(name => !string.IsNullOrWhiteSpace(name))
                    .Distinct(StringComparer.OrdinalIgnoreCase)
                    .OrderBy(name => name)
                    .ToList(),
                ReleaseYear: movie.ReleaseDate.Year,
                AverageRating: movie.AverageRating))
            .ToList();
    }
}
