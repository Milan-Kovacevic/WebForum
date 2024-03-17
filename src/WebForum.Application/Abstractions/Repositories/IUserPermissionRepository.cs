using WebForum.Domain.Entities;

namespace WebForum.Application.Abstractions.Repositories;

public interface IUserPermissionRepository
{
    Task<IEnumerable<UserPermission>> GetAllRoomPermissions(Guid userId, Guid roomId,
        CancellationToken cancellationToken = default);

    Task InsertAsync(UserPermission entity, CancellationToken cancellationToken = default);

    Task<bool> ExistsByIdAsync(Guid userId, Guid roomId, int permissionId,
        CancellationToken cancellationToken = default);
    
    Task<UserPermission?> GetByIdAsync(Guid userId, Guid roomId, int permissionId,
        CancellationToken cancellationToken = default);
    
    void Delete(UserPermission entity);
    
    void DeleteUserPermissions(UserPermission[] entities);
}