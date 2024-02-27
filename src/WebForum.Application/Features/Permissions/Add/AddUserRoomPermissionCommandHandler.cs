using WebForum.Application.Abstractions.MediatR.Base;
using WebForum.Application.Abstractions.Repositories;
using WebForum.Domain.Entities;
using WebForum.Domain.Shared.Errors;
using WebForum.Domain.Shared.Results;

namespace WebForum.Application.Features.Permissions.Add;

public class AddUserRoomPermissionCommandHandler(
    IUserRepository userRepository,
    IRoomRepository roomRepository,
    IPermissionRepository permissionRepository,
    IUserPermissionRepository userPermissionRepository)
    : ICommandHandler<AddUserRoomPermissionCommand>
{
    public async Task<Result> Handle(AddUserRoomPermissionCommand request, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetByIdAsync(request.UserId, cancellationToken);
        if (user is null)
            return Result.Failure(DomainErrors.User.NotFound(request.UserId));
        var room = await roomRepository.GetByIdAsync(request.RoomId, cancellationToken);
        if (room is null)
            return Result.Failure(DomainErrors.Room.NotFound(request.RoomId));
        var permission = await permissionRepository.GetByIdAsync(request.PermissionId, cancellationToken);
        if (permission is null)
            return Result.Failure(DomainErrors.Permission.NotFound(request.PermissionId));
        if (await userPermissionRepository.ExistsByIdAsync(request.UserId, request.RoomId, request.PermissionId,
                cancellationToken))
            return Result.Failure(
                DomainErrors.UserPermission.Conflict(request.UserId, request.RoomId, request.PermissionId));

        // TODO: Check if trying to add PostComment and BlockComment permission to regular user
        
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
        return Result.Success();
    }
}