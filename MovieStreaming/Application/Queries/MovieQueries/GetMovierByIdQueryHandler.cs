using AutoMapper;
using MediatR;
using MovieStreaming.Application.DTOs;
using MovieStreaming.Application.Interfaces;
using MovieStreaming.Domain.Aggregates.Movies;
using MovieStreaming.Domain.Common.Result;
using MovieStreaming.Infrastructure.Repository;

namespace MovieStreaming.Application.Queries.MovieQueries
{
    public class GetMovierByIdQueryHandler : IRequestHandler<GetMovieByIdQuery, Result<MovieDto>>
    {
        private readonly IMovieRepository _movieRepository;
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public GetMovierByIdQueryHandler(IMovieRepository movieRepository, IUnitOfWork unitOfWork, IMapper mapper, IUserRepository userRepository)
        {
            _movieRepository = movieRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userRepository = userRepository;
        }


        public async Task<Result<MovieDto>> Handle(GetMovieByIdQuery request, CancellationToken cancellationToken)
        {
            var movie = await _movieRepository.GetByIdWithReviewsAsync(request.id);
            if (movie == null) return Result.Failure<MovieDto>(new("Movie.NotFound", "Movie Not Found"));

            // Map your reviews manually or tell AutoMapper how to handle the extra userName field
            var reviewDtos = new List<ReviewDto>();
            foreach (var r in movie.Reviews)
            {
                var userEntity = await _userRepository.GetUserById(r.UserId);
                reviewDtos.Add(new ReviewDto(r.Id, r.UserId, userEntity?.Name ?? "Anonymous", r.Rating, r.Comment));
            }

            var result = new MovieDto(movie.Id, movie.Title, movie.Description, movie.Duration,movie.ReleaseDate,movie.VideoUrl,movie.PosterUrl, reviewDtos);
            return Result.Success<MovieDto>(result);
        }
    }
}
