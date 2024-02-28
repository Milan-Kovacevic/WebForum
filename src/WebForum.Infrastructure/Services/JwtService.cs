using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using WebForum.Application.Abstractions.Services;
using WebForum.Application.Models;
using WebForum.Domain.Entities;
using WebForum.Domain.Enums;
using WebForum.Infrastructure.Options;

namespace WebForum.Infrastructure.Services;

public class JwtService(IOptions<JwtOptions> options) : IJwtService
{
    private readonly JwtOptions _options = options.Value;
    private const string CustomTokenIdClaimName = "tokenId";
    private const string CustomTokenTypeClaimName = "type";

    public async Task<AuthToken> GenerateUserToken(User user)
    {
        var accessTokenId = Guid.NewGuid();
        var refreshTokenId = Guid.NewGuid();
        var accessToken = await GenerateToken(accessTokenId, user, TokenType.Access);
        var refreshToken = await GenerateToken(accessTokenId, user, TokenType.Refresh);

        var authTokens = new AuthToken()
        {
            AccessToken = accessToken,
            AccessTokenId = accessTokenId,
            RefreshToken = refreshToken,
            RefreshTokenId = refreshTokenId,
            RefreshTokenExpiration = _options.RefreshTokenExpirationTime,
            AccessTokenExpiration = _options.AccessTokenExpirationTime
        };
        return authTokens;
    }

    private Task<string> GenerateToken(Guid tokenId, User user, TokenType tokenType)
    {
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.UserId.ToString()),
            new Claim(CustomTokenIdClaimName, tokenId.ToString()),
            new Claim(CustomTokenTypeClaimName, tokenType.ToString()),
        };

        SigningCredentials credentials;
        DateTime expirationTime;
        switch (tokenType)
        {
            case TokenType.Access:
                credentials = new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.AccessTokenSigningKey)),
                    SecurityAlgorithms.HmacSha384);
                expirationTime = DateTime.UtcNow.AddSeconds(_options.AccessTokenExpirationTime);
                break;
            case TokenType.Refresh:
                credentials = new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.RefreshTokenSigningKey)),
                    SecurityAlgorithms.HmacSha512);
                expirationTime = DateTime.UtcNow.AddSeconds(_options.RefreshTokenExpirationTime);
                break;
            default:
                throw new ArgumentException("Token type is not recognized", tokenType.ToString());
        }

        var token = new JwtSecurityToken(_options.Issuer, _options.Audience, claims, null, expirationTime, credentials);
        var tokenHandler = new JwtSecurityTokenHandler();
        var tokenValue = tokenHandler.WriteToken(token);
        return Task.FromResult(tokenValue);
    }
}