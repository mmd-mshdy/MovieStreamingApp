using AutoMapper;
using MediatR;
using MovieStreaming.Application.DTOs;
using MovieStreaming.Application.Interfaces;
using MovieStreaming.Domain.Common.Errors;
using MovieStreaming.Domain.Common.Result;

namespace MovieStreaming.Application.Queries.MovieQueries
{
    public class GetAllMoviesQueryHandler : IRequestHandler<GetAllMoviesQuery , Result<IEnumerable<MovieDto>>>
    {
        private readonly IMovieRepository _repository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork unitOfWork;

        public GetAllMoviesQueryHandler(IMovieRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<Result<IEnumerable<MovieDto>>> Handle(GetAllMoviesQuery request, CancellationToken cancellationToken)
        {
            
            var movies = await _repository.GetAllAsync();
            var result = _mapper.Map<IEnumerable<MovieDto>>(movies);
            if (!result.Any()) return Result.Failure<IEnumerable<MovieDto>>(new Error("Movie.NotFound", "MOvies were not found"));
            return Result.Success(result);

        }
    }
}
