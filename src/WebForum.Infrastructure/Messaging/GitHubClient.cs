using WebForum.Application.Abstractions.Messaging;

namespace WebForum.Infrastructure.Messaging;

public class GitHubClient(HttpClient httpClient) : IGitHubClient
{
    private const string AuthorizationEndpoint = "https://github.com/login/oauth/authorize";
    private const string AccessTokenEndpoint = "https://github.com/login/oauth/access_token";
    private const string UserInformationEndpoint = "https://api.github.com/user";
}