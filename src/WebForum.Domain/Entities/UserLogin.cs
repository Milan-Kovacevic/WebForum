using WebForum.Domain.Enums;

namespace WebForum.Domain.Entities;

public class UserLogin
{
    public required LoginProvider LoginProvider { get; set; }
    public required string ProviderKey { get; set; }
    public required Guid UserId { get; set; }
    public User? User { get; set; }
}