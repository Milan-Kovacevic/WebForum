using WebForum.Application.Abstractions.Messaging.MediatR;
using WebForum.Application.Features.Topics.Responses;

namespace WebForum.Application.Features.Topics.Commands;

public class InsertTopicCommand : ICommand<TopicResponse>
{
    public required string Name { get; init; }
    public string? Description { get; init; }
    
    public RequestFlag Type => RequestFlag.Command | RequestFlag.Transaction | RequestFlag.Validate;
}