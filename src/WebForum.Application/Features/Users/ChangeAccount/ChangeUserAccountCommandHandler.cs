using WebForum.Application.Abstractions.MediatR.Base;
using WebForum.Application.Abstractions.Repositories;
using WebForum.Domain.Enums;
using WebForum.Domain.Shared.Errors;
using WebForum.Domain.Shared.Results;
using UserRole = WebForum.Domain.Entities.UserRole;

namespace WebForum.Application.Features.Users.ChangeAccount;

public class ChangeUserAccountCommandHandler(
    IUserRepository userRepository,
    IRoleRepository roleRepository,
    IUserPermissionRepository userPermissionRepository)
    : ICommandHandler<ChangeUserAccountCommand>
{
    public async Task<Result> Handle(ChangeUserAccountCommand request, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetByIdAsync(request.UserId, cancellationToken);
        if (user is null)
            return Result.Failure(DomainErrors.User.NotFound(request.UserId));

        if (request.Role is not null)
        {
            var role = await roleRepository.GetByIdAsync((int)request.Role, cancellationToken);
            if (role is null)
                return Result.Failure(DomainErrors.Role.NotFound((int)request.Role));
            // If previous user role was changed from non-regular user to regular user -> Remove non-authoritative permissions.
            if (user.Role is not null && role.RoleId == UserRole.Regular.RoleId && user.Role.RoleId != UserRole.Regular.RoleId)
            {
                userPermissionRepository.DeleteAllUserPermissionsWithId(user.UserId,
                    (int)RoomPermission.PostComment);
                userPermissionRepository.DeleteAllUserPermissionsWithId(user.UserId,
                    (int)RoomPermission.BlockComment);
            }

            user.Role = role;
        }

        if (request.IsEnabled is not null)
        {
            user.IsEnabled = (bool)request.IsEnabled;
        }

        userRepository.Update(user);
        return Result.Success();
    }
}