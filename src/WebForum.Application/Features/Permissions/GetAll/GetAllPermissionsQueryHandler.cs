using WebForum.Application.Abstractions.MediatR.Base;
using WebForum.Application.Abstractions.Repositories;
using WebForum.Application.Responses;
using WebForum.Domain.Shared.Results;

namespace WebForum.Application.Features.Permissions.GetAll;

public class GetAllPermissionsQueryHandler(IPermissionRepository permissionRepository)
    : IQueryHandler<GetAllPermissionsQuery, IEnumerable<PermissionResponse>>
{
    public async Task<Result<IEnumerable<PermissionResponse>>> Handle(GetAllPermissionsQuery request,
        CancellationToken cancellationToken)
    {
        var permissions = await permissionRepository.GetAllAsync(cancellationToken);
        var result = permissions.Select(x =>
            new PermissionResponse(x.PermissionId, x.Name));
        return Result.Success(result);
    }
}