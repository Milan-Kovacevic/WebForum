using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using WebForum.Application.Abstractions.Services;
using WebForum.Application.Models;
using WebForum.Domain.Entities;
using WebForum.Domain.Enums;
using WebForum.Infrastructure.Options;
using JwtRegisteredClaimNames = System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames;

namespace WebForum.Infrastructure.Services;

public class JwtService(IOptions<JwtOptions> options, IUserAuthService userAuthService) : IJwtService
{
    private readonly JwtOptions _options = options.Value;
    private const string CustomTokenIdClaimName = "tokenId";
    private const string CustomTokenTypeClaimName = "type";

    public async Task<AuthToken> GenerateUserToken(User user, CancellationToken cancellationToken = default)
    {
        var accessTokenId = Guid.NewGuid();
        var refreshTokenId = Guid.NewGuid();
        var accessToken = await GenerateToken(accessTokenId, user, TokenType.Access);
        var refreshToken = await GenerateToken(refreshTokenId, user, TokenType.Refresh);

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

    public async Task<TokenClaimValues?> ExtractClaimValues(IEnumerable<Claim> claims,
        CancellationToken cancellationToken = default)
    {
        var claimList = claims.ToList();
        var subClaim = claimList.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Sub);
        var tokenClaim = claimList.FirstOrDefault(x => x.Type == CustomTokenIdClaimName);
        var tokenTypeClaim = claimList.FirstOrDefault(x => x.Type == CustomTokenTypeClaimName);
        if (subClaim is null || tokenClaim is null || tokenTypeClaim is null)
            return null;

        if (!Guid.TryParse(subClaim.Value, out var userId) || !Guid.TryParse(tokenClaim.Value, out var tokenId) ||
            !Enum.TryParse<TokenType>(tokenTypeClaim.Value, out var tokenType))
            return null;

        var claimValues = new TokenClaimValues()
        {
            UserId = userId,
            TokenId = tokenId,
            Type = tokenType
        };
        return await Task.FromResult(claimValues);
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