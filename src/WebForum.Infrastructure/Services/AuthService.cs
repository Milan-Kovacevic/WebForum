using System.Security.Cryptography;
using WebForum.Application.Abstractions.Services;
using WebForum.Application.Models;
using WebForum.Domain.Enums;
using WebForum.Infrastructure.Authentication.OAuth.Handlers.GitHub;

namespace WebForum.Infrastructure.Services;

public class AuthService(IGitHubOAuthHandler gitHubOAuthHandler)
    : IAuthService
{
    public Task<string> Generate2FaCode(int codeSize, CancellationToken cancellationToken = default)
    {
        if (codeSize is < 0 or > 10)
            throw new InvalidOperationException();
        var endNumberExclusive = Math.Pow(10.0, codeSize);
        var codeValue = RandomNumberGenerator.GetInt32(0, (int)endNumberExclusive).ToString("D6");
        return Task.FromResult(codeValue);
    }

    public Task<OAuthResult> ExternallyAuthenticateUser(LoginProvider loginProvider, string providerCode)
    {
        if (loginProvider == LoginProvider.GitHub)
            return gitHubOAuthHandler.AuthenticateUserExternally(providerCode);
        throw new InvalidOperationException();
    }

    public string ComputePasswordHash(string text)
    {
        return BCrypt.Net.BCrypt.HashPassword(text);
    }

    public bool ValidatePasswordHash(string text, string hashText)
    {
        return BCrypt.Net.BCrypt.Verify(text, hashText);
    }
}
