using MediatR;
using MovieStreaming.Domain.Common.Result;
using MovieStreaming.Domain.Aggregates.Users;
using MovieStreaming.Application.Interfaces;
using Microsoft.AspNetCore.Identity;
using MovieStreaming.Domain.Enums;

namespace MovieStreaming.Application.Command.UserCommands
{
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, Result<User>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly IUnitOfWork _unitOfWork;

        public CreateUserCommandHandler(IUserRepository userRepository, IPasswordHasher<User> passwordHasher, IUnitOfWork unitOfWork)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<User>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var dto = request.Dto;
            IEnumerable<User> exists = await _userRepository.GetUserByEmail(dto.email);
            if (exists.Any()) return Result.Failure<User>(new("User.Create.Exists", "User Already Exists"));

            var user = new User(request.Id, dto.name, dto.email, SubscriptionType.None);
            var hashedPassword = _passwordHasher.HashPassword(user, dto.password);
            user.HashPsasword(hashedPassword);
            if (user == null) return Result.Failure<User>(new("User.Null", "User can not br null here"));

            await _userRepository.CreateUser(user);
            await _unitOfWork.CommitAsync(cancellationToken);

            return Result.Success<User>(user);

        }
    }
}
