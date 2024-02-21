using MediatR;
using WebForum.Application.Features.Topics.Responses;

namespace WebForum.Application.Features.Topics.Commands;

public class InsertTopicCommand : IRequest<TopicResponse>
{
    public required string Name { get; init; }
    public string? Description { get; init; }
}