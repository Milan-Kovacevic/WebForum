using WebForum.Application.Abstractions.MediatR.Base;
using WebForum.Application.Abstractions.Repositories;
using WebForum.Application.Features.Topics.Responses;
using WebForum.Domain.Models.Errors;
using WebForum.Domain.Models.Results;

namespace WebForum.Application.Features.Topics.Update;

public class UpdateTopicCommandHandler(ITopicRepository topicRepository)
    : ICommandHandler<UpdateTopicCommand, TopicResponse>
{
    public async Task<Result<TopicResponse>> Handle(UpdateTopicCommand request, CancellationToken cancellationToken)
    {
        var topic = await topicRepository.GetByIdAsync(request.TopicId, cancellationToken);
        if (topic is null)
            return Result.Failure<TopicResponse>(DomainErrors.Topic.NotFound(request.TopicId));

        topic.Name = request.Name;
        topic.Description = request.Description;
        topicRepository.Update(topic);
        return Result.Success(new TopicResponse(topic.TopicId, topic.Name, topic.Description));
    }
}