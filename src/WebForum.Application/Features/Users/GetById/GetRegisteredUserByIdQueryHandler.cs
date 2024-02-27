using WebForum.Application.Abstractions.MediatR.Base;
using WebForum.Application.Abstractions.Repositories;
using WebForum.Application.Responses;
using WebForum.Domain.Shared.Errors;
using WebForum.Domain.Shared.Results;

namespace WebForum.Application.Features.Users.GetById;

public class GetRegisteredUserByIdQueryHandler(IUserRepository userRepository)
    : IQueryHandler<GetRegisteredUserByIdQuery, SingleRegisteredUserResponse>
{
    public async Task<Result<SingleRegisteredUserResponse>> Handle(GetRegisteredUserByIdQuery request,
        CancellationToken cancellationToken)
    {
        var registeredUser = await userRepository.GetByIdWithPermissionsAsync(request.UserId, cancellationToken);
        if (registeredUser is null)
            return Result.Failure<SingleRegisteredUserResponse>(DomainErrors.User.NotFound(request.UserId));

        // TODO: Fix check for external authentication flag
        var userPermissions =
            registeredUser.Permissions.Select(x =>
                new UserPermissionResponse(x.Permission.PermissionId, x.Permission.Name, x.Room.RoomId, x.Room.Name));
        var result = new SingleRegisteredUserResponse(registeredUser.UserId, registeredUser.DisplayName,
            registeredUser.IsEnabled, registeredUser.Role!.RoleId, registeredUser.Role.Name,
            registeredUser.Username is null, userPermissions);
        return Result.Success(result);
    }
}