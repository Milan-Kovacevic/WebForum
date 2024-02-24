namespace WebForum.Domain.Models;

public class AuthTokens
{
    public required string AccessToken { get; set; }
    public required string RefreshToken { get; set; }
    public required long ExpiresIn { get; set; }
}