using WebForum.Application.Abstractions.MediatR.Base;
using WebForum.Application.Abstractions.Repositories;
using WebForum.Application.Features.Topics.Responses;
using WebForum.Domain.Entities;
using WebForum.Domain.Shared.Errors;
using WebForum.Domain.Shared.Results;

namespace WebForum.Application.Features.Topics.Create;

public class CreateTopicCommandHandler(ITopicRepository topicRepository)
    : ICommandHandler<CreateTopicCommand, TopicResponse>
{
    public async Task<Result<TopicResponse>> Handle(CreateTopicCommand request, CancellationToken cancellationToken)
    {
        if (await topicRepository.ExistsByName(request.Name))
            return Result.Failure<TopicResponse>(DomainErrors.Topic.ConflictName(request.Name));

        var topic = new Topic()
        {
            Name = request.Name,
            Description = request.Description
        };
        await topicRepository.InsertAsync(topic, cancellationToken);
        return Result.Success(new TopicResponse(topic.TopicId, topic.Name, topic.Description));
    }
}