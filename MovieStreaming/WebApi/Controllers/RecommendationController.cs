using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieStreaming.Application.DTOs;
using MovieStreaming.Application.Interfaces;
using MovieStreaming.Application.Queries
    .RecommendationQueries;

namespace MovieStreaming.WebApi.Controllers;

[ApiController]
[Route("api/recommendations")]
[Authorize]
public sealed class RecommendationController
    : ControllerBase
{
    private readonly ISender _sender;
    private readonly IRecommendationService _recommendationService;

    public RecommendationController(
        ISender sender,
        IRecommendationService recommendationService)
    {
        _sender = sender;
        _recommendationService = recommendationService;
    }

    [HttpGet]
    [ProducesResponseType(
        typeof(IReadOnlyList<RecommendedMovieDto>),
        StatusCodes.Status200OK)]
    public async Task<IActionResult> GetRecommendations(
        [FromQuery] int count = 10,
        CancellationToken cancellationToken = default)
    {
        var recommendations = await _sender.Send(
            new GetRecommendationsQuery(count),
            cancellationToken);

        return Ok(recommendations);
    }

    [HttpGet("status")]
    [AllowAnonymous]
    public async Task<IActionResult> GetStatus(
        CancellationToken cancellationToken)
    {
        var ready =
            await _recommendationService
                .IsEngineReadyAsync(cancellationToken);

        return Ok(new
        {
            ready
        });
    }
}