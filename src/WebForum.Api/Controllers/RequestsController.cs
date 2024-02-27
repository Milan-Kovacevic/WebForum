using MediatR;
using Microsoft.AspNetCore.Mvc;
using WebForum.Api.Configuration.Extensions;
using WebForum.Application.Features.RegistrationRequests.GetAll;
using WebForum.Application.Features.RegistrationRequests.Process;
using WebForum.Domain.Enums;
using WebForum.Domain.Shared.Results;
using WebForum.Infrastructure.Authentication.Attributes;

namespace WebForum.Api.Controllers;

[Route("/api/[controller]")]
public class RequestsController(ISender sender) : ApiController(sender)
{
    [HttpGet]
    [HasRole(UserRole.RootAdmin, UserRole.Admin)]
    public async Task<IActionResult> GetAllRegistrationRequests(CancellationToken cancellationToken)
    {
        return await Result
            .CreateFrom(new GetAllRequestsQuery())
            .Process(query => Sender.Send(query, cancellationToken))
            .Respond(Ok, HandleFailure);
    }

    [HttpPost("{requestId:guid}/Approve")]
    [HasRole(UserRole.RootAdmin, UserRole.Admin)]
    public async Task<IActionResult> ApproveRegistrationRequest(Guid requestId, CancellationToken cancellationToken)
    {
        return await Result
            .CreateFrom(new ProcessRequestCommand(requestId, true))
            .Process(command => Sender.Send(command, cancellationToken))
            .Respond(Ok, HandleFailure);
    }

    [HttpPost("{requestId:guid}/Reject")]
    [HasRole(UserRole.RootAdmin, UserRole.Admin)]
    public async Task<IActionResult> RejectRegistrationRequest(Guid requestId, CancellationToken cancellationToken)
    {
        return await Result
            .CreateFrom(new ProcessRequestCommand(requestId, false))
            .Process(command => Sender.Send(command, cancellationToken))
            .Respond(Ok, HandleFailure);
    }
}