public sealed class ContinueWatchingDto
{
    public Guid MovieId { get; init; }

    public string Title { get; init; } = string.Empty;

    public string PosterUrl { get; init; } = string.Empty;

    public int PositionSeconds { get; init; }
}