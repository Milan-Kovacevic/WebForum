using WebForum.Application.Abstractions.MediatR.Base;
using WebForum.Application.Responses;

namespace WebForum.Application.Features.Users.GetAll;

public record GetAllRegisteredUsersQuery : IQuery<IEnumerable<RegisteredUserResponse>>
{
    public RequestFlag Type => RequestFlag.Query;
}