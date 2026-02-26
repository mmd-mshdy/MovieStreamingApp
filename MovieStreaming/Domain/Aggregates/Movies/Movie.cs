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
        public void AddReview(Guid id,Guid userId , int rating , string comment)
        {
            var newReview = new Review(id,userId, rating, comment);
            _reviews.Add(newReview);

        }

        public Movie(Guid id ,string title, string description, TimeSpan duration, DateOnly releaseDate) :base(id)
        {
            Title = title;
            Description = description;
            Duration = duration;
            ReleaseDate = releaseDate;
        }
    }
}
