using MediatR;

namespace MovieStreaming.Application.Command.WatchListCommands
{
    public record AddMovieToWatchListCommand(Guid MovieId) : IRequest;
}
