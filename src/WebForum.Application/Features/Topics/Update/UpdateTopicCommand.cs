using WebForum.Application.Abstractions.MediatR.Base;
using WebForum.Application.Features.Topics.Responses;

namespace WebForum.Application.Features.Topics.Update;

public record UpdateTopicCommand(Guid TopicId, string Name, string? Description) : ICommand<TopicResponse>
{
    public RequestFlag Type => RequestFlag.Command | RequestFlag.Transaction | RequestFlag.Validate;
}