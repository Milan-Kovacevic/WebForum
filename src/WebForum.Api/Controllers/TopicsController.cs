using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebForum.Api.Configuration.Extensions;
using WebForum.Application.Requests;
using WebForum.Application.Features.Topics.Create;
using WebForum.Application.Features.Topics.Delete;
using WebForum.Application.Features.Topics.GetAll;
using WebForum.Application.Features.Topics.GetById;
using WebForum.Application.Features.Topics.Update;
using WebForum.Domain.Enums;
using WebForum.Domain.Shared.Results;
using WebForum.Infrastructure.Authentication.Attributes;

namespace WebForum.Api.Controllers;

[Route("/api/[controller]")]
public class TopicsController(ISender sender) : ApiController(sender)
{
    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetAllTopics(CancellationToken cancellationToken)
    {
        return await Result
            .CreateFrom(new GetAllTopicsQuery())
            .Process(query => Sender.Send(query, cancellationToken))
            .Respond(Ok, HandleFailure);
    }

    [HttpGet("{topicId:guid}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetTopicById(Guid topicId, CancellationToken cancellationToken)
    {
        return await Result
            .CreateFrom(new GetTopicByIdQuery(topicId))
            .Process(query => Sender.Send(query, cancellationToken))
            .Respond(Ok, HandleFailure);
    }

    [HttpPost]
    [HasRole(UserRole.RootAdmin, UserRole.Admin)]
    [HasPermission(CommentPermission.BanComment, CommentPermission.PostComment, CommentPermission.CreateComment,
        CommentPermission.EditComment)]
    public async Task<IActionResult> AddTopic([FromBody] TopicRequest request, CancellationToken cancellationToken)
    {
        return await Result
            .CreateFrom(new CreateTopicCommand(request.Name, request.Description))
            .Process(command => Sender.Send(command, cancellationToken))
            .Respond(Ok, HandleFailure);
    }

    [HttpPut("{topicId:guid}")]
    [HasRole(UserRole.RootAdmin)]
    [HasPermission(CommentPermission.RemoveComment)]
    public async Task<IActionResult> UpdateTopic(Guid topicId, [FromBody] TopicRequest request,
        CancellationToken cancellationToken)
    {
        return await Result
            .CreateFrom(new UpdateTopicCommand(topicId, request.Name, request.Description))
            .Process(query => Sender.Send(query, cancellationToken))
            .Respond(Ok, HandleFailure);
    }

    [HttpDelete("{topicId:guid}")]
    [HasRole(UserRole.RootAdmin)]
    public async Task<IActionResult> DeleteTopic(Guid topicId, CancellationToken cancellationToken)
    {
        return await Result
            .CreateFrom(new DeleteTopicCommand(topicId))
            .Process(query => Sender.Send(query, cancellationToken))
            .Respond(NoContent, HandleFailure);
    }
}