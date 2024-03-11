using WebForum.Application.Abstractions.MediatR.Base;
using WebForum.Application.Responses;

namespace WebForum.Application.Features.Auth.MyInfo;

public record MyInfoQuery(Guid UserId) : IQuery<MyInfoResponse>
{
    public RequestFlag Type => RequestFlag.Query;
}