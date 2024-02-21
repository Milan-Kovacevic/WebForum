using MediatR;
using Microsoft.AspNetCore.Mvc;
using WebForum.Api.Requests;
using WebForum.Application.Features.Topics.Commands;
using WebForum.Application.Features.Topics.Queries;

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
    
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetTopicById(Guid id, CancellationToken cancellationToken)
    {
        var query = new GetTopicByIdQuery(id);
        var result = await Sender.Send(query, cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : HandleFailure(result);
    }

    [HttpPost]
    public async Task<IActionResult> AddTopic([FromBody] TopicRequest request)
    {
        var command = new InsertTopicCommand()
        {
            Name = request.Name,
            Description = request.Description
        };
        var result = await Sender.Send(command);
        return result.IsSuccess ? Ok(result.Value) : HandleFailure(result);
    }
}