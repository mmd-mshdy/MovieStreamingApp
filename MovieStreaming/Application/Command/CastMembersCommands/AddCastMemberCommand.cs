using MediatR;
using MovieStreaming.Application.DTOs;
using MovieStreaming.Domain.Common.Result;

namespace MovieStreaming.Application.Command.CastMembersCommands
{
    public record AddCastMemberCommand (Guid Id ,CastMemberDto dto) : IRequest<Result<CastMemberDto>>
    {
        Guid Id { get; set; } = Guid.NewGuid();
        public CastMemberDto dto { get; } = dto;
    }
}
