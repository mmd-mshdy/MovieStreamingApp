using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieStreaming.Application.Command.WatchListCommands;
using MovieStreaming.Application.Queries.WatchListQueries;

namespace MovieStreaming.WebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/watchlist")]
    public class WatchListController : ControllerBase
    {
        private readonly ISender _sender;

        public WatchListController(ISender sender)
        {
            _sender = sender;
        }

        [HttpPost]
        public async Task<IActionResult> AddToWatchList([FromBody] AddToWatchListDto dto)
        {
            var command = new AddMovieToWatchListCommand(dto.MovieId);
            await _sender.Send(command);
            return Ok();
        }

        [HttpDelete("{movieId:guid}")]
        public async Task<IActionResult> RemoveFromWatchList(Guid movieId)
        {
            var command = new RemoveMovieFromWatchListCommand(movieId);
            await _sender.Send(command);
            return NoContent();
        }
        [HttpGet]
        public async Task<IActionResult> GetWatchList()
        {
            var query = new GetWatchListQuery();
            var result = await _sender.Send(query);

            return Ok(result);
        }
    }

    public record AddToWatchListDto(Guid MovieId);
}