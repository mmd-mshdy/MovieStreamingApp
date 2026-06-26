using MovieStreaming.Domain.Aggregates.Movies;

namespace MovieStreaming.Application.Interfaces
{
    public interface IMovieRepository
    {
        Task<IEnumerable<Movie>> GetAllAsync();
        Task<Movie> FindById(Guid id);
        Task<IEnumerable<Movie>> FindByTitle(string title);
        Task<IEnumerable<Movie>> FindByCastMember(string name);
        Task CreateAsync(Movie movie);
        Task UpdateAsync(Movie movie);
        Task DeleteAsync(Movie movie);
        Task<Movie> AddReview(Guid movieId, Review review);
    }
}
