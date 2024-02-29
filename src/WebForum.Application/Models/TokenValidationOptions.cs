namespace WebForum.Application.Models;

public class TokenValidationOptions
{
    public required bool ValidateIssuer { get; init; }
    public required bool ValidateAudience { get; init; }
    public required bool ValidateSignature { get; init; }
    public required bool ValidateExpiration { get; init; }
    public required bool IsAccessToken { get; init; }
}