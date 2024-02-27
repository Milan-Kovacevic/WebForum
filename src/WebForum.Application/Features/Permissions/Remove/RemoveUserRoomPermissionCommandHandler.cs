using WebForum.Application.Abstractions.MediatR.Base;
using WebForum.Application.Abstractions.Repositories;
using WebForum.Domain.Shared.Errors;
using WebForum.Domain.Shared.Results;

namespace WebForum.Application.Features.Permissions.Remove;

public class RemoveUserRoomPermissionCommandHandler(IUserPermissionRepository userPermissionRepository)
    : ICommandHandler<RemoveUserRoomPermissionCommand>
{
    public async Task<Result> Handle(RemoveUserRoomPermissionCommand request, CancellationToken cancellationToken)
    {
        var userPermission = await userPermissionRepository.GetByIdAsync(request.UserId, request.RoomId,
            request.PermissionId, cancellationToken);
        if (userPermission is null)
            return Result.Failure(
                DomainErrors.UserPermission.NotFound(request.UserId, request.RoomId, request.PermissionId));

        userPermissionRepository.Delete(userPermission);
        return Result.Success();
    }
}