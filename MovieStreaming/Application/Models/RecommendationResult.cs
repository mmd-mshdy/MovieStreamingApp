using System.Text.Json.Serialization;

namespace MovieStreaming.Application.Models;

public sealed class RecommendationResult
{
    [JsonPropertyName("movieId")]
    public Guid MovieId { get; init; }

    [JsonPropertyName("score")]
    public double Score { get; init; }

    [JsonPropertyName("reason")]
    public string Reason { get; init; } = string.Empty;
}