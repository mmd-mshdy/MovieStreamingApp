using MovieStreaming.Domain.Aggregates.Users;

namespace MovieStreaming.Application.Interfaces
{
    public interface IJwtTokenGenerator
    {
        string GenerateToken(User user);
    }
}