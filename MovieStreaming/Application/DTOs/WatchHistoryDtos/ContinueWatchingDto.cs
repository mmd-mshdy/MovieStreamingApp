public sealed class ContinueWatchingDto
{
    public Guid MovieId { get; init; }

    public string Title { get; init; } = string.Empty;

    public string PosterUrl { get; init; } = string.Empty;

    public int PositionSeconds { get; init; }

    public int DurationSeconds { get; init; }

    public double ProgressPercentage =>
        DurationSeconds > 0
            ? Math.Round(
                Math.Clamp(
                    PositionSeconds * 100.0 / DurationSeconds,
                    0,
                    100),
                1)
            : 0;
}