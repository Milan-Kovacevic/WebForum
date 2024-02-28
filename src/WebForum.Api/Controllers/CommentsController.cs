using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebForum.Api.Configuration.Extensions;
using WebForum.Application.Abstractions.Services;
using WebForum.Application.Features.Comments.Block;
using WebForum.Application.Features.Comments.Create;
using WebForum.Application.Features.Comments.Edit;
using WebForum.Application.Features.Comments.GetAllCreatedForRoom;
using WebForum.Application.Features.Comments.GetPostedForRoom;
using WebForum.Application.Features.Comments.Post;
using WebForum.Application.Features.Comments.Remove;
using WebForum.Application.Requests;
using WebForum.Domain.Enums;
using WebForum.Domain.Shared.Results;
using WebForum.Infrastructure.Authentication.Attributes;

namespace WebForum.Api.Controllers;

[Route("/api")]
public class CommentsController(
    ISender sender,
    IAuthorizationService authorizationService,
    IResourceResolverService resourceResolverService) : ApiController(sender)
{
    [HttpGet("Rooms/{roomId:guid}/[controller]/Posted")]
    [HasRole(UserRole.RootAdmin, UserRole.Admin, UserRole.Moderator, UserRole.Regular)]
    public async Task<IActionResult> GetPostedComments(Guid roomId, CancellationToken cancellationToken)
    {
        return await Result
            .CreateFrom(new GetPostedCommentsQuery(roomId))
            .Process(query => Sender.Send(query, cancellationToken))
            .Respond(Ok, HandleFailure);
    }

    [HttpGet("Rooms/{roomId:guid}/[controller]/Created")]
    [HasRole(UserRole.RootAdmin, UserRole.Admin, UserRole.Moderator)]
    public async Task<IActionResult> GetAllCreatedComments(Guid roomId, CancellationToken cancellationToken)
    {
        return await Result
            .CreateFrom(new GetAllCreatedCommentsQuery(roomId))
            .Process(query => Sender.Send(query, cancellationToken))
            .Respond(Ok, HandleFailure);
    }

    [HttpPost("[controller]")]
    [HasRole(UserRole.RootAdmin, UserRole.Admin, UserRole.Moderator, UserRole.Regular)]
    public async Task<IActionResult> CreateComment([FromBody] CreateCommentRequest request,
        CancellationToken cancellationToken)
    {
        var authorizationResult =
            await authorizationService.AuthorizeAsync(User, request.RoomId, RoomPermissionRequirements.CreateComment);
        if (!authorizationResult.Succeeded)
            return Forbid();

        return await Result
            .CreateFrom(new CreateCommentCommand(request.RoomId, request.UserId, request.Content))
            .Process(command => Sender.Send(command, cancellationToken))
            .Respond(Ok, HandleFailure);
    }

    [HttpPut("[controller]/{commentId:guid}")]
    [HasRole(UserRole.RootAdmin, UserRole.Admin, UserRole.Moderator, UserRole.Regular)]
    public async Task<IActionResult> EditComment([FromRoute] Guid commentId, [FromBody] EditCommentRequest request,
        CancellationToken cancellationToken)
    {
        var roomId = await resourceResolverService.ResolveRoomIdByCommentIdAsync(commentId, cancellationToken);
        var authorizationResult =
            await authorizationService.AuthorizeAsync(User, roomId, RoomPermissionRequirements.EditComment);
        if (!authorizationResult.Succeeded)
            return Forbid();

        return await Result
            .CreateFrom(new EditCommentCommand(commentId, request.NewContent))
            .Process(command => Sender.Send(command, cancellationToken))
            .Respond(Ok, HandleFailure);
    }

    [HttpDelete("[controller]/{commentId:guid}")]
    [HasRole(UserRole.RootAdmin, UserRole.Admin, UserRole.Moderator, UserRole.Regular)]
    public async Task<IActionResult> RemoveComment([FromRoute] Guid commentId, CancellationToken cancellationToken)
    {
        var roomId = await resourceResolverService.ResolveRoomIdByCommentIdAsync(commentId, cancellationToken);
        var authorizationResult =
            await authorizationService.AuthorizeAsync(User, roomId, RoomPermissionRequirements.RemoveComment);
        if (!authorizationResult.Succeeded)
            return Forbid();

        return await Result
            .CreateFrom(new RemoveCommentCommand(commentId))
            .Process(command => Sender.Send(command, cancellationToken))
            .Respond(NoContent, HandleFailure);
    }

    [HttpPost("[controller]/{commentId:guid}/Post")]
    [HasRole(UserRole.RootAdmin, UserRole.Admin, UserRole.Moderator)]
    public async Task<IActionResult> PostComment([FromRoute] Guid commentId, [FromBody] PostCommentRequest request,
        CancellationToken cancellationToken)
    {
        var roomId = await resourceResolverService.ResolveRoomIdByCommentIdAsync(commentId, cancellationToken);
        var authorizationResult =
            await authorizationService.AuthorizeAsync(User, roomId, RoomPermissionRequirements.PostComment);
        if (!authorizationResult.Succeeded)
            return Forbid();

        return await Result
            .CreateFrom(new PostCommentCommand(commentId, request.UpdatedContent))
            .Process(command => Sender.Send(command, cancellationToken))
            .Respond(Ok, HandleFailure);
    }

    [HttpPost("[controller]/{commentId:guid}/Block")]
    [HasRole(UserRole.RootAdmin, UserRole.Admin, UserRole.Moderator)]
    public async Task<IActionResult> BlockComment([FromRoute] Guid commentId, CancellationToken cancellationToken)
    {
        var roomId = await resourceResolverService.ResolveRoomIdByCommentIdAsync(commentId, cancellationToken);
        var authorizationResult =
            await authorizationService.AuthorizeAsync(User, roomId, RoomPermissionRequirements.BlockComment);
        if (!authorizationResult.Succeeded)
            return Forbid();

        return await Result
            .CreateFrom(new BlockCommentCommand(commentId))
            .Process(command => Sender.Send(command, cancellationToken))
            .Respond(NoContent, HandleFailure);
    }
}