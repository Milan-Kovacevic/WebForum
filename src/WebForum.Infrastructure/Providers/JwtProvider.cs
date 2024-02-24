using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using WebForum.Application.Abstractions.Providers;
using WebForum.Domain.Entities;
using WebForum.Domain.Models;
using WebForum.Infrastructure.Options;

namespace WebForum.Infrastructure.Providers;

public class JwtProvider(IOptions<JwtOptions> options) : IJwtProvider
{
    private readonly JwtOptions _options = options.Value;
    
    public async Task<AuthTokens> GenerateUserToken(User user)
    {
        var accessToken = await GenerateAccessToken(user);
        var refreshToken = await GenerateRefreshToken(user);
        var authTokens = new AuthTokens()
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken,
            ExpiresIn = _options.RefreshTokenExpirationTime
        };
        return authTokens;
    }

    private Task<string> GenerateRefreshToken(User user)
    {
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.UserId.ToString()),
        };
        var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.RefreshTokenSigningKey));
        var credentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha512);
        var token = new JwtSecurityToken(_options.Issuer, _options.Audience, claims, null,
            DateTime.UtcNow.AddSeconds(_options.RefreshTokenExpirationTime), credentials);
        var tokenHandler = new JwtSecurityTokenHandler();
        var tokenValue = tokenHandler.WriteToken(token);
        return Task.FromResult(tokenValue);
    }

    private Task<string> GenerateAccessToken(User user)
    {
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.UserId.ToString()),
            new Claim(JwtRegisteredClaimNames.Name, user.DisplayName),
            new Claim("role", user.Role.ToString())
        };
        var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.AccessTokenSigningKey));
        var credentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha384);
        var token = new JwtSecurityToken(_options.Issuer, _options.Audience, claims, null,
            DateTime.UtcNow.AddSeconds(_options.AccessTokenExpirationTime), credentials);
        var tokenHandler = new JwtSecurityTokenHandler();
        var tokenValue = tokenHandler.WriteToken(token);
        return Task.FromResult(tokenValue);
    }
}