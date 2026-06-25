using MediatR;
using MovieStreaming.Domain.Aggregates.Users;

namespace MovieStreaming.Application.Command.WatchListCommands
{
    public record RemoveMovieFromWatchListCommand(Guid MovieId) : IRequest;
}
