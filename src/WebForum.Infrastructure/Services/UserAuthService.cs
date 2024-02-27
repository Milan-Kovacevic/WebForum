using WebForum.Application.Abstractions.Repositories;
using WebForum.Domain.Entities;
using WebForum.Domain.Enums;
using WebForum.Infrastructure.Authentication;
using UserRole = WebForum.Domain.Entities.UserRole;

namespace WebForum.Infrastructure.Services;

public class UserAuthService(IUserRepository userRepository) : IUserAuthService
{
    public async Task<UserRole?> GetUserRole(Guid userId, CancellationToken cancellationToken = default)
    {
        var user = await userRepository.GetByIdAsync(userId, cancellationToken);
        return user?.Role;
    }

    public async Task<IEnumerable<Permission>> GetUserRoomPermissions(Guid userId, Guid roomId,
        CancellationToken cancellationToken = default)
    {
        var user = await userRepository.GetByIdWithPermissionsAsync(userId, cancellationToken);
        return user?.Permissions.Select(x => x.Permission) ?? [];
    }
}