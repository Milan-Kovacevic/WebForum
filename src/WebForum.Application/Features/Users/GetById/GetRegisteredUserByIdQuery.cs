using WebForum.Application.Abstractions.MediatR.Base;
using WebForum.Application.Responses;

namespace WebForum.Application.Features.Users.GetById;

public record GetRegisteredUserByIdQuery(Guid UserId) : IQuery<SingleRegisteredUserResponse>
{
    public RequestFlag Type => RequestFlag.Query;
}