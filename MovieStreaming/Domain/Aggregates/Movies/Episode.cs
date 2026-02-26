using MovieStreaming.Domain.Common;

namespace MovieStreaming.Domain.Aggregates.Movies
{
    public class Episode : Entity
    {
        public string Title { get; private set; }
        public TimeSpan Duration { get; private set; }
        public int SeasonNumber { get; private set; }
        public int EpisodeNumber { get; private set; }
        public Episode(Guid id, string title, TimeSpan duration, int seasonNumber, int episodeNumber) : base(id)
        {
            Title = title;
            Duration = duration;
            SeasonNumber = seasonNumber;
            EpisodeNumber = episodeNumber;
        }
    }
}
