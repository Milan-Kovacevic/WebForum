using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebForum.Api.Requests;
using WebForum.Application.Features.Topics.Create;
using WebForum.Application.Features.Topics.Delete;
using WebForum.Application.Features.Topics.GetAll;
using WebForum.Application.Features.Topics.GetById;
using WebForum.Application.Features.Topics.Update;
using WebForum.Domain.Models.Results;

namespace WebForum.Api.Controllers;

[Route("/api/[controller]")]
public class TopicsController(ISender sender) : ApiController(sender)
{
    [HttpGet]
    public async Task<IActionResult> GetAllTopics(CancellationToken cancellationToken)
    {
        var query = new GetAllTopicsQuery();
        var result = await Sender.Send(query, cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : HandleFailure(result);
    }

    [HttpGet("{topicId:guid}")]
    public async Task<IActionResult> GetTopicById(Guid topicId, CancellationToken cancellationToken)
    {
        var query = new GetTopicByIdQuery(topicId);
        var result = await Sender.Send(query, cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : HandleFailure(result);
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> AddTopic([FromBody] TopicRequest request, CancellationToken cancellationToken)
    {
        Result.Create(new CreateTopicCommand(request.Name, request.Description));
        
        var command = new CreateTopicCommand(request.Name, request.Description);
        var result = await Sender.Send(command, cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : HandleFailure(result);
    }

    [HttpPut("{topicId:guid}")]
    [Authorize]
    public async Task<IActionResult> UpdateTopic(Guid topicId, [FromBody] TopicRequest request,
        CancellationToken cancellationToken)
    {
        var command = new UpdateTopicCommand(topicId, request.Name, request.Description);
        var result = await Sender.Send(command, cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : HandleFailure(result);
    }

    [HttpDelete("{topicId:guid}")]
    [Authorize]
    public async Task<IActionResult> DeleteTopic(Guid topicId, CancellationToken cancellationToken)
    {
        var command = new DeleteTopicCommand(topicId);
        var result = await Sender.Send(command, cancellationToken);
        return result.IsSuccess ? NoContent() : HandleFailure(result);
    }
}