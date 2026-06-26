using MovieStreaming.Application.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace MovieStreaming.Infrastructure.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public Guid UserId
        {
            get
            {
                var user = _httpContextAccessor.HttpContext?.User;
                if (user == null) return Guid.Empty;

                // Check standard Identity schema first, fallback to standard JWT Sub claim string
                var userId = user.FindFirstValue(ClaimTypes.NameIdentifier)
                             ?? user.FindFirstValue(JwtRegisteredClaimNames.Sub);

                return Guid.TryParse(userId, out var id) ? id : Guid.Empty;
            }
        }

        public bool IsAuthenticated =>
            _httpContextAccessor.HttpContext?.User?.Identity?.IsAuthenticated ?? false;

        // 🎬 Useful Extra: Instantly access subscription context anywhere in the application layer
        public string? SubscriptionType =>
            _httpContextAccessor.HttpContext?.User?.FindFirstValue("subscription_type");
    }
}