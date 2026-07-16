using System.Data;
using Dapper;

public class WatchHistoryQueries : IWatchHistoryQueries
{
    private readonly IDbConnection _dbConnection;

    public WatchHistoryQueries(
        IDbConnection dbConnection)
    {
        _dbConnection = dbConnection;
    }

    public async Task<List<WatchHistoryDto>> GetWatchHistory(Guid userId)
    {
        const string sql = """
        SELECT
            m.Id AS MovieId,
            m.Title,
            m.PosterUrl,
            DATEDIFF(
                SECOND,
                CAST('00:00:00' AS time),
                wh.LastPosition
            ) AS PositionSeconds,
            wh.LastWatchedAt
        FROM WatchHistories wh
        INNER JOIN Movies m
            ON m.Id = wh.MovieId
        WHERE wh.UserId = @UserId
        ORDER BY wh.LastWatchedAt DESC
        """;

        var result = await _dbConnection.QueryAsync<WatchHistoryDto>(
            sql,
            new { UserId = userId });

        return result.ToList();
    }

    public async Task<List<ContinueWatchingDto>> GetContinueWatching(
    Guid userId,
    CancellationToken cancellationToken)
    {
        const string sql = """
        SELECT
            m.Id AS MovieId,
            m.Title AS Title,
            m.PosterUrl AS PosterUrl,

            DATEDIFF(
                SECOND,
                CAST('00:00:00' AS time),
                wh.LastPosition
            ) AS PositionSeconds,

            DATEDIFF(
                SECOND,
                CAST('00:00:00' AS time),
                m.Duration
            ) AS DurationSeconds

        FROM WatchHistories wh
        INNER JOIN Movies m
            ON m.Id = wh.MovieId

        WHERE wh.UserId = @UserId
          AND wh.Completed = 0

        ORDER BY wh.LastWatchedAt DESC;
        """;

        var command = new CommandDefinition(
            sql,
            new { UserId = userId },
            cancellationToken: cancellationToken);

        var result =
            await _dbConnection.QueryAsync<ContinueWatchingDto>(command);

        return result.ToList();
    }
}