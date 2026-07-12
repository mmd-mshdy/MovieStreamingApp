using System.Collections.Generic;
using System.Threading.Tasks;

namespace MovieStreamingApp.Application.Interfaces
{
    public interface IRecommendationService
    {
        Task<List<int>> GetRecommendationsAsync(List<int> watchedIds, int topN);
        Task<bool> IsEngineReadyAsync();
    }
}