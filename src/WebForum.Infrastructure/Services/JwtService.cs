using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using WebForum.Application.Abstractions.Services;
using WebForum.Application.Models;
using WebForum.Domain.Enums;
using WebForum.Infrastructure.Options;
using JwtRegisteredClaimNames = System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames;

namespace WebForum.Infrastructure.Services;

public class JwtService(IOptions<JwtOptions> options) : IJwtService
{
    private readonly JwtOptions _options = options.Value;
    private const string CustomTokenIdClaimName = "tokenId";
    private const string CustomTokenTypeClaimName = "type";

    public async Task<JwtTokensResult> GenerateUserTokens(Guid userId)
    {
        var accessTokenId = Guid.NewGuid();
        var refreshTokenId = Guid.NewGuid();
        var accessToken = await GenerateToken(accessTokenId, userId, TokenType.Access);
        var refreshToken = await GenerateToken(refreshTokenId, userId, TokenType.Refresh);

        var authTokens = new JwtTokensResult()
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

    public async Task<JwtClaimsResult?> ExtractTokenClaimValues(IEnumerable<Claim> claims)
    {
        var claimList = claims.ToList();
        var subClaim = claimList.FirstOrDefault(x => x.Type is ClaimTypes.NameIdentifier or JwtRegisteredClaimNames.Sub);
        var tokenClaim = claimList.FirstOrDefault(x => x.Type == CustomTokenIdClaimName);
        var tokenTypeClaim = claimList.FirstOrDefault(x => x.Type == CustomTokenTypeClaimName);
        if (subClaim is null || tokenClaim is null || tokenTypeClaim is null)
            return null;

        if (!Guid.TryParse(subClaim.Value, out var userId) || !Guid.TryParse(tokenClaim.Value, out var tokenId) ||
            !Enum.TryParse<TokenType>(tokenTypeClaim.Value, out var tokenType))
            return null;

        var claimValues = new JwtClaimsResult()
        {
            UserId = userId,
            TokenId = tokenId,
            Type = tokenType
        };
        return await Task.FromResult(claimValues);
    }

    public Task<TokenValidationResult> ValidateUserToken(string jwtToken, TokenType tokenType)
    {
        if (tokenType == TokenType.Access)
            return ValidateAccessToken(jwtToken);
        if(tokenType == TokenType.Refresh)
            return ValidateRefreshToken(jwtToken);

        throw new InvalidOperationException();
    }
    
    private async Task<TokenValidationResult> ValidateAccessToken(string jwtToken)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var validationParameters = new TokenValidationParameters()
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = false,
            ValidateIssuerSigningKey = true,
            ValidIssuer = _options.Issuer,
            ValidAudience = _options.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.AccessTokenSigningKey)),
            ClockSkew = TimeSpan.Zero
        };
        var validationResult = await tokenHandler.ValidateTokenAsync(jwtToken, validationParameters);
        return validationResult;
    }
    
    private async Task<TokenValidationResult> ValidateRefreshToken(string jwtToken)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var validationParameters = new TokenValidationParameters()
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = _options.Issuer,
            ValidAudience = _options.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.RefreshTokenSigningKey)),
            ClockSkew = TimeSpan.Zero
        };
        var validationResult = await tokenHandler.ValidateTokenAsync(jwtToken, validationParameters);
        return validationResult;
    }

    private Task<string> GenerateToken(Guid tokenId, Guid userId, TokenType tokenType)
    {
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()),
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