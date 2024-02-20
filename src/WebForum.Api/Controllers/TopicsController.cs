using MediatR;
using Microsoft.AspNetCore.Mvc;
using WebForum.Api.Requests;
using WebForum.Application.Features.Topics.Commands;
using WebForum.Application.Features.Topics.Queries;

namespace WebForum.Api.Controllers;

[ApiController]
[Route("/api/[controller]")]
public class TopicsController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAllTopics(CancellationToken cancellationToken)
    {
        var query = new GetAllTopicsQuery();
        var result = await mediator.Send(query, cancellationToken);
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> AddTopic([FromBody] TopicRequest request)
    {
        var command = new InsertTopicCommand()
        {
            Name = request.Name,
            Description = request.Description
        };
        var result = await mediator.Send(command);
        return Ok(result);
    }
}