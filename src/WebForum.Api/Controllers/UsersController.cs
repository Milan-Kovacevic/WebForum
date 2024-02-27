using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebForum.Api.Configuration.Extensions;
using WebForum.Application.Features.Users.ChangeRole;
using WebForum.Application.Features.Users.GetAll;
using WebForum.Application.Features.Users.GetById;
using WebForum.Application.Requests;
using WebForum.Domain.Shared.Results;

namespace WebForum.Api.Controllers;

[Route("/api/[controller]")]
public class UsersController(ISender sender) : ApiController(sender)
{
    // TODO: Role - Admin
    [AllowAnonymous]
    [HttpGet]
    public async Task<IActionResult> GetRegisteredUsers(CancellationToken cancellationToken)
    {
        return await Result
            .CreateFrom(new GetAllRegisteredUsersQuery())
            .Process(query => Sender.Send(query, cancellationToken))
            .Respond(Ok, HandleFailure);
    }

    [AllowAnonymous]
    [HttpGet("{userId:guid}")]
    public async Task<IActionResult> GetRegisteredUserById([FromRoute] Guid userId, CancellationToken cancellationToken)
    {
        return await Result
            .CreateFrom(new GetRegisteredUserByIdQuery(userId))
            .Process(query => Sender.Send(query, cancellationToken))
            .Respond(Ok, HandleFailure);
    }

    [AllowAnonymous]
    [HttpPut("{userId:guid}/Group")]
    public async Task<IActionResult> ChangeUserGroup([FromRoute] Guid userId, [FromBody] ChangeUserGroupRequest request,
        CancellationToken cancellationToken)
    {
        return await Result
            .CreateFrom(new ChangeUserRoleCommand(userId, request.Role))
            .Process(query => Sender.Send(query, cancellationToken))
            .Respond(NoContent, HandleFailure);
    }
}