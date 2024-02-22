using WebForum.Domain.Enums;

namespace WebForum.Domain.Entities;

public class UserLogin
{
    public required LoginProvider LoginProvider { get; init; }
    public required string ProviderKey { get; init; }
    public required Guid UserId { get; init; }
    public User? User { get; init; }
}