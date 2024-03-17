using WebForum.Application.Abstractions.MediatR.Base;
using WebForum.Application.Abstractions.Repositories;
using WebForum.Domain.Entities;
using WebForum.Domain.Enums;
using WebForum.Domain.Shared.Errors;
using WebForum.Domain.Shared.Results;
using UserRole = WebForum.Domain.Entities.UserRole;

namespace WebForum.Application.Features.Permissions.Change;

public class ChangeUserRoomPermissionsCommandHandler(
    IUserRepository userRepository,
    IRoomRepository roomRepository,
    IPermissionRepository permissionRepository,
    IUserPermissionRepository userPermissionRepository)
    : ICommandHandler<ChangeUserRoomPermissionsCommand>
{
    public async Task<Result> Handle(ChangeUserRoomPermissionsCommand request, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetByIdAsync(request.UserId, cancellationToken);
        if (user is null)
            return Result.Failure(DomainErrors.User.NotFound(request.UserId));
        var room = await roomRepository.GetByIdAsync(request.RoomId, cancellationToken);
        if (room is null)
            return Result.Failure(DomainErrors.Room.NotFound(request.RoomId));

        var allRoomPermissions =
            await userPermissionRepository.GetAllRoomPermissions(request.UserId, request.RoomId, cancellationToken);
        foreach (var roomPermission in allRoomPermissions)
        {
            if (!request.Permissions.Contains(roomPermission.PermissionId))
                userPermissionRepository.Delete(roomPermission);
        }

        foreach (var permissionId in request.Permissions)
        {
            if (await userPermissionRepository.ExistsByIdAsync(request.UserId, request.RoomId, permissionId,
                    cancellationToken))
                continue;

            var permission = await permissionRepository.GetByIdAsync(permissionId, cancellationToken);
            if (permission is null)
                return Result.Failure(DomainErrors.Permission.NotFound(permissionId));

            if (user.RoleId == UserRole.Regular.RoleId && permission.Name == RoomPermission.PostComment.ToString() ||
                permission.Name == RoomPermission.BlockComment.ToString())
                return Result.Failure(DomainErrors.Permission.NotAvailable(permissionId));

            var userPermission = new UserPermission
            {
                UserId = user.UserId,
                RoomId = room.RoomId,
                PermissionId = permission.PermissionId,
                User = user,
                Room = room,
                Permission = permission
            };
            await userPermissionRepository.InsertAsync(userPermission, cancellationToken);
        }

        return Result.Success();
    }
}