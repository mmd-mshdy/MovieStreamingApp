using AutoMapper;
using MediatR;
using MovieStreaming.Application.DTOs;
using MovieStreaming.Application.Interfaces;
using MovieStreaming.Domain.Aggregates.Movies;
using MovieStreaming.Domain.Common.Result;

namespace MovieStreaming.Application.Queries.MovieQueries
{
    public class GetMovierByIdQueryHandler : IRequestHandler<GetMovieByIdQuery, Result<MovieDto>>
    {
        private readonly IMovieRepository _movieRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public GetMovierByIdQueryHandler(IMovieRepository movieRepository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _movieRepository = movieRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<MovieDto>> Handle(GetMovieByIdQuery request, CancellationToken cancellationToken)
        {
            var movie =await _movieRepository.FindById(request.id);
            if (movie == null) return Result.Failure<MovieDto>(new("Movie.NotFound", "Movie Not Found"));
            var result = _mapper.Map<MovieDto>(movie);
            return Result.Success<MovieDto>(result);
        }
    }
}
