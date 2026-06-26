using MediatR;
using MovieStreaming.Application.DTOs;
using MovieStreaming.Domain.Common.Result;

namespace MovieStreaming.Application.Command.UserCommands
{
    public record LoginUserCommand(LoginRequestDto LoginDto) : IRequest<Result<AuthResponseDto>>;
}
