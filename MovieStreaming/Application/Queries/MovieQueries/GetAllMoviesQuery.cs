using MediatR;
using MovieStreaming.Application.DTOs;
using MovieStreaming.Domain.Common.Result;

namespace MovieStreaming.Application.Queries.MovieQueries
{
    public record GetAllMoviesQuery : IRequest<Result<IEnumerable<MovieDto>>>;
}
