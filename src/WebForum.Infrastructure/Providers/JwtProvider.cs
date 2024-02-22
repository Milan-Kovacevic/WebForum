using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using WebForum.Application.Abstractions.Providers;
using WebForum.Domain.Entities;
using WebForum.Infrastructure.Settings;

namespace WebForum.Infrastructure.Providers;

public class JwtProvider(IOptions<JwtOptions> options) : IJwtProvider
{
    private readonly JwtOptions _options = options.Value;
    
    public Task<string> GenerateUserToken(User user)
    {
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.UserId.ToString()),
            new Claim(JwtRegisteredClaimNames.Name, user.DisplayName),
            new Claim("role", user.Role.ToString())
        };
        var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.SecretKey));
        var credentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken(_options.Issuer, _options.Audience, claims, null,
            DateTime.UtcNow.AddHours(_options.ExpirationTime), credentials);
        var tokenHandler = new JwtSecurityTokenHandler();
        var tokenValue = tokenHandler.WriteToken(token);
        return Task.FromResult(tokenValue);
    }
}