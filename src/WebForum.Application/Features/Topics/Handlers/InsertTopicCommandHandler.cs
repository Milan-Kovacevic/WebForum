using WebForum.Application.Abstractions.Messaging.MediatR;
using WebForum.Application.Abstractions.Repositories;
using WebForum.Application.Features.Topics.Commands;
using WebForum.Application.Features.Topics.Responses;
using WebForum.Domain.Entities;
using WebForum.Domain.Models.Results;

namespace WebForum.Application.Features.Topics.Handlers;

public class InsertTopicCommandHandler(ITopicRepository topicRepository)
    : ICommandHandler<InsertTopicCommand, TopicResponse>
{
    public async Task<Result<TopicResponse>> Handle(InsertTopicCommand request, CancellationToken cancellationToken)
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