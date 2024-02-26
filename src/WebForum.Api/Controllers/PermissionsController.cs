using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebForum.Api.Configuration.Extensions;
using WebForum.Application.Features.Permissions.GetAll;
using WebForum.Domain.Shared.Results;

namespace WebForum.Api.Controllers;

[Route("/api/[controller]")]
public class PermissionsController(ISender sender) : ApiController(sender)
{
    [AllowAnonymous]
    [HttpGet]
    public async Task<IActionResult> GetRoomPermissions(CancellationToken cancellationToken)
    {
        return await Result.CreateFrom(new GetAllPermissionsQuery())
            .Process(query => Sender.Send(query, cancellationToken))
            .Respond(Ok, HandleFailure);
    }
}