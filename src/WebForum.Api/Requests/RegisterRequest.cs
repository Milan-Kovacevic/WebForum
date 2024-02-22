namespace WebForum.Api.Requests;

public record RegisterRequest(string DisplayName, string Username, string Email, string Password);