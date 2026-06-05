using MediatR;
using MovieStreaming.Application.DTOs;
using MovieStreaming.Domain.Aggregates.Movies;
using MovieStreaming.Domain.Common.Result;

namespace MovieStreaming.Application.Queries.MovieQueries
{
    public record GetMovieByIdQuery(Guid id) : IRequest<Result<MovieDto>>;
}
