using WebForum.Application.Abstractions.MediatR.Base;
using WebForum.Application.Abstractions.Repositories;
using WebForum.Domain.Shared.Errors;
using WebForum.Domain.Shared.Results;

namespace WebForum.Application.Features.Users.ChangeRole;

public class ChangeUserRoleCommandHandler(IUserRepository userRepository, IRoleRepository roleRepository)
    : ICommandHandler<ChangeUserRoleCommand>
{
    public async Task<Result> Handle(ChangeUserRoleCommand request, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetByIdAsync(request.UserId, cancellationToken);
        if (user is null)
            return Result.Failure(DomainErrors.User.NotFound(request.UserId));
        var role = await roleRepository.GetByIdAsync((int)request.Role, cancellationToken);
        if (role is null)
            return Result.Failure(DomainErrors.Role.NotFound((int)request.Role));

        user.Role = role;
        userRepository.Update(user);
        return Result.Success();
    }
}