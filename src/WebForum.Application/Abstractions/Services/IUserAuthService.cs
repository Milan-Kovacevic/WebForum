using WebForum.Domain.Entities;
using UserRole = WebForum.Domain.Entities.UserRole;

namespace WebForum.Application.Abstractions.Services;

public interface IUserAuthService
{
    Task<UserRole?> GetUserRole(Guid userId, CancellationToken cancellationToken = default);
    Task<IEnumerable<UserPermission>> GetUserPermissions(Guid userId, CancellationToken cancellationToken = default);
    public string ComputePasswordHash(string text);
    public bool ValidatePasswordHash(string text, string hashText);
}