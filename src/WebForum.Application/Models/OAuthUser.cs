namespace WebForum.Application.Models;

public class OAuthUser
{
    public required string ProviderId { get; init; }
    public required string DisplayName { get; init; }
}