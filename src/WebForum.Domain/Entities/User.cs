using WebForum.Domain.Enums;

namespace WebForum.Domain.Entities;

public class User
{
    public Guid UserId { get; set; }
    public required string DisplayName { get; set; }
    public string? Username { get; set; }
    public string? Email { get; set; }
    public string? Password { get; set; }
    public required bool IsEnabled { get; set; }
    public DateTime LockoutEnd { get; set; }
    public required int AccessFailedCount { get; set; }
    public required UserRole Role { get; set; }
    public required Guid ConcurrencyStamp { get; set; }
}