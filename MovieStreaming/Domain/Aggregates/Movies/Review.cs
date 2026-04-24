using MovieStreaming.Domain.Common;
using System.Text.Json.Serialization;

namespace MovieStreaming.Domain.Aggregates.Movies
{
    public class Review:Entity
    {
        public Guid UserId { get; set; }
        public Guid MovieId { get; set; }

        [JsonIgnore]
        public Movie Movie { get; set; }
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
