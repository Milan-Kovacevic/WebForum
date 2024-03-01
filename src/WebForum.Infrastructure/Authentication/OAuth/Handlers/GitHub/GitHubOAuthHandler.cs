using System.Text.Json;
using Microsoft.Extensions.Options;
using WebForum.Application.Models;
using WebForum.Infrastructure.Options;

namespace WebForum.Infrastructure.Authentication.OAuth.Handlers.GitHub;

public class GitHubOAuthHandler(IOptions<GitHubOptions> options, IHttpClientFactory httpClientFactory)
    : OAuthHandler<GitHubOptions>(options, httpClientFactory), IGitHubOAuthHandler
{
    private const string IdPropertyName = "id";
    private const string NamePropertyName = "name";

    protected override OAuthUser? ExtractOAuthUser(JsonElement userData)
    {
        if (!userData.TryGetProperty(IdPropertyName, out var userId))
            return null;
        if (!userData.TryGetProperty(NamePropertyName, out var displayName))
            return null;
        if (userId.ValueKind != JsonValueKind.Number || displayName.ValueKind != JsonValueKind.String)
            return null;
        return new OAuthUser()
        {
            ProviderId = userId.GetInt32().ToString(),
            DisplayName = displayName.GetString()!
        };
    }
}