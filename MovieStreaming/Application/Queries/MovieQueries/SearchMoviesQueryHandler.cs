using AutoMapper;
using MediatR;
using MovieStreaming.Application.DTOs;
using MovieStreaming.Application.Interfaces;
using MovieStreaming.Domain.Common.Errors;
using MovieStreaming.Domain.Common.Result;

namespace MovieStreaming.Application.Queries.MovieQueries;

public sealed class SearchMoviesQueryHandler
    : IRequestHandler<
        SearchMoviesQuery,
        Result<IReadOnlyList<MovieDto>>>
{
    private readonly IMovieRepository _movieRepository;
    private readonly IMapper _mapper;

    public SearchMoviesQueryHandler(
        IMovieRepository movieRepository,
        IMapper mapper)
    {
        _movieRepository = movieRepository;
        _mapper = mapper;
    }

    public async Task<Result<IReadOnlyList<MovieDto>>> Handle(
        SearchMoviesQuery request,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.SearchTerm))
        {
            return Result.Success<IReadOnlyList<MovieDto>>(
                []);
        }

        var movies = await _movieRepository.SearchByTitleAsync(
            request.SearchTerm,
            cancellationToken);

        var movieDtos = _mapper
            .Map<List<MovieDto>>(movies);

        return Result.Success<IReadOnlyList<MovieDto>>(
            movieDtos);
    }
}