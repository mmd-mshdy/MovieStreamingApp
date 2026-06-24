namespace MovieStreaming.Domain.Aggregates.Users
{
    public class WatchHistory : AggregateRoot
    {
        public Guid UserId { get; private set; }
        public Guid MovieId { get; private set; }

        public TimeSpan LastPosition { get; private set; }

        public bool Completed { get; private set; }

        public DateTime LastWatchedAt { get; private set; }
        private WatchHistory() { }

        public WatchHistory(Guid userId, Guid movieId)
        {
            UserId = userId;
            MovieId = movieId;
            LastPosition = TimeSpan.Zero;
            Completed = false;
            LastWatchedAt = DateTime.UtcNow;
        }

        public void UpdateProgress(
        TimeSpan position,
        bool completed)
        {
            LastPosition = position;
            Completed = completed;
            LastWatchedAt = DateTime.UtcNow;
        }
    }
}
