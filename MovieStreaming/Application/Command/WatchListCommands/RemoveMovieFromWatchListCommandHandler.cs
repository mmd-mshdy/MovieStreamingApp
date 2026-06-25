using MediatR;
using MovieStreaming.Application.Interfaces;

namespace MovieStreaming.Application.Command.WatchListCommands
{
    public class RemoveMovieFromWatchListCommandHandler : IRequestHandler<RemoveMovieFromWatchListCommand>
    {
        private readonly IWatchListRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUser;

        public RemoveMovieFromWatchListCommandHandler(IWatchListRepository repository, IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
            _currentUser = currentUserService;
        }

        public async Task Handle(RemoveMovieFromWatchListCommand request, CancellationToken cancellationToken)
        {
            var userId = _currentUser.UserId;

            // Check if it already exists to prevent duplicates
            var exists = await _repository.ExistsAsync(userId, request.MovieId);
            if (exists) return;
            await _repository.RemoveAsync(userId, request.MovieId);

        }
    }
}
