using MediatR;
using MovieStreaming.Domain.Aggregates.Movies;
using MovieStreaming.Domain.Common.Result;
using MovieStreaming.Application.DTOs;

namespace MovieStreaming.Application.Command.Movie
{
    public record CreateMovieCommand(CreateMovieDto dto) : IRequest<Result<CreateMovieDto>>
    {
        public Guid Id { get;} = Guid.NewGuid();
        public CreateMovieDto Dto { get;} = dto;
    }

}
