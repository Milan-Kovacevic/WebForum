using WebForum.Domain.Enums;

namespace WebForum.Application.Models;

public class JwtClaimsResult
{
    public required Guid UserId { get; init; }
    public required Guid TokenId { get; init; }
    public required TokenType Type { get; init; }
}