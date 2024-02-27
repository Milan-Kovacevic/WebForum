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
}