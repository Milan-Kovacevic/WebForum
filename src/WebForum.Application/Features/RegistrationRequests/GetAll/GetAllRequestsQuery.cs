using WebForum.Application.Abstractions.MediatR.Base;
using WebForum.Application.Responses;

namespace WebForum.Application.Features.RegistrationRequests.GetAll;

public class GetAllRequestsQuery : IQuery<IEnumerable<RegistrationResponse>>
{
    public RequestFlag Type => RequestFlag.Query;
}