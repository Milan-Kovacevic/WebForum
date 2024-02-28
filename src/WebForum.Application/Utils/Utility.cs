using System.Security.Claims;

namespace WebForum.Application.Utils;

public static class Utility
{
    public static Guid? ExtractIdFromUserClaims(IEnumerable<Claim> claims)
    {
        var subject = claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
        if (subject is null || !Guid.TryParse(subject, out var userId))
            return null;
        return userId;
    }
}