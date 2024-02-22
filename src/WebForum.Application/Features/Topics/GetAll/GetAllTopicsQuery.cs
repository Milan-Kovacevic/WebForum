using WebForum.Application.Abstractions.MediatR.Base;
using WebForum.Application.Features.Topics.Responses;

namespace WebForum.Application.Features.Topics.GetAll;

public record GetAllTopicsQuery : IQuery<IEnumerable<TopicResponse>>
{
    public RequestFlag Type => RequestFlag.Query;
}