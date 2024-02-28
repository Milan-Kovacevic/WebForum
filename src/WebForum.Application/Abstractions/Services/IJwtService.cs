using System.Security.Claims;
using Microsoft.IdentityModel.JsonWebTokens;
using WebForum.Application.Models;
using WebForum.Domain.Entities;

namespace WebForum.Application.Abstractions.Services;

public interface IJwtService
{
    Task<AuthToken> GenerateUserToken(User user, CancellationToken cancellationToken = default);
    Task<TokenClaimValues?> ExtractClaimValues(IEnumerable<Claim> claims, CancellationToken cancellationToken = default);
}