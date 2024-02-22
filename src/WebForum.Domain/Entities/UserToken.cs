using WebForum.Domain.Enums;

namespace WebForum.Domain.Entities;

public class UserToken
{
    public required Guid TokenId { get; init; }
    public required Guid UserId { get; init; }
    public required TokenType Type { get; init; }
    public required string Value { get; init; }
    public User? User { get; init; }
}