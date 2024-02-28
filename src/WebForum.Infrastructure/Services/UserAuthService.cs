using WebForum.Application.Abstractions.Repositories;
using WebForum.Application.Abstractions.Services;
using WebForum.Domain.Entities;
using UserRole = WebForum.Domain.Entities.UserRole;

namespace WebForum.Infrastructure.Services;

public class UserAuthService(IUserRepository userRepository, IUnitOfWork unitOfWork) : IUserAuthService
{
    public async Task<UserRole?> GetUserRole(Guid userId, CancellationToken cancellationToken = default)
    {
        var user = await userRepository.GetByIdAsync(userId, cancellationToken);
        return user?.Role;
    }

    public async Task<IEnumerable<UserPermission>> GetUserPermissions(Guid userId, CancellationToken cancellationToken = default)
    {
        var user = await userRepository.GetByIdWithPermissionsAsync(userId, cancellationToken);
        return user?.Permissions ?? [];
    }

    public string ComputePasswordHash(string text)
    {
        return BCrypt.Net.BCrypt.HashPassword(text);
    }

    public bool ValidatePasswordHash(string text, string hashText)
    {
        return BCrypt.Net.BCrypt.Verify(text, hashText);
    }
}