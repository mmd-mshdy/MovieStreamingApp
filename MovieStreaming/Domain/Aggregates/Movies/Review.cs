using MovieStreaming.Domain.Common;

namespace MovieStreaming.Domain.Aggregates.Movies
{
    public class Review:Entity
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public int Rating { get; set; }
        public string Comment { get; set; }
        public Review(Guid id ,Guid userId, int rating, string comment) : base(id)
        {
            UserId = userId;
            Rating = rating;
            Comment = comment;
        }
    }
}
