using WebForum.Domain.Enums;

namespace WebForum.Application.Models;

public class TokenClaimValues
{
    public required Guid UserId { get; init; }
    public required Guid TokenId { get; init; }
    public required TokenType Type { get; init; }
}