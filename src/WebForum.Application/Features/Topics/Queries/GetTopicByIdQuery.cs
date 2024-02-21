using WebForum.Application.Abstractions.Messaging.MediatR;
using WebForum.Application.Features.Topics.Responses;

namespace WebForum.Application.Features.Topics.Queries;

public record GetTopicByIdQuery(Guid TopicId) : IQuery<TopicResponse>
{
    public RequestFlag Type { get; } = RequestFlag.Query;
}