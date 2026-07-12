namespace MovieStreaming.Application.DTOs
{
    public record MovieDto(
        Guid id,
        string title,
        string description,
        TimeSpan duration,
        DateOnly releaseDate,
        string videoUrl,
        string posterUrl,
        List<ReviewDto> reviews,
        List<string> genres 
    );
}