using Microsoft.AspNetCore.Authorization;
using WebForum.Domain.Enums;

namespace WebForum.Infrastructure.Authentication.Attributes;

public class HasRoomPermissionAttribute(RoomPermission roomPermission)
    : AuthorizeAttribute(policy: roomPermission.ToString())
{
}