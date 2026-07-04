namespace MovieStreaming.Application.DTOs
{
    public record CreateMovieDto(string title, string description, TimeSpan duration , DateOnly releaseDate , Guid genreId , string posterUrl , string videoUrl );

}
