using MovieStreaming.Domain.Aggregates.Movies;

namespace MovieStreaming.Application.Interfaces
{
    public interface IMovieRepository
    {
        Task<IEnumerable<Movie>> GetAllAsync();
        Task<Movie> FindById(Guid id);
        Task<IReadOnlyList<Movie>> SearchByTitleAsync(string searchTerm,CancellationToken cancellationToken = default);
        Task CreateAsync(Movie movie);
        Task UpdateAsync(Movie movie);
        Task DeleteAsync(Movie movie);
        Task<Movie> AddReview(Guid movieId, Review review);
        Task<Movie?> GetByIdWithReviewsAsync(Guid id);
        Task<IReadOnlyList<Movie>> GetByIdsWithDetailsAsync(IReadOnlyCollection<Guid> movieIds,CancellationToken cancellationToken = default);
    }
}
