using WebForum.Application.Abstractions.MediatR.Base;
using WebForum.Application.Abstractions.Repositories;
using WebForum.Domain.Models.Errors;
using WebForum.Domain.Models.Results;

namespace WebForum.Application.Features.Topics.Delete;

public class DeleteTopicCommandHandler(ITopicRepository topicRepository) : ICommandHandler<DeleteTopicCommand>
{
    public async Task<Result> Handle(DeleteTopicCommand request, CancellationToken cancellationToken)
    {
        var topic = await topicRepository.GetByIdAsync(request.TopicId, cancellationToken);
        if (topic is null)
            return Result.Failure(DomainErrors.Topic.NotFound(request.TopicId));

        topicRepository.Delete(topic);
        return Result.Success();
    }
}