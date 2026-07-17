using MediatR;
using MovieStreaming.Application.DTOs;
using MovieStreaming.Domain.Common.Result;

namespace MovieStreaming.Application.Queries.MovieQueries;

public sealed record SearchMoviesQuery(string SearchTerm) : IRequest<Result<IReadOnlyList<MovieDto>>>;