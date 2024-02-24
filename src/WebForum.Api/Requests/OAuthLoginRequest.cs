namespace WebForum.Api.Requests;

public record OAuthLoginRequest(string Code, string Provider);