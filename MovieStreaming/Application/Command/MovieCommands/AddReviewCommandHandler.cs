using MediatR;
using MovieStreaming.Application.Interfaces;
using MovieStreaming.Domain.Aggregates.Movies;
using MovieStreaming.Domain.Common.Result;

namespace MovieStreaming.Application.Command.MovieCommands
{
    public class AddReviewCommandHandler : IRequestHandler<AddReviewCommand, Result<Review>>
    {
        private readonly IMovieRepository _movieRepository;
        private readonly IUnitOfWork _unitOfWork;

        public AddReviewCommandHandler(IMovieRepository movieRepository, IUnitOfWork unitOfWork)
        {
            _movieRepository = movieRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<Review>> Handle(AddReviewCommand request, CancellationToken cancellationToken)
        {
            var dto = request.Dto;
            var review = new Review(Guid.NewGuid(),request.Id, dto.userId, dto.rating, dto.comment);
            if (review == null) Result.Failure<Review>(new("Review.NotFound", "Review Mus not be null"));
            await _movieRepository.AddReview(request.MovieId, review);
            await _unitOfWork.CommitAsync(cancellationToken);
            return Result.Success(review);
        }
    }
}
