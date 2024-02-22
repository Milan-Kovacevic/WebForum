
using WebForum.Application.Abstractions.MediatR.Base;
using WebForum.Application.Abstractions.Repositories;
using WebForum.Application.Features.Topics.Responses;
using WebForum.Domain.Models.Errors;
using WebForum.Domain.Models.Results;

namespace WebForum.Application.Features.Topics.GetById;

public class GetTopicByIdQueryHandler(ITopicRepository topicRepository)
    : IQueryHandler<GetTopicByIdQuery, TopicResponse>
{
    public async Task<Result<TopicResponse>> Handle(GetTopicByIdQuery request, CancellationToken cancellationToken)
    {
        var topic = await topicRepository.GetByIdAsync(request.TopicId, cancellationToken);
        return topic is null
            ? Result.Failure<TopicResponse>(DomainErrors.Topic.NotFound(request.TopicId))
            : Result.Success(new TopicResponse(topic.TopicId, topic.Name, topic.Description));
    }
}