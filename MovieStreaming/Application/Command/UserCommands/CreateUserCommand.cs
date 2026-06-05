using MediatR;
using Swashbuckle;
using MovieStreaming.Domain.Aggregates.Users;
using MovieStreaming.Domain.Common.Result;
using MovieStreaming.Application.DTOs;

namespace MovieStreaming.Application.Command.UserCommands
{
    public class CreateUserCommand(Guid Id ,CreateUserDto dto) : IRequest<Result<User>>
    {
        public Guid Id { get; set; } = Guid.NewGuid();  
        public CreateUserDto Dto { get; } = dto;
    }
}
