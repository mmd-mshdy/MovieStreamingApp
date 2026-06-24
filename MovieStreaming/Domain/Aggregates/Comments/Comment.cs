using MovieStreaming.Domain.Enums;
namespace MovieStreaming.Domain.Aggregates.Comments
{
    public sealed class Comment : AggregateRoot
    {
        public Guid MovieId { get; private set; }
        public Guid UserId { get; private set; }
        public string Content { get; private set; }
        public Guid? ParentCommentId { get; private set; } // Null if it's a top-level comment
        public CommentStatus Status { get; private set; }
        public DateTime CreatedAt { get; private set; }

        private Comment() { } // For EF Core

        // Constructor for a new comment
        public Comment(Guid movieId, Guid userId, string content, Guid? parentCommentId = null)
        {
            if (string.IsNullOrWhiteSpace(content))
                throw new ArgumentException("Comment content cannot be empty.");

            if (content.Length > 500)
                throw new ArgumentException("Comment exceeds maximum length of 500 characters.");

            Id = Guid.NewGuid();
            MovieId = movieId;
            UserId = userId;
            Content = content;
            ParentCommentId = parentCommentId;
            CreatedAt = DateTime.UtcNow;

            // Default to approved for this example, but could be PendingModeration based on business rules
            Status = CommentStatus.Approved;
        }

        public void Approve() => Status = CommentStatus.Approved;
        public void Reject() => Status = CommentStatus.Rejected;
    }
}
