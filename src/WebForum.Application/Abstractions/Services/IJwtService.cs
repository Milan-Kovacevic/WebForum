using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using WebForum.Application.Models;
using WebForum.Domain.Enums;

namespace WebForum.Application.Abstractions.Services;

public interface IJwtService
{
    Task<AuthToken> GenerateUserToken(Guid userId);
    Task<TokenClaimValues?> ExtractClaimValues(IEnumerable<Claim> claims);
    Task<TokenValidationResult> ValidateUserToken(string jwtToken,TokenType tokenType);
}