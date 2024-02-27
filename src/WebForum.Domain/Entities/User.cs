using WebForum.Domain.Enums;

namespace WebForum.Domain.Entities;

public class User
{
    public Guid UserId { get; init; }
    public required string DisplayName { get; set; }
    public string? Username { get; set; }
    public string? Email { get; set; }
    public string? PasswordHash { get; set; }
    public required bool IsEnabled { get; set; }
    public DateTime? LockoutEnd { get; set; }
    public required int AccessFailedCount { get; set; }
    public int RoleId { get; set; }
    public UserRole? Role { get; set; }
    public RegistrationRequest? RegistrationRequest { get; set; }
    public IEnumerable<UserPermission> Permissions { get; set; }
}