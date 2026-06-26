using MediatR;
using Microsoft.AspNetCore.Identity;
using MovieStreaming.Application.DTOs;
using MovieStreaming.Application.Interfaces;
using MovieStreaming.Domain.Aggregates.Users;
using MovieStreaming.Domain.Common.Result;

namespace MovieStreaming.Application.Command.UserCommands
{
    public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, Result<AuthResponseDto>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;

        public LoginUserCommandHandler(
            IUserRepository userRepository,
            IPasswordHasher<User> passwordHasher,
            IJwtTokenGenerator jwtTokenGenerator)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _jwtTokenGenerator = jwtTokenGenerator;
        }

        public async Task<Result<AuthResponseDto>> Handle(LoginUserCommand request, CancellationToken cancellationToken)
        {
            // 1. Find the user by Email
            var user = await _userRepository.FindByEmailAsync(request.LoginDto.Email);
            if (user == null || string.IsNullOrEmpty(user.PasswordHash))
            {
                return Result.Failure<AuthResponseDto>(new("Auth.InvalidCredentials", "Invalid email or password."));
            }

            // 2. Verify password hash using ASP.NET Core Identity's Hasher
            var verificationResult = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, request.LoginDto.Password);
            if (verificationResult == PasswordVerificationResult.Failed)
            {
                return Result.Failure<AuthResponseDto>(new("Auth.InvalidCredentials", "Invalid email or password."));
            }

            // 3. Generate secure JWT token
            var token = _jwtTokenGenerator.GenerateToken(user);

            // 4. Return successful mapping with token details
            var response = new AuthResponseDto(user.Id, user.Name ?? "", user.Email ?? "", token);
            return Result.Success(response);
        }
    }
}