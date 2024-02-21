using MediatR;
using WebForum.Application.Abstractions.Repositories;
using WebForum.Application.Features.Topics.Queries;
using WebForum.Application.Features.Topics.Responses;

namespace WebForum.Application.Features.Topics.Handlers;

public class GetAllTopicsQueryHandler(ITopicRepository topicRepository)
    : IRequestHandler<GetAllTopicsQuery, IEnumerable<TopicResponse>>
{
    public async Task<IEnumerable<TopicResponse>> Handle(GetAllTopicsQuery request, CancellationToken cancellationToken)
    {
        var topics = await topicRepository.GetAllAsync(cancellationToken);
        return topics.Select(x => new TopicResponse()
            { TopicId = x.TopicId, Name = x.Name, Description = x.Description });
    }
}