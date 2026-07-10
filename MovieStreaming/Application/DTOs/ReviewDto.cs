namespace MovieStreaming.Application.DTOs
{
    public record ReviewDto(Guid id, Guid userId, string userName, int rating, string comment);
}