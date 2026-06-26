namespace MovieStreaming.Application.DTOs
{
    public record LoginRequestDto(string Email, string Password);

    public record AuthResponseDto(Guid Id, string Name, string Email, string Token);
}