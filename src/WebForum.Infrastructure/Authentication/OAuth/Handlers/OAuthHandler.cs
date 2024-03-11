using System.Net.Http.Headers;
using System.Text.Json;
using Microsoft.Extensions.Options;
using WebForum.Application.Models;

namespace WebForum.Infrastructure.Authentication.OAuth.Handlers;

public abstract class OAuthHandler<TOptions>(IOptions<TOptions> options, IHttpClientFactory httpClientFactory)
    : IOAuthHandler where TOptions : OAuthOptions
{
    private readonly TOptions _options = options.Value;
    private const string ClientUserAgent = "WebForum.Api";

    public async Task<OAuthResult> AuthenticateUserExternally(string providerCode)
    {
        if (string.IsNullOrEmpty(providerCode))
            return Fail($"OAuth code was not found.");
        var jsonResult = await ExchangeCodeAsync(providerCode);
        if (!jsonResult.HasValue)
            return Fail("Failed to exchange code for access token.");

        var root = jsonResult.Value;
        if (!root.TryGetProperty("access_token", out var accessTokenElement) ||
            accessTokenElement.ValueKind != JsonValueKind.String)
            return Fail(ReportOAuthError(root, "Failed to retrieve access token."));

        var jsonUser = await ObtainUserDetailsAsync(accessTokenElement.GetString()!);
        if (jsonUser is null)
            return Fail("An error occurred while retrieving the user details.");

        var oauthUser = ExtractOAuthUser(jsonUser.RootElement);
        return oauthUser is null ? Fail("Unable to extract data from user details.") : Succeed(oauthUser);
    }

    private async Task<JsonElement?> ExchangeCodeAsync(string code,
        CancellationToken cancellationToken = default)
    {
        var tokenRequestParameters = new Dictionary<string, string>()
        {
            { "client_id", _options.ClientId },
            { "redirect_uri", _options.CallbackPath },
            { "client_secret", _options.ClientSecret },
            { "code", code },
            { "grant_type", "authorization_code" },
        };

        var requestContent = new FormUrlEncodedContent(tokenRequestParameters!);
        var httpClient = httpClientFactory.CreateClient();

        var requestMessage = new HttpRequestMessage(HttpMethod.Post, _options.TokenEndpoint);
        requestMessage.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        requestMessage.Content = requestContent;
        requestMessage.Version = httpClient.DefaultRequestVersion;
        var response = await httpClient.SendAsync(requestMessage, cancellationToken);
        var body = await response.Content.ReadAsStringAsync(cancellationToken);
        return !response.IsSuccessStatusCode ? null : JsonDocument.Parse(body).RootElement;
    }

    private async Task<JsonDocument?> ObtainUserDetailsAsync(string accessToken,
        CancellationToken cancellationToken = default)
    {
        using var request = new HttpRequestMessage(HttpMethod.Get, _options.UserInformationEndpoint);
        request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

        var httpClient = httpClientFactory.CreateClient();
        httpClient.DefaultRequestHeaders.UserAgent.ParseAdd(ClientUserAgent);
        using var response =
            await httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken);
        var body = await response.Content.ReadAsStringAsync(cancellationToken);
        if (!response.IsSuccessStatusCode)
            return null;
        return JsonDocument.Parse(body);
    }

    protected abstract OAuthUser? ExtractOAuthUser(JsonElement userData);

    private static string ReportOAuthError(JsonElement element, string falloutMessage)
    {
        if (element.TryGetProperty("error_description", out var description))
            return description.GetString() ?? "Unexpected error.";
        return falloutMessage;
    }

    private static OAuthResult Fail(string reason)
    {
        return new OAuthResult()
        {
            HasError = true,
            ErrorMessage = reason
        };
    }

    private static OAuthResult Succeed(OAuthUser user)
    {
        return new OAuthResult()
        {
            HasError = false,
            User = user
        };
    }
}