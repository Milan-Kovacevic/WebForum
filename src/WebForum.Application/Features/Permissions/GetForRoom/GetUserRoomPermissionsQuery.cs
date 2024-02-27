using WebForum.Application.Abstractions.MediatR.Base;
using WebForum.Application.Responses;

namespace WebForum.Application.Features.Permissions.GetForRoom;

public record GetUserRoomPermissionsQuery(Guid UserId, Guid RoomId) : IQuery<IEnumerable<PermissionResponse>>
{
    public RequestFlag Type => RequestFlag.Query;
}