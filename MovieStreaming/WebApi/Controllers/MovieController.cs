using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieStreaming.Application.Command.Movie;
using MovieStreaming.Application.Command.MovieCommands;
using MovieStreaming.Application.Command.WatchHistoryCommands;
using MovieStreaming.Application.DTOs;
using MovieStreaming.Application.Queries.MovieQueries;
using MovieStreaming.Application.Queries.RecommendationQueries;

namespace MovieStreaming.WebApi.Controllers
{
    [ApiController]
    [Route("api/movies")] // Lowercase, plural RESTful routing
    public class MovieController : ControllerBase // Changed to ControllerBase (preferred for APIs)
    {
        private readonly ISender _sender;
        public MovieController(ISender sender)
        {
            _sender = sender;
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetMovieById(Guid id)
        {
            var query = new GetMovieByIdQuery(id);
            var result = await _sender.Send(query);
            return result.IsSuccess ? Ok(result.Value) : NotFound(result.Error);
        }

        [HttpPost("Add Movie")]
        [Authorize]
        public async Task<IActionResult> CreateMovie([FromBody] CreateMovieDto dto)
        {
            var command = new CreateMovieCommand(dto);
            var result = await _sender.Send(command);
            return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
        }

        [HttpPost("{movieId:guid}/reviews")] // Clean REST path: /api/movies/{movieId}/reviews
        public async Task<IActionResult> AddReview([FromRoute] Guid movieId, [FromBody] AddReviewDto dto)
        {
            Guid id = Guid.NewGuid();
            var command = new AddReviewCommand(id, movieId, dto);
            var result = await _sender.Send(command);
            return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
        }
        [HttpGet]
        public async Task<IActionResult> GetAllMovies()
        {
            var query = new GetAllMoviesQuery();
            var result = await _sender.Send(query);
            return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
        }

        [HttpPost("progress")] // Route: POST api/watch-history/progress
        public async Task<IActionResult> UpdateProgress([FromBody] UpdateWatchProgressCommand command)
        {
            await _sender.Send(command);
            return NoContent(); // 204 Success response with no body content
        }

        [HttpGet("recommendation-catalog")]
        public async Task<IActionResult> GetRecommendationCatalog(
            CancellationToken cancellationToken)
        {
            var catalog = await _sender.Send(
                new GetRecommendationCatalogQuery(),
                cancellationToken);

            return Ok(catalog);
        }
    }
}