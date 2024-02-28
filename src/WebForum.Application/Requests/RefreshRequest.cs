namespace WebForum.Application.Requests;

public record RefreshRequest(string AccessToken, string RefreshToken);