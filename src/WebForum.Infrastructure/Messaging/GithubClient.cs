using System.Net.Http.Json;
using System.Text.Json.Nodes;
using WebForum.Application.Abstractions.Messaging;

namespace WebForum.Infrastructure.Messaging;

public class GithubClient(HttpClient httpClient) : IGithubClient
{
    public async Task<JsonObject?> GetUserInfo(string username)
    {
        return await httpClient.GetFromJsonAsync<JsonObject>($"/users/{username}");
    }
}