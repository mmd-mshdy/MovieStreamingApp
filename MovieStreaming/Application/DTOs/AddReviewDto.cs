namespace MovieStreaming.Application.DTOs
{
    public record AddReviewDto(Guid userId, int rating, string comment);
}
