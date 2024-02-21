using MediatR;
using WebForum.Application.Abstractions.Messaging.MediatR;
using WebForum.Application.Features.Topics.Responses;

namespace WebForum.Application.Features.Topics.Queries;

public class GetAllTopicsQuery : IQuery<IEnumerable<TopicResponse>>
{
    public RequestFlag Type => RequestFlag.Query;
}