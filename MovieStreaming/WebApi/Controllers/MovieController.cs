using MediatR;
using Microsoft.AspNetCore.Mvc;
using MovieStreaming.Application.Command.Movie;
using MovieStreaming.Application.Command.MovieCommands;
using MovieStreaming.Application.DTOs;
using MovieStreaming.Application.Queries.MovieQueries;

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

            // Assuming your queries return a Result pattern wrapper
            return result.IsSuccess ? Ok(result.Value) : NotFound(result.Error);
        }

        [HttpPost]
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
    }
}