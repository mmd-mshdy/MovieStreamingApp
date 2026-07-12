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

    public async Task<List<WatchHistoryDto>>
        GetWatchHistory(Guid userId)
    {
        var sql = """
            SELECT
                m.Id AS MovieId,
                m.Title,
                m.PosterUrl,
                wh.LastPosition,
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

    public async Task<List<ContinueWatchingDto>> GetContinueWatching(Guid userId)
    {
        var sql = """
        SELECT
            m.Id AS MovieId,
            m.Title,
            m.PosterUrl,
            wh.LastPosition
        FROM WatchHistories wh
        INNER JOIN Movies m
            ON m.Id = wh.MovieId
        WHERE wh.UserId = @UserId
          AND wh.Completed = 0
        ORDER BY wh.LastWatchedAt DESC
        """;

        var result =
            await _dbConnection.QueryAsync<ContinueWatchingDto>(
                sql,
                new { UserId = userId });

        return result.ToList();
    }
}