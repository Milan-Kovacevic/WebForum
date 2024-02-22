using System.Text.Json.Nodes;

namespace WebForum.Application.Abstractions.Messaging;

public interface IGitHubClient
{
    Task<JsonObject?> GetUserInfo(string username);
}