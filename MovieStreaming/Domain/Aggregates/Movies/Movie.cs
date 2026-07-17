using MovieStreaming.Domain.Common;
using MovieStreaming.Domain.Common.Result;
using System.Collections.ObjectModel;

namespace MovieStreaming.Domain.Aggregates.Movies
{
    public class Movie : AggregateRoot
    {
        private readonly List<Genre> _genres = new();
        private readonly List<Review> _reviews = new List<Review>();
        public string Title { get; set; }
        public string Description { get; set; }
        public TimeSpan Duration { get; set; }
        public DateOnly ReleaseDate { get; set; }
        public Guid GenreId { get; private set; }

        public string PosterUrl { get; private set; }

        public string VideoUrl { get; private set; }

        public double AverageRating { get; private set; }
        public IReadOnlyCollection<Genre> Genres => _genres.AsReadOnly();
        public IReadOnlyCollection<Review> Reviews => _reviews.AsReadOnly();
        public Result AddReview(
    Guid reviewId,
    Guid userId,
    int rating,
    string comment)
        {
            if (rating < 1 || rating > 5)
            {
                return Result.Failure(
                    new(
                        "Review.Rating.Invalid",
                        "Rating must be between 1 and 5"));
            }

            if (_reviews.Any(r => r.UserId == userId))
            {
                return Result.Failure(
                    new(
                        "Review.AlreadyExists",
                        "User has already reviewed this movie"));
            }

            var review = new Review(
                reviewId,
                Id,
                userId,
                rating,
                comment);

            _reviews.Add(review);

            RecalculateAverageRating();

            return Result.Success(review);
        }
        private void RecalculateAverageRating()
        {
            if (!_reviews.Any())
            {
                AverageRating = 0;
                return;
            }

            AverageRating = _reviews.Average(r => r.Rating);
        }
        public void UpdateMediaUrls(string posterUrl, string videoUrl)
        {
            PosterUrl = posterUrl;
            VideoUrl = videoUrl;
        }
        private Movie() {}
        public Movie(Guid id ,string title, string description, TimeSpan duration, DateOnly releaseDate) :base(id)
        {
            Title = title;
            Description = description;
            Duration = duration;
            ReleaseDate = releaseDate;
        }
    }
}
