namespace WebForum.Application.Models;

public class JwtTokensResult
{
    public required string AccessToken { get; init; }
    public required Guid AccessTokenId { get; init; }
    public required long AccessTokenExpiration { get; init; }
    public required Guid RefreshTokenId { get; init; }
    public required string RefreshToken { get; init; }
    public required long RefreshTokenExpiration { get; init; }
    
}