using MediatR;
using MovieStreaming.Application.Interfaces;
using MovieStreaming.Domain.Aggregates.Users;
using MovieStreaming.Domain.Common.Result;

namespace MovieStreaming.Application.Queries.UserQueries
{
    public class GetUserbyIdQueryHandler : IRequestHandler<GetUserByIdQuery, Result<User>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;

        public GetUserbyIdQueryHandler(IUserRepository userRepository, IUnitOfWork unitOfWork)
        {
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<User>> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        {
            var foundUser = await _userRepository.GetUserById(request.id);
            if (foundUser == null) return Result.Failure<User>(new("USer.NotFound", "USer not found"));
            await _unitOfWork.CommitAsync(cancellationToken);
            return Result.Success<User>(foundUser);

        }
    }
}
