using WebForum.Application.Abstractions.MediatR.Base;
using WebForum.Application.Abstractions.Repositories;
using WebForum.Application.Responses;
using WebForum.Domain.Enums;
using WebForum.Domain.Shared.Errors;
using WebForum.Domain.Shared.Results;

namespace WebForum.Application.Features.Auth.MyInfo;

public class MyInfoQueryHandler(IUserRepository userRepository) : IQueryHandler<MyInfoQuery, MyInfoResponse>
{
    public async Task<Result<MyInfoResponse>> Handle(MyInfoQuery request, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetByIdAsync(request.UserId, cancellationToken);
        if (user is null)
            return Result.Failure<MyInfoResponse>(DomainErrors.User.NotFound(request.UserId));

        var info = new MyInfoResponse(user.UserId, user.DisplayName, (UserRole)user.Role!.RoleId);
        return Result.Success(info);
    }
}