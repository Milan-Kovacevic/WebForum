using WebForum.Application.Abstractions.MediatR.Base;
using WebForum.Application.Abstractions.Repositories;
using WebForum.Application.Responses;
using WebForum.Domain.Shared.Errors;
using WebForum.Domain.Shared.Results;

namespace WebForum.Application.Features.Permissions.GetForRoom;

public class GetUserRoomPermissionsQueryHandler(
    IUserRepository userRepository,
    IRoomRepository roomRepository,
    IUserPermissionRepository userPermissionRepository)
    : IQueryHandler<GetUserRoomPermissionsQuery, IEnumerable<PermissionResponse>>
{
    public async Task<Result<IEnumerable<PermissionResponse>>> Handle(GetUserRoomPermissionsQuery request,
        CancellationToken cancellationToken)
    {
        if (!await userRepository.ExistsByIdAsync(request.UserId, cancellationToken))
            return Result.Failure<IEnumerable<PermissionResponse>>(DomainErrors.User.NotFound(request.UserId));
        if (!await roomRepository.ExistsByIdAsync(request.RoomId, cancellationToken))
            return Result.Failure<IEnumerable<PermissionResponse>>(DomainErrors.Room.NotFound(request.UserId));

        var userRoomPermissions =
            await userPermissionRepository.GetAllRoomPermissions(request.UserId, request.RoomId, cancellationToken);
        var result = userRoomPermissions.Select(x => new PermissionResponse(x.PermissionId, x.Permission.Name));
        return Result.Success(result);
    }
}