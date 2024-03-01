using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using WebForum.Application.Models;
using WebForum.Domain.Enums;

namespace WebForum.Application.Abstractions.Services;

public interface IAuthService
{
    Task<string> Generate2FaCode(int codeSize, CancellationToken cancellationToken = default);
    Task<OAuthResult> ExternallyAuthenticateUser(LoginProvider loginProvider, string providerCode);
    public string ComputePasswordHash(string text);
    public bool ValidatePasswordHash(string text, string hashText);
}