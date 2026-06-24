using MediatR;

namespace MovieStreaming.Application.Command.WatchHistoryCommands
{
    public record UpdateWatchProgressCommand(
    Guid MovieId,
    int PositionSeconds
) : IRequest;
}
