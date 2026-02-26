using MovieStreaming.Domain.Common;
using System.Collections.ObjectModel;

namespace MovieStreaming.Domain.Aggregates.Movies
{
    public class Movie : AggregateRoot
    {
        private readonly List<Review> _reviews = new();
        public string Title { get; set; }
        public string Description { get; set; }
        public TimeSpan Duration { get; set; }
        public DateOnly ReleaseDate { get; set; }
        public IReadOnlyCollection<Review> Reviews => _reviews;
        public Movie() { }
    }
}
