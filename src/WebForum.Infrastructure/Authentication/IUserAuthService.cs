using WebForum.Domain.Entities;
using WebForum.Domain.Enums;
using UserRole = WebForum.Domain.Entities.UserRole;

namespace WebForum.Infrastructure.Authentication;

public interface IUserAuthService
{
    Task<UserRole?> GetUserRole(Guid userId, CancellationToken cancellationToken = default);
}