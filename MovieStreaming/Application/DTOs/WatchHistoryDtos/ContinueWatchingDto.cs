
public record ContinueWatchingDto(
        Guid MovieId,
        string Title,
        string PosterUrl,
        TimeSpan LastPosition
    )
{
    public double ProgressPercentage => 0;
}
