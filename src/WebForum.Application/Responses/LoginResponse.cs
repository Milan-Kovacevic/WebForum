namespace WebForum.Application.Responses;

public record LoginResponse(string AccessToken, string RefreshToken, long ExpiresIn);