using MediatR;
using Microsoft.AspNetCore.Mvc;
using MovieStreaming.Application.Command.Movie;
using MovieStreaming.Application.Command.MovieCommands;
using MovieStreaming.Application.DTOs;
using MovieStreaming.Application.Queries.MovieQueries;
using MovieStreaming.Domain.Common.Result;

namespace MovieStreaming.WebApi.Controllers
{
    [ApiController]
    [Route("Api/[controller]")]
    public class MovieController : Controller
    {
        private readonly ISender _sender;
        public MovieController(ISender sender)
        {
            _sender=sender;
        }
        [HttpGet("{id:Guid}")]
        public async Task<IActionResult> GetMovieById (Guid id)
        {
            var command = new GetMovieByIdQuery(id);
            var result =await _sender.Send(command);
            return result!=null ? Ok(result) : BadRequest();
        }
        [HttpPost("Add Movie")]
        public async Task<IActionResult> CreateMovie([FromBody] CreateMovieDto dto)
        {
            var command = new CreateMovieCommand(dto);
            var result = await _sender.Send(command);
            return result == null? BadRequest() : Ok(result);

        }
        [HttpPost("Add Review")]
        public async Task<IActionResult> AddReview([FromBody]AddReviewDto dto , Guid movieId)
        {
            Guid id = Guid.NewGuid();
            var command = new AddReviewCommand(id,movieId,dto);
            var result = await _sender.Send(command);
            return result==null? BadRequest() : Ok(result);

        }
    }
}
