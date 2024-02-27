using Microsoft.EntityFrameworkCore;
using WebForum.Application.Abstractions.Repositories;
using WebForum.Domain.Entities;
using WebForum.Persistence.DbContext;

namespace WebForum.Persistence.Repositories;

public class UserPermissionRepository(ApplicationDbContext context) : IUserPermissionRepository
{
    public async Task<IEnumerable<UserPermission>> GetAllRoomPermissions(Guid userId, Guid roomId,
        CancellationToken cancellationToken = default)
    {
        return await context.Set<UserPermission>()
            .AsNoTracking()
            .Include(x => x.Permission)
            .Where(x => x.UserId == userId && x.RoomId == roomId)
            .ToListAsync(cancellationToken);
    }

    public async Task InsertAsync(UserPermission entity, CancellationToken cancellationToken = default)
    {
        await context.Set<UserPermission>().AddAsync(entity, cancellationToken);
    }

    public async Task<bool> ExistsByIdAsync(Guid userId, Guid roomId, int permissionId,
        CancellationToken cancellationToken = default)
    {
        return await context.Set<UserPermission>().FirstOrDefaultAsync(
            x => x.UserId == userId && x.RoomId == roomId && x.PermissionId == permissionId,
            cancellationToken) != default;
    }

    public async Task<UserPermission?> GetByIdAsync(Guid userId, Guid roomId, int permissionId,
        CancellationToken cancellationToken = default)
    {
        return await context.Set<UserPermission>().FindAsync([userId, roomId, permissionId], cancellationToken);
    }

    public void Delete(UserPermission entity)
    {
        context.Set<UserPermission>().Remove(entity);
    }
}