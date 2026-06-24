using MovieStreaming.Domain.Common;

public class Review : Entity
{
    public Guid UserId { get; private set; }

    public Guid MovieId { get; set; }

    public int Rating { get; private set; }

    public string Comment { get; private set; }

    public DateTime CreatedAt { get; private set; }

    private Review(Guid id) : base (id){ }

    public Review(
        Guid id,
        Guid movieId,
        Guid userId,
        int rating,
        string comment)
        : base(id)
    {
        MovieId = movieId;
        UserId = userId;
        Rating = rating;
        Comment = comment;
        CreatedAt = DateTime.UtcNow;
    }
}