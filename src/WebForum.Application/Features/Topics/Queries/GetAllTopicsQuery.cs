using MediatR;
using WebForum.Application.Features.Topics.Responses;

namespace WebForum.Application.Features.Topics.Queries;

public class GetAllTopicsQuery : IRequest<IEnumerable<TopicResponse>>
{
    
}