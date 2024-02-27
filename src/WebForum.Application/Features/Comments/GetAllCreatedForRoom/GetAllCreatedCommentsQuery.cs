using WebForum.Application.Abstractions.MediatR.Base;
using WebForum.Application.Responses;

namespace WebForum.Application.Features.Comments.GetAllCreatedForRoom;

public record GetAllCreatedCommentsQuery(Guid RoomId) : IQuery<IEnumerable<CommentResponse>>
{
    public RequestFlag Type => RequestFlag.Query;
}