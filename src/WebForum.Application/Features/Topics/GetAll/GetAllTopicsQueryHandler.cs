using WebForum.Application.Abstractions.MediatR.Base;
using WebForum.Application.Abstractions.Repositories;
using WebForum.Application.Features.Topics.Responses;
using WebForum.Domain.Shared.Results;

namespace WebForum.Application.Features.Topics.GetAll;

public class GetAllTopicsQueryHandler(ITopicRepository topicRepository)
    : IQueryHandler<GetAllTopicsQuery, IEnumerable<TopicResponse>>
{
    public async Task<Result<IEnumerable<TopicResponse>>> Handle(GetAllTopicsQuery request,
        CancellationToken cancellationToken)
    {
        var topics = await topicRepository.GetAllAsync(cancellationToken);
        return Result.Success(topics.Select(x => new TopicResponse(x.TopicId, x.Name, x.Description)));
    }
}