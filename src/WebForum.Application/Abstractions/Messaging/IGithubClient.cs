using System.Text.Json.Nodes;

namespace WebForum.Application.Abstractions.Messaging;

public interface IGithubClient
{
    Task<JsonObject?> GetUserInfo(string username);
}