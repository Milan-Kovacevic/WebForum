using System.Net.Http.Json;
using System.Text.Json.Nodes;
using WebForum.Application.Abstractions.Messaging;

namespace WebForum.Infrastructure.Messaging;

public class GitHubClient(HttpClient httpClient) : IGitHubClient
{
    private const string GitHubEndpoint = "/users/{username}";

    public async Task<JsonObject?> GetUserInfo(string username)
    {
        return await httpClient.GetFromJsonAsync<JsonObject>(GitHubEndpoint.Replace("{username}", username));
    }
}