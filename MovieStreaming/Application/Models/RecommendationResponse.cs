namespace MovieStreaming.Application.Models
{
    public class RecommendationResponse
    {
        // Matches the JSON key returned by your Python code
        public List<int> RecommendedMovieIds { get; set; }
    }
}
