namespace WebForum.Application.Requests;

public record LoginRequest(string Username, string Password, string? TwoFactorCode);