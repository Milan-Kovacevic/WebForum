using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebForum.Api.Configuration.Extensions;
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
public class CommentsController(ISender sender, IAuthorizationService authorizationService) : ApiController(sender)
{
    [HttpGet("Rooms/{roomId:guid}/[controller]/Posted")]
    //[HasRole(UserRole.RootAdmin, UserRole.Admin, UserRole.Moderator, UserRole.Regular)]
    [AllowAnonymous]
    public async Task<IActionResult> GetPostedComments(Guid roomId, CancellationToken cancellationToken)
    {
        //var authorizationResult = await authorizationService.AuthorizeAsync(User, roomId, "");
//
        //return authorizationResult.Succeeded switch
        //{
        //    false when User.Identity!.IsAuthenticated => Forbid(),
        //    false when !User.Identity!.IsAuthenticated => Challenge(),
        //    _ => await Result
        //        .CreateFrom(new GetPostedRoomCommentsQuery(roomId))
        //        .Process(query => Sender.Send(query, cancellationToken))
        //        .Respond(Ok, HandleFailure)
        //};
        return await Result
            .CreateFrom(new GetPostedCommentsQuery(roomId))
            .Process(query => Sender.Send(query, cancellationToken))
            .Respond(Ok, HandleFailure);
    }

    [HttpGet("Rooms/{roomId:guid}/[controller]/Created")]
    //[HasRole(UserRole.RootAdmin, UserRole.Admin, UserRole.Moderator)]
    [AllowAnonymous]
    public async Task<IActionResult> GetAllCreatedComments(Guid roomId, CancellationToken cancellationToken)
    {
        return await Result
            .CreateFrom(new GetAllCreatedCommentsQuery(roomId))
            .Process(query => Sender.Send(query, cancellationToken))
            .Respond(Ok, HandleFailure);
    }

    [HttpPost("[controller]")]
    [AllowAnonymous]
    //[HasRoomPermission(RoomPermission.CreateComment)]
    public async Task<IActionResult> CreateComment([FromBody] CreateCommentRequest request,
        CancellationToken cancellationToken)
    {
        return await Result
            .CreateFrom(new CreateCommentCommand(request.RoomId, request.UserId, request.Content))
            .Process(command => Sender.Send(command, cancellationToken))
            .Respond(Ok, HandleFailure);
    }

    [HttpPut("[controller]/{commentId:guid}")]
    //[HasRoomPermission(RoomPermission.EditComment)]
    [AllowAnonymous]
    public async Task<IActionResult> EditComment([FromRoute] Guid commentId, [FromBody] EditCommentRequest request,
        CancellationToken cancellationToken)
    {
        return await Result
            .CreateFrom(new EditCommentCommand(commentId, request.NewContent))
            .Process(command => Sender.Send(command, cancellationToken))
            .Respond(Ok, HandleFailure);
    }

    [HttpDelete("[controller]/{commentId:guid}")]
    //[HasRoomPermission(RoomPermission.RemoveComment)]
    [AllowAnonymous]
    public async Task<IActionResult> RemoveComment([FromRoute] Guid commentId, CancellationToken cancellationToken)
    {
        return await Result
            .CreateFrom(new RemoveCommentCommand(commentId))
            .Process(command => Sender.Send(command, cancellationToken))
            .Respond(NoContent, HandleFailure);
    }

    [HttpPost("[controller]/{commentId:guid}/Post")]
    //[HasRoomPermission(RoomPermission.PostComment)]
    [AllowAnonymous]
    public async Task<IActionResult> PostComment([FromRoute] Guid commentId, [FromBody] PostCommentRequest request,
        CancellationToken cancellationToken)
    {
        return await Result
            .CreateFrom(new PostCommentCommand(commentId, request.UpdatedContent))
            .Process(command => Sender.Send(command, cancellationToken))
            .Respond(Ok, HandleFailure);
    }

    [HttpPost("[controller]/{commentId:guid}/Block")]
    //[HasRoomPermission(RoomPermission.BlockComment)]
    [AllowAnonymous]
    public async Task<IActionResult> BlockComment([FromRoute] Guid commentId, CancellationToken cancellationToken)
    {
        return await Result
            .CreateFrom(new BlockCommentCommand(commentId))
            .Process(command => Sender.Send(command, cancellationToken))
            .Respond(NoContent, HandleFailure);
    }
}