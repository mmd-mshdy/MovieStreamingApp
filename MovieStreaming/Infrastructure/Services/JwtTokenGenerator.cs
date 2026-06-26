using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MovieStreaming.Application.Interfaces;
using MovieStreaming.Domain.Aggregates.Users;

namespace MovieStreaming.Infrastructure.Services
{
    public class JwtTokenGenerator : IJwtTokenGenerator
    {
        private readonly IConfiguration _configuration;

        public JwtTokenGenerator(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GenerateToken(User user)
        {
            // 1. Fetch JWT settings from configuration
            var secretKey = _configuration["JwtSettings:Secret"]
              ?? throw new InvalidOperationException("JWT Secret key is not configured.");
            var issuer = _configuration["JwtSettings:Issuer"];
            var audience = _configuration["JwtSettings:Audience"];
            var expiryMinutes = double.Parse(_configuration["JwtSettings:ExpiryInMinutes"] ?? "60");

            // 2. Setup the security key and credentials
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // 3. Define the user claims (User context stored safely inside the token)
            var claims = new[]
            {
        new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
        new Claim(JwtRegisteredClaimNames.Email, user.Email ?? ""),
        new Claim(JwtRegisteredClaimNames.Name, user.Name ?? ""),
        new Claim("subscription_type", user.SubscriptionType.ToString()) // Useful for premium authorization later!
      };

            // 4. Create and configure the token object
            var token = new JwtSecurityToken(
              issuer: issuer,
              audience: audience,
              claims: claims,
              expires: DateTime.UtcNow.AddMinutes(expiryMinutes),
              signingCredentials: credentials);

            // 5. Serialize the token into its string format
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}