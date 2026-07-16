using System.Text.Json.Serialization;

namespace MovieStreaming.Application.Models;

public sealed class EngineStatus
{
    [JsonPropertyName("trained")]
    public bool Trained { get; init; }

    [JsonPropertyName("movieCount")]
    public int MovieCount { get; init; }
}