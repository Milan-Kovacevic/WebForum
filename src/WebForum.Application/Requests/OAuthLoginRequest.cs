namespace WebForum.Application.Requests;

public record OAuthLoginRequest(string Code, string Provider);