using WebForum.Application.Abstractions.MediatR.Base;
using WebForum.Application.Abstractions.Repositories;
using WebForum.Application.Features.RegistrationRequests.Responses;
using WebForum.Domain.Shared.Results;

namespace WebForum.Application.Features.RegistrationRequests.GetAll;

public class GetAllRequestsQueryHandler(IRegistrationRequestRepository registrationRequestRepository)
    : IQueryHandler<GetAllRequestsQuery, IEnumerable<RegistrationResponse>>
{
    public async Task<Result<IEnumerable<RegistrationResponse>>> Handle(GetAllRequestsQuery request,
        CancellationToken cancellationToken)
    {
        var registrationRequests = await registrationRequestRepository.GetAllAsync(cancellationToken);
        var result = registrationRequests.Select(x =>
            new RegistrationResponse(x.RequestId, x.SubmitDate, x.User.Username!, x.User.DisplayName));
        return Result.Success(result);
    }
}