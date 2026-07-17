using AutoMapper;
using Dapper;
using Microsoft.EntityFrameworkCore;
using MovieStreaming.Application.Interfaces;
using MovieStreaming.Domain.Aggregates.Movies;
using MovieStreaming.Domain.Common.Errors;
using MovieStreaming.Domain.Common.Result;
using System.Data;

namespace MovieStreaming.Infrastructure.Repository
{
    public class MovieRepository : IMovieRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IDbConnection _dbConnection;
        public MovieRepository(ApplicationDbContext context, IDbConnection dbConnection, IMapper mapper)
        {
            _context = context;
            _dbConnection = dbConnection;
        }
        public async Task<IEnumerable<Movie>> GetAllAsync()
        {
            return await _context.Movies
                .Include(m => m.Reviews)
                .Include(m => m.Genres)
                .ToListAsync();

        }
        public async Task<Movie?> GetByIdWithReviewsAsync(Guid id)
        {
            return await _context.Movies
                .Include(m => m.Reviews)
                .Include(m => m.Genres)
                .FirstOrDefaultAsync(m => m.Id == id);
        }
        public async Task<Movie> FindById(Guid id)
        {
            // Ensure you have a space after "Movies"
            var query = @"SELECT * 
                          FROM Movies 
                          WHERE Id = @Id"; // Using verbatim string literal with newlines for readability

            // Pass the parameter 'id' to the Dapper query
            // Use 'new { Id = id }' to map the C# variable 'id' to the SQL parameter '@Id'
            var movie = await _dbConnection.QuerySingleOrDefaultAsync<Movie>(query, new { Id = id });
            if (movie == null) throw new ArgumentNullException(nameof(movie));
            return movie;

        }
        public async Task<IReadOnlyList<Movie>> SearchByTitleAsync(
    string searchTerm,
    CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                return [];
            }

            var normalizedSearchTerm = searchTerm.Trim();

            return await _context.Movies
                .AsNoTracking()
                .Include(movie => movie.Reviews)
                .Include(movie => movie.Genres)
                .Where(movie =>
                    EF.Functions.Like(
                        movie.Title,
                        $"%{normalizedSearchTerm}%"))
                .OrderBy(movie => movie.Title)
                .Take(20)
                .ToListAsync(cancellationToken);
        }


        public async Task CreateAsync(Movie movie)
        {
            if (movie == null) throw new ArgumentNullException(nameof(movie));

            await _context.Movies.AddAsync(movie);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateAsync(Movie movie)
        {
            var foundmovie = await _context.Movies.SingleOrDefaultAsync(m => m.Id == movie.Id);
            if (foundmovie == null) throw new ArgumentException(nameof(foundmovie));
            await _context.SaveChangesAsync();
        }
        public async Task DeleteAsync(Movie movie)
        {
            var foundmovie = await _context.Movies.SingleOrDefaultAsync(m => m.Id == movie.Id);
            if (foundmovie == null) throw new ArgumentException(nameof(foundmovie));
            _context.Movies.Remove(foundmovie);

        }
        public async Task<Movie> AddReview(Guid movieId, Review review)
        {
            // 1. Fetch the specific movie you want to add the review to.
            //    Use .Include(m => m.Reviews) if you want to return the movie with its updated reviews list.
            var movieToUpdate = await _context.Movies
                                        .Include(m => m.Reviews) // Include reviews to potentially return the updated list
                                        .FirstOrDefaultAsync(m => m.Id == movieId);

            if (movieToUpdate == null)
            {
                throw new ArgumentException($"Movie with ID {movieId} not found.");
            }

            if (review == null)
            {
                throw new ArgumentNullException(nameof(review));
            }

            // 2. Assign the MovieId to the review.
            //    This ensures the relationship is correctly set before adding.
            review.MovieId = movieId;
            // If your Review entity has a navigation property `Movie`, EF Core can often automatically
            // set it if you add the review to the movie's collection, but explicitly setting
            // `MovieId` is robust.

            // 3. Add the review to the context.
            //    Since you have a one-to-many relationship, EF knows how to handle this.
            //    You can either:
            //    a) Add the review directly:
            _context.Reviews.Add(review);
            //    b) Add the review to the movie's collection (if your Movie entity has one):
            //       movieToUpdate.AddReview(review); // Assuming AddReview adds to an internal list.
            //       Then EF will track it. Method 3a is more explicit.

            // 4. Save changes. EF Core will insert the review and set the foreign key.
            await _context.SaveChangesAsync();

            // 5. Return the movie (potentially with its updated reviews list if you included it).
            return movieToUpdate;
        }
        public async Task<IReadOnlyList<Movie>>
    GetByIdsWithDetailsAsync(
        IReadOnlyCollection<Guid> movieIds,
        CancellationToken cancellationToken = default)
        {
            if (movieIds.Count == 0)
            {
                return [];
            }

            return await _context.Movies
                .AsNoTracking()
                .Include(movie => movie.Reviews)
                .Include(movie => movie.Genres)
                .Where(movie => movieIds.Contains(movie.Id))
                .ToListAsync(cancellationToken);
        }
    }
}
