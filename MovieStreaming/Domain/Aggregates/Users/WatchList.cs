using MovieStreaming.Domain.Aggregates.Movies;
using MovieStreaming.Domain.Common;

namespace MovieStreaming.Domain.Aggregates.Users
{
    public class WatchList : Entity
    {
        public Guid UserId { get; private set; }
        public User? user { get; private set; }
        public Guid MovieId { get; private set; }
        public ICollection<Movie> Movies { get; private set; } = new List<Movie>();
        public DateTime? AddedAt { get; private set; }
        public WatchList(Guid id ,Guid userId, Guid movieId) : base(id)
        {
            UserId = userId;
            MovieId = movieId;
        }
        public void CreateWatchList (Guid id, Guid userId, Guid movieId)
        {
            var newWatchlist = new WatchList(id , userId, movieId);

        }

    }
}
