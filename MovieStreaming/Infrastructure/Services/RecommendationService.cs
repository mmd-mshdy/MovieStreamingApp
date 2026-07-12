using MovieStreaming.Application.Interfaces;
using MovieStreaming.Application.Models;
using MovieStreamingApp.Application.Interfaces;

public class RecommendationService : IRecommendationService
{
    private readonly HttpClient _httpClient;

    public RecommendationService(HttpClient httpClient)
    {
        _httpClient = httpClient;
        // Point this to your FastAPI URL
        _httpClient.BaseAddress = new Uri("http://localhost:8000/");
    }

    public async Task<List<int>> GetRecommendationsAsync(List<int> watchedIds, int topN)
    {
        var response = await _httpClient.PostAsJsonAsync("recommend", new { watched_movie_ids = watchedIds, top_n = topN });
        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadFromJsonAsync<RecommendationResponse>();
        return result.RecommendedMovieIds;
    }

    public async Task<bool> IsEngineReadyAsync()
    {
        var response = await _httpClient.GetAsync("status");
        if (response.IsSuccessStatusCode)
        {
            var status = await response.Content.ReadFromJsonAsync<EngineStatus>();
            return status.Trained;
        }
        return false;
    }
}