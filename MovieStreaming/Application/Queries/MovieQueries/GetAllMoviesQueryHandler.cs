using AutoMapper;
using MediatR;
using MovieStreaming.Application.DTOs;
using MovieStreaming.Application.Interfaces;
using MovieStreaming.Domain.Common.Errors;
using MovieStreaming.Domain.Common.Result;

namespace MovieStreaming.Application.Queries.MovieQueries;

public sealed class GetAllMoviesQueryHandler
    : IRequestHandler<
        GetAllMoviesQuery,
        Result<IEnumerable<MovieDto>>>
{
    private readonly IMovieRepository _repository;
    private readonly IMapper _mapper;

    public GetAllMoviesQueryHandler(
        IMovieRepository repository,
        IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<Result<IEnumerable<MovieDto>>> Handle(
        GetAllMoviesQuery request,
        CancellationToken cancellationToken)
    {
        var movies = await _repository.GetAllAsync();

        var movieList = movies.ToList();

        if (movieList.Count == 0)
        {
            return Result.Failure<IEnumerable<MovieDto>>(
                new Error(
                    "Movie.NotFound",
                    "Movies were not found."));
        }

        var movieDtos =
            _mapper.Map<IEnumerable<MovieDto>>(movieList);

        return Result.Success(movieDtos);
    }
}