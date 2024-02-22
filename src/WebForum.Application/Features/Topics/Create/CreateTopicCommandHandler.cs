using WebForum.Application.Abstractions.MediatR.Base;
using WebForum.Application.Abstractions.Repositories;
using WebForum.Application.Features.Topics.Responses;
using WebForum.Domain.Entities;
using WebForum.Domain.Models.Results;

namespace WebForum.Application.Features.Topics.Create;

public class CreateTopicCommandHandler(ITopicRepository topicRepository)
    : ICommandHandler<CreateTopicCommand, TopicResponse>
{
    public async Task<Result<TopicResponse>> Handle(CreateTopicCommand request, CancellationToken cancellationToken)
    {
        var topic = new Topic()
        {
            Name = request.Name,
            Description = request.Description
        };
        await topicRepository.InsertAsync(topic, cancellationToken);
        return Result.Success(new TopicResponse(topic.TopicId, topic.Name, topic.Description));
    }
}