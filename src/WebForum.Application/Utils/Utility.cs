using System.Security.Claims;

namespace WebForum.Application.Utils;

public static class Utility
{
    public static string ComputeHash(string text)
    {
        return BCrypt.Net.BCrypt.HashPassword(text);
    }
    
    public static bool ValidateHash(string text, string hashText)
    {
        return BCrypt.Net.BCrypt.Verify(text, hashText);
    }

    public static Guid? ExtractIdFromUserClaims(IEnumerable<Claim> claims)
    {
        var subject = claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
        if (subject is null || !Guid.TryParse(subject, out var userId))
            return null;
        return userId;
    }
}