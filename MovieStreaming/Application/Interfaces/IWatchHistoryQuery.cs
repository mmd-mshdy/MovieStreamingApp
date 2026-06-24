public interface IWatchHistoryQueries
{
    Task<List<WatchHistoryDto>> GetWatchHistory(
        Guid userId);

    Task<List<ContinueWatchingDto>> GetContinueWatching(
        Guid userId);
}