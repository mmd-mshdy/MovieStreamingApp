using MovieStreaming.Application.DTOs;

namespace MovieStreaming.Application.Interfaces
{
    public interface IWatchListQueries
    {
        Task<List<WatchListDto>> GetWatchListByUserId(Guid userId);
    }
}