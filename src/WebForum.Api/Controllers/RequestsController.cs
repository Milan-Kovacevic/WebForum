using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebForum.Api.Configuration.Extensions;
using WebForum.Application.Features.RegistrationRequests.GetAll;
using WebForum.Application.Features.RegistrationRequests.Process;
using WebForum.Domain.Shared.Results;

namespace WebForum.Api.Controllers;

[Route("/api/[controller]")]
public class RequestsController(ISender sender) : ApiController(sender)
{
    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetAllRegistrationRequests(CancellationToken cancellationToken)
    {
        return await Result
            .CreateFrom(new GetAllRequestsQuery())
            .Process(query => Sender.Send(query, cancellationToken))
            .Respond(Ok, HandleFailure);
    }

    [HttpPost("{requestId:guid}/approve")]
    [AllowAnonymous]
    public async Task<IActionResult> ApproveRegistrationRequest(Guid requestId, CancellationToken cancellationToken)
    {
        return await Result
            .CreateFrom(new ProcessRequestCommand(requestId, true))
            .Process(command => Sender.Send(command, cancellationToken))
            .Respond(Ok, HandleFailure);
    }

    [HttpPost("{requestId:guid}/reject")]
    [AllowAnonymous]
    public async Task<IActionResult> RejectRegistrationRequest(Guid requestId, CancellationToken cancellationToken)
    {
        return await Result
            .CreateFrom(new ProcessRequestCommand(requestId, false))
            .Process(command => Sender.Send(command, cancellationToken))
            .Respond(Ok, HandleFailure);
    }
}