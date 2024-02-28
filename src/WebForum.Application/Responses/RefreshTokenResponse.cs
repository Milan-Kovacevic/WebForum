namespace WebForum.Application.Responses;

public record RefreshTokenResponse(string AccessToken, string RefreshToken, long ExpiresIn);