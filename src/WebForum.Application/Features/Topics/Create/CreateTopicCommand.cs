using WebForum.Application.Abstractions.MediatR.Base;
using WebForum.Application.Features.Topics.Responses;

namespace WebForum.Application.Features.Topics.Create;

public record CreateTopicCommand(string Name, string? Description) : ICommand<TopicResponse>
{
    public RequestFlag Type => RequestFlag.Command | RequestFlag.Transaction | RequestFlag.Validate;
}