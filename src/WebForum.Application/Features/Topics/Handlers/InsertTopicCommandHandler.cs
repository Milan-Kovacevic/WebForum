using MediatR;
using WebForum.Application.Abstractions.Repositories;
using WebForum.Application.Features.Topics.Commands;
using WebForum.Application.Features.Topics.Responses;
using WebForum.Domain.Entities;

namespace WebForum.Application.Features.Topics.Handlers;

public class InsertTopicCommandHandler(ITopicRepository topicRepository, IUnitOfWork unitOfWork)
    : IRequestHandler<InsertTopicCommand, TopicResponse>
{
    public async Task<TopicResponse> Handle(InsertTopicCommand request, CancellationToken cancellationToken)
    {
        var topic = new Topic()
        {
            Name = request.Name,
            Description = request.Description
        };
        await topicRepository.InsertAsync(topic, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return new TopicResponse()
        {
            TopicId = topic.TopicId,
            Name = topic.Name,
            Description = topic.Description
        };
    }
}