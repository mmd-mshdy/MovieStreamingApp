using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using MovieStreaming.Application.Command.WatchHistoryCommands;
using MovieStreaming.Application.Interfaces;
using MovieStreaming.Domain.Aggregates.Movies;
using MovieStreaming.Domain.Aggregates.Users;

public class UpdateWatchProgressCommandHandler
    : IRequestHandler<UpdateWatchProgressCommand>
{
    private readonly IWatchHistoryRepository _repository;
    private readonly IMovieRepository _movieRepository;
    private readonly ICurrentUserService _currentUser;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateWatchProgressCommandHandler(
        IWatchHistoryRepository repository,
        IMovieRepository movieRepository,
        ICurrentUserService currentUser,
        IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _movieRepository = movieRepository; 
        _currentUser = currentUser;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(
        UpdateWatchProgressCommand request,
        CancellationToken cancellationToken)
    {
        var userId = _currentUser.UserId;

        var movie = await _movieRepository.FindById(request.MovieId);
        if (movie == null)
        {
            throw new InvalidOperationException($"Cannot track progress for missing or unmapped Movie ID: {request.MovieId}");
        }
        var completed =
            request.PositionSeconds >=
            movie.Duration.TotalSeconds * 0.95;

        var history = await _repository.GetAsync(
            userId,
            request.MovieId);

        if (history is null)
        {
            history = new WatchHistory(
                userId,
                request.MovieId);

            await _repository.AddAsync(history);
        }

        history.UpdateProgress(
            TimeSpan.FromSeconds(request.PositionSeconds),
            completed);

        await _unitOfWork.CommitAsync(cancellationToken);
    }
}