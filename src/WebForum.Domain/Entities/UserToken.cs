using WebForum.Domain.Enums;

namespace WebForum.Domain.Entities;

public class UserToken
{
    public required Guid UserId { get; set; }
    public required TokenType Type { get; set; }
    public required string Value { get; set; }
}