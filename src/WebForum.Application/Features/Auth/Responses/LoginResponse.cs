namespace WebForum.Application.Features.Auth.Responses;

public record LoginResponse(string AccessToken, string RefreshToken);