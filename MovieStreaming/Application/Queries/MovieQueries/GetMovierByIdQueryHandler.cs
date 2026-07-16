using MediatR;
using MovieStreaming.Application.DTOs;
using MovieStreaming.Application.Interfaces;
using MovieStreaming.Domain.Common.Result;
using MovieStreaming.Infrastructure.Repository;

namespace MovieStreaming.Application.Queries.MovieQueries;

public class GetMovieByIdQueryHandler
    : IRequestHandler<GetMovieByIdQuery, Result<MovieDto>>
{
    private readonly IMovieRepository _movieRepository;
    private readonly IUserRepository _userRepository;

    public GetMovieByIdQueryHandler(
        IMovieRepository movieRepository,
        IUserRepository userRepository)
    {
        _movieRepository = movieRepository;
        _userRepository = userRepository;
    }

    public async Task<Result<MovieDto>> Handle(
        GetMovieByIdQuery request,
        CancellationToken cancellationToken)
    {
        var movie = await _movieRepository.GetByIdWithReviewsAsync(request.id);

        if (movie is null)
        {
            return Result.Failure<MovieDto>(
                new ("Movie.NotFound", "Movie not found."));
        }

        var reviewDtos = new List<ReviewDto>();

        foreach (var review in movie.Reviews)
        {
            var user = await _userRepository.GetUserById(review.UserId);

            reviewDtos.Add(
                new ReviewDto(
                    review.Id,
                    review.UserId,
                    user?.Name ?? "Anonymous",
                    review.Rating,
                    review.Comment));
        }

        var genreNames = movie.Genres
            .Select(genre => genre.Name)
            .ToList();

        var movieDto = new MovieDto(
            movie.Id,
            movie.Title,
            movie.Description,
            movie.Duration,
            movie.ReleaseDate,
            movie.VideoUrl,
            movie.PosterUrl,
            reviewDtos,
            genreNames);

        return Result.Success(movieDto);
    }
}