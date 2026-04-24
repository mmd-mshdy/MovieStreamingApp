using MovieStreaming.Domain.Common;
using MovieStreaming.Domain.Common.Result;
using System.Collections.ObjectModel;

namespace MovieStreaming.Domain.Aggregates.Movies
{
    public class Movie : AggregateRoot
    {
        private readonly List<Review> _reviews = new List<Review>();
        private readonly List<CastMembers> _castMembers = new List<CastMembers>();
        public string Title { get; set; }
        public string Description { get; set; }
        public TimeSpan Duration { get; set; }
        public DateOnly ReleaseDate { get; set; }
        public IReadOnlyCollection<CastMembers> CastMembers => _castMembers.AsReadOnly();
        public IReadOnlyCollection<Review> Reviews => _reviews.AsReadOnly();
        public Result AddReview(Guid id, Guid userId, int rating, string comment)
        {
            var newReview = new Review(id, userId, rating, comment);
            if (newReview == null) return Result.Failure(new("Review.empty", "Review can not be empty"));
            if (newReview.Rating < 1 || newReview.Rating > 5) return Result.Failure(new("Review.Rating.Invalid", "Put a valid rating input between 1 and 5"));
            _reviews.Add(newReview);
            return Result.Success(newReview);

        }
        public Result AddCastMembers (IEnumerable<CastMembers> castMembers)
        {
            if (castMembers == null) return Result.Failure(new("CastMembers.Null", "Members should not be null"));
            foreach (var castMember in castMembers)
            {
                _castMembers.Add(castMember);
            }
            return Result.Success();
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
