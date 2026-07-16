using System.Net;
using System.Net.Http.Json;
using MovieStreaming.Application.Interfaces;
using MovieStreaming.Application.Models;

namespace MovieStreaming.Infrastructure.Services;

public sealed class RecommendationService
    : IRecommendationService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<RecommendationService> _logger;

    public RecommendationService(
        HttpClient httpClient,
        ILogger<RecommendationService> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task<IReadOnlyList<RecommendationResult>>
        GetRecommendationsAsync(
            RecommendationRequest request,
            CancellationToken cancellationToken = default)
    {
        try
        {
            using var response = await _httpClient.PostAsJsonAsync(
                "recommend",
                request,
                cancellationToken);

            if (response.StatusCode ==
                HttpStatusCode.ServiceUnavailable)
            {
                _logger.LogWarning(
                    "The recommendation engine is not ready.");

                return [];
            }

            if (!response.IsSuccessStatusCode)
            {
                var responseBody =
                    await response.Content.ReadAsStringAsync(
                        cancellationToken);

                _logger.LogError(
                    "Recommendation service returned {StatusCode}. " +
                    "Response: {ResponseBody}",
                    response.StatusCode,
                    responseBody);

                return [];
            }

            var result =
                await response.Content
                    .ReadFromJsonAsync<RecommendationResponse>(
                        cancellationToken: cancellationToken);

            return result?.Recommendations ?? [];
        }
        catch (OperationCanceledException)
            when (!cancellationToken.IsCancellationRequested)
        {
            _logger.LogWarning(
                "Recommendation service request timed out.");

            return [];
        }
        catch (HttpRequestException exception)
        {
            _logger.LogError(
                exception,
                "Could not connect to the recommendation service.");

            return [];
        }
    }

    public async Task<bool> IsEngineReadyAsync(
        CancellationToken cancellationToken = default)
    {
        try
        {
            using var response = await _httpClient.GetAsync(
                "status",
                cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                return false;
            }

            var status =
                await response.Content
                    .ReadFromJsonAsync<EngineStatus>(
                        cancellationToken: cancellationToken);

            return status?.Trained == true;
        }
        catch (HttpRequestException exception)
        {
            _logger.LogWarning(
                exception,
                "Recommendation engine status check failed.");

            return false;
        }
    }
}