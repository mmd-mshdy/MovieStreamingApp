using AutoMapper;
using MediatR;
using MovieStreaming.Application.Command.Movie;
using MovieStreaming.Application.DTOs;
using MovieStreaming.Application.Interfaces;
using MovieStreaming.Domain.Aggregates.Movies;
using MovieStreaming.Domain.Common.Result;
using System.Reflection.Metadata.Ecma335;

namespace MovieStreaming.Application.Command.MovieCommands
{
    public class CreateMovieCommandHandler : IRequestHandler<CreateMovieCommand, Result<CreateMovieDto>>
    {
        private readonly IMovieRepository _movieRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public CreateMovieCommandHandler(IMovieRepository repository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _movieRepository = repository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<CreateMovieDto>> Handle(CreateMovieCommand request, CancellationToken cancellationToken)
        {
            var createDto = request.dto;
            IEnumerable<Domain.Aggregates.Movies.Movie> movieExists = await _movieRepository.FindByTitle(createDto.title);
            if (movieExists.Any()) return Result.Failure<CreateMovieDto>(new("Movie.CreateMovie.Error.MovieExists", "This movie already exists"));

            var newmovie = _mapper.Map<MovieStreaming.Domain.Aggregates.Movies.Movie>(createDto);
            if (newmovie == null) return Result.Failure<CreateMovieDto>(new("Movie.NullException", "Movie Must not Be Null"));
            await _movieRepository.CreateAsync(newmovie);

            await _unitOfWork.CommitAsync(cancellationToken);

            return Result.Success<CreateMovieDto>(createDto);
            

            
        }
    }
}
