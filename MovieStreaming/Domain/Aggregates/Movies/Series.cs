using MovieStreaming.Domain.Common;

namespace MovieStreaming.Domain.Aggregates.Movies
{
    public class Series : AggregateRoot
    {
        private readonly List<Episode> _episodes = new();
        public string Title { get; private set; }
        public string Description {  get; private set; }
        public IReadOnlyCollection<Episode> Episodes => _episodes;
        public Series(Guid id , string title , string description) : base(id)
        {
            Title = title;
            Description = description;
        }
        public void AddEpisode(Guid id ,string title , TimeSpan duration ,int  episodeNumber, int seasonNumber )
        {
            var newEpisode = new Episode(id, title, duration, seasonNumber, episodeNumber);
            _episodes.Add(newEpisode);
        }

    }
}
