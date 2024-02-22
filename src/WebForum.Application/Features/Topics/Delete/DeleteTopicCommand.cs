using WebForum.Application.Abstractions.MediatR.Base;

namespace WebForum.Application.Features.Topics.Delete;

public record DeleteTopicCommand(Guid TopicId) : ICommand
{
    public RequestFlag Type =>
        RequestFlag.Command | RequestFlag.Transaction | RequestFlag.Validate | RequestFlag.Sensitive;
}