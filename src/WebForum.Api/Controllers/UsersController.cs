using MediatR;
using Microsoft.AspNetCore.Mvc;
using WebForum.Api.Configuration.Extensions;
using WebForum.Application.Features.Users.ChangeAccount;
using WebForum.Application.Features.Users.GetAll;
using WebForum.Application.Features.Users.GetById;
using WebForum.Application.Requests;
using WebForum.Domain.Enums;
using WebForum.Domain.Shared.Results;
using WebForum.Infrastructure.Authentication.Attributes;

namespace WebForum.Api.Controllers;

[Route("/api/[controller]")]
public class UsersController(ISender sender) : ApiController(sender)
{
    [HttpGet]
    [HasRole(UserRole.RootAdmin, UserRole.Admin)]
    public async Task<IActionResult> GetRegisteredUsers(CancellationToken cancellationToken)
    {
        return await Result
            .CreateFrom(new GetAllRegisteredUsersQuery())
            .Process(query => Sender.Send(query, cancellationToken))
            .Respond(Ok, HandleFailure);
    }
    
    [HttpGet("{userId:guid}")]
    [HasRole(UserRole.RootAdmin, UserRole.Admin)]
    public async Task<IActionResult> GetRegisteredUserById([FromRoute] Guid userId, CancellationToken cancellationToken)
    {
        return await Result
            .CreateFrom(new GetRegisteredUserByIdQuery(userId))
            .Process(query => Sender.Send(query, cancellationToken))
            .Respond(Ok, HandleFailure);
    }

    [HttpPost("{userId:guid}/Change")]
    [HasRole(UserRole.RootAdmin, UserRole.Admin)]
    public async Task<IActionResult> ChangeUserAccount([FromRoute] Guid userId, [FromBody] ChangeUserAccountRequest request,
        CancellationToken cancellationToken)
    {
        return await Result
            .CreateFrom(new ChangeUserAccountCommand(userId, request.Role, request.IsEnabled))
            .Process(query => Sender.Send(query, cancellationToken))
            .Respond(NoContent, HandleFailure);
    }
}