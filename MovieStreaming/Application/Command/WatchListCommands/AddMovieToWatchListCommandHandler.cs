using MediatR;
using MovieStreaming.Application.Command.WatchListCommands;
using MovieStreaming.Application.Interfaces;
using MovieStreaming.Domain.Aggregates.Users;

public class AddMovieToWatchListCommandHandler : IRequestHandler<AddMovieToWatchListCommand>
{
    private readonly IWatchListRepository _repository; // You'll need this interface defined
    private readonly ICurrentUserService _currentUser;
    private readonly IUnitOfWork _unitOfWork;

    public AddMovieToWatchListCommandHandler(
        IWatchListRepository repository,
        ICurrentUserService currentUser,
        IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _currentUser = currentUser;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(AddMovieToWatchListCommand request, CancellationToken cancellationToken)
    {
        var userId = _currentUser.UserId;

        // Check if it already exists to prevent duplicates
        var exists = await _repository.ExistsAsync(userId, request.MovieId);
        if (exists) return;

        var watchListItem = new WatchList(Guid.NewGuid(), userId, request.MovieId);

        await _repository.AddAsync(watchListItem);
        await _unitOfWork.CommitAsync(cancellationToken);
    }
}