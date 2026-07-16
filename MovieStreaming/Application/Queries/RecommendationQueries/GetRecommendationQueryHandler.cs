using MediatR;
using MovieStreaming.Application.DTOs;
using MovieStreaming.Application.Interfaces;
using MovieStreaming.Application.Models;

namespace MovieStreaming.Application.Queries
    .RecommendationQueries;

public sealed class GetRecommendationsQueryHandler
    : IRequestHandler<
        GetRecommendationsQuery,
        IReadOnlyList<RecommendedMovieDto>>
{
    private readonly ICurrentUserService _currentUserService;
    private readonly IRecommendationInteractionQueries
        _interactionQueries;
    private readonly IRecommendationService
        _recommendationService;
    private readonly IMovieRepository _movieRepository;

    public GetRecommendationsQueryHandler(
        ICurrentUserService currentUserService,
        IRecommendationInteractionQueries interactionQueries,
        IRecommendationService recommendationService,
        IMovieRepository movieRepository)
    {
        _currentUserService = currentUserService;
        _interactionQueries = interactionQueries;
        _recommendationService = recommendationService;
        _movieRepository = movieRepository;
    }

    public async Task<IReadOnlyList<RecommendedMovieDto>> Handle(
        GetRecommendationsQuery request,
        CancellationToken cancellationToken)
    {
        var userId = _currentUserService.UserId;

        if (userId == Guid.Empty)
        {
            return [];
        }

        var count = Math.Clamp(request.Count, 1, 30);

        var interactions =
            await _interactionQueries.GetUserInteractionsAsync(
                userId,
                cancellationToken);

        var recommendationRequest =
            new RecommendationRequest
            {
                UserId = userId,
                Interactions = interactions,
                TopN = count
            };

        var recommendationResults =
            await _recommendationService
                .GetRecommendationsAsync(
                    recommendationRequest,
                    cancellationToken);

        if (recommendationResults.Count == 0)
        {
            return [];
        }

        var recommendedIds = recommendationResults
            .Select(result => result.MovieId)
            .Distinct()
            .ToList();

        var movies = await _movieRepository
            .GetByIdsWithDetailsAsync(
                recommendedIds,
                cancellationToken);

        var moviesById = movies.ToDictionary(
            movie => movie.Id);

        /*
         * We iterate over recommendationResults rather than movies.
         *
         * SQL does not guarantee that the returned movies remain in
         * the recommendation ranking order.
         */

        var response =
            new List<RecommendedMovieDto>();

        foreach (var recommendation in recommendationResults)
        {
            if (!moviesById.TryGetValue(
                    recommendation.MovieId,
                    out var movie))
            {
                continue;
            }

            var reviewDtos = movie.Reviews
                .Select(review => new ReviewDto(
                    review.Id,
                    review.UserId,
                    "Anonymous",
                    review.Rating,
                    review.Comment))
                .ToList();

            var genreNames = movie.Genres
                .Select(genre => genre.Name)
                .Distinct()
                .ToList();

            var movieDto = new MovieDto(
                movie.Id,
                movie.Title,
                movie.Description,
                movie.Duration,
                movie.ReleaseDate,
                movie.VideoUrl,
                movie.PosterUrl,
                reviewDtos,
                genreNames);

            response.Add(
                new RecommendedMovieDto(
                    Movie: movieDto,
                    Score: recommendation.Score,
                    Reason: recommendation.Reason));
        }

        return response;
    }
}