using System.Text.Json.Serialization;

public class OmdbSeedModel
{
    [JsonPropertyName("Title")]
    public string Title { get; set; } = string.Empty;

    [JsonPropertyName("Plot")]
    public string Plot { get; set; } = string.Empty;

    [JsonPropertyName("Runtime")]
    public string Runtime { get; set; } = string.Empty;

    [JsonPropertyName("Released")]
    public string Released { get; set; } = string.Empty;

    [JsonPropertyName("Poster")]
    public string Poster { get; set; } = string.Empty;
}