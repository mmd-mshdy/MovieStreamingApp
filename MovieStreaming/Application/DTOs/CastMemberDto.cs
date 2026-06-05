using MovieStreaming.Domain.Enums;

namespace MovieStreaming.Application.DTOs
{
    public record CastMemberDto(string name, string familyName, string description, bool isfavorite , CastPositon CastPositon);
}
