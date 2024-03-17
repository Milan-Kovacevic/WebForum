using WebForum.Application.Abstractions.MediatR.Base;
using WebForum.Application.Abstractions.Repositories;
using WebForum.Application.Responses;
using WebForum.Domain.Shared.Results;

namespace WebForum.Application.Features.Users.GetAll;

public class GetAllRegisteredUsersQueryHandler(IUserRepository userRepository)
    : IQueryHandler<GetAllRegisteredUsersQuery, IEnumerable<RegisteredUserResponse>>
{
    public async Task<Result<IEnumerable<RegisteredUserResponse>>> Handle(GetAllRegisteredUsersQuery request,
        CancellationToken cancellationToken)
    {
        var registeredUsers = await userRepository.GetAllRegisteredAsync(cancellationToken);
        var result =
            registeredUsers.Select(x => new RegisteredUserResponse(x.UserId, x.DisplayName, x.IsEnabled, x.Role!.RoleId));
        return Result.Success(result);
    }
}