using WebForum.Application.Abstractions.MediatR.Base;
using WebForum.Application.Features.Topics.Responses;

namespace WebForum.Application.Features.Topics.GetById;

public record GetTopicByIdQuery(Guid TopicId) : IQuery<TopicResponse>
{
    public RequestFlag Type => RequestFlag.Query;
}