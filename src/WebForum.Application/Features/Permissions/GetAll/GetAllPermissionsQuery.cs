using WebForum.Application.Abstractions.MediatR.Base;
using WebForum.Application.Responses;

namespace WebForum.Application.Features.Permissions.GetAll;

public class GetAllPermissionsQuery : IQuery<IEnumerable<PermissionResponse>>
{
    public RequestFlag Type => RequestFlag.Query;
}