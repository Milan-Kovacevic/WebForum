using System.Text.Json;
using Microsoft.Extensions.Options;
using WebForum.Application.Models;
using WebForum.Infrastructure.Options;

namespace WebForum.Infrastructure.Authentication.OAuth.Handlers.Google;

public class GoogleOAuthHandler(IOptions<GoogleOptions> options, IHttpClientFactory httpClientFactory)
    : OAuthHandler<GoogleOptions>(options, httpClientFactory), IGoogleOAuthHandler
{
    private const string UserNamesPropertyName = "names";
    private const string UserMetadataPropertyName = "metadata";
    private const string UserMetadataSourcePropertyName = "source";
    private const string IdPropertyName = "id";
    private const string NamePropertyName = "displayName";
    
    protected override OAuthUser? ExtractOAuthUser(JsonElement userData)
    {
        var userNames = userData.GetProperty(UserNamesPropertyName);
        var userMetadataSource = userNames[0].GetProperty(UserMetadataPropertyName).GetProperty(UserMetadataSourcePropertyName);
        if (!userMetadataSource.TryGetProperty(IdPropertyName, out var userId))
            return null;
        if (!userNames[0].TryGetProperty(NamePropertyName, out var displayName))
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