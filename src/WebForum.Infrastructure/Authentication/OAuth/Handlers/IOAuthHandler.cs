using WebForum.Application.Models;

namespace WebForum.Infrastructure.Authentication.OAuth.Handlers;

public interface IOAuthHandler
{
    Task<OAuthResult> AuthenticateUserExternally(string providerCode);
}