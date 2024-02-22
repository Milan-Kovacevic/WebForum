namespace WebForum.Api.Requests;

public record LoginRequest(string Username, string Password, string? TwoFactorCode);