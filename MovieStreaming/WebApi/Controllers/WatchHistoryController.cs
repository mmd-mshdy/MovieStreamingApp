using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieStreaming.Application.Command.WatchHistoryCommands;
using MovieStreaming.Application.Queries.WatchHistoryQueries;

namespace MovieStreaming.WebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/watch-history")]
    public class WatchHistoryController : ControllerBase
    {
        private readonly ISender _sender;

        public WatchHistoryController(ISender sender)
        {
            _sender = sender;
        }

        [HttpGet]
        public async Task<IActionResult> GetHistory()
        {
            var result = await _sender.Send(new GetWatchHistoryQuery());
            return Ok(result);
        }
        [Authorize]
        [Authorize]
        [HttpGet("continue-watching")]
        public async Task<ActionResult<List<ContinueWatchingDto>>> GetContinueWatching(
    CancellationToken cancellationToken)
        {
            var result = await _sender.Send(
                new GetContinueWatchingQuery(),
                cancellationToken);

            return Ok(result);
        }

        [HttpPost("progress")] // Added missing progress tracker endpoint!
        public async Task<IActionResult> UpdateProgress([FromBody] UpdateProgressDto dto)
        {
            var command = new UpdateWatchProgressCommand(dto.MovieId, dto.PositionSeconds);
            await _sender.Send(command);
            return NoContent();
        }
    }

    public record UpdateProgressDto(Guid MovieId, int PositionSeconds);
}