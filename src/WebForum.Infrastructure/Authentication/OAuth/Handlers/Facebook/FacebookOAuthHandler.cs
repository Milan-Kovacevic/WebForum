using System.Text.Json;
using Microsoft.Extensions.Options;
using WebForum.Application.Models;
using WebForum.Infrastructure.Options;

namespace WebForum.Infrastructure.Authentication.OAuth.Handlers.Facebook;

public class FacebookOAuthHandler(IOptions<FacebookOptions> options, IHttpClientFactory httpClientFactory)
    : OAuthHandler<FacebookOptions>(options, httpClientFactory), IFacebookOAuthHandler
{
    private const string IdPropertyName = "id";
    private const string NamePropertyName = "name";
    
    protected override OAuthUser? ExtractOAuthUser(JsonElement userData)
    {
        if (!userData.TryGetProperty(IdPropertyName, out var userId))
            return null;
        if (!userData.TryGetProperty(NamePropertyName, out var displayName))
            return null;
        if (userId.ValueKind != JsonValueKind.String || displayName.ValueKind != JsonValueKind.String)
            return null;
        return new OAuthUser()
        {
            ProviderId = userId.ToString(),
            DisplayName = displayName.GetString()!
        };
    }
}