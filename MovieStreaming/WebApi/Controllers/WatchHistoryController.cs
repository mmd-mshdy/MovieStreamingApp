using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Authorize]
[ApiController]
[Route("api/watch-history")]
public class WatchHistoryController : ControllerBase
{
    private readonly ISender _sender;

    public WatchHistoryController(
        ISender sender)
    {
        _sender = sender;
    }

    [HttpGet]
    public async Task<IActionResult> GetHistory()
    {
        var result = await _sender.Send(
            new GetWatchHistoryQuery());

        return Ok(result);
    }

    [HttpGet("continue-watching")]
    public async Task<IActionResult> GetContinueWatching()
    {
        var result = await _sender.Send(
            new GetContinueWatchingQuery());

        return Ok(result);
    }
}