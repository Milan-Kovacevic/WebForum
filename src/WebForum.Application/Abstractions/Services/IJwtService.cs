using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using WebForum.Application.Models;
using WebForum.Domain.Enums;

namespace WebForum.Application.Abstractions.Services;

public interface IJwtService
{
    Task<JwtTokensResult> GenerateUserTokens(Guid userId);
    Task<TokenValidationResult> ValidateUserToken(string jwtToken, TokenType tokenType);
    Task<JwtClaimsResult?> ExtractTokenClaimValues(IEnumerable<Claim> claims);
}