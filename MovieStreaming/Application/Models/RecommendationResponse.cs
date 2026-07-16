using System.Text.Json.Serialization;

namespace MovieStreaming.Application.Models;

public sealed class RecommendationResponse
{
    [JsonPropertyName("recommendations")]
    public List<RecommendationResult> Recommendations
    {
        get;
        init;
    } = [];
}