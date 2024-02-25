using WebForum.Domain.Enums;

namespace WebForum.Infrastructure.Authentication;

public interface IUserAuthService
{
    Task<UserRole?> GetUserRole(Guid userId, CancellationToken cancellationToken = default);
}