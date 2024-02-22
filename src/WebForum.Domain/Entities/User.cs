using WebForum.Domain.Enums;

namespace WebForum.Domain.Entities;

public class User
{
    public Guid UserId { get; init; }
    public required string DisplayName { get; init; }
    public string? Username { get; init; }
    public string? Email { get; init; }
    public string? Password { get; init; }
    public required bool IsEnabled { get; init; }
    public DateTime LockoutEnd { get; init; }
    public required int AccessFailedCount { get; init; }
    public required UserRole Role { get; init; }
    public required Guid ConcurrencyStamp { get; init; }
}