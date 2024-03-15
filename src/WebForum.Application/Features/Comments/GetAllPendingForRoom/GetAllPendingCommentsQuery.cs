using WebForum.Application.Abstractions.MediatR.Base;
using WebForum.Application.Responses;

namespace WebForum.Application.Features.Comments.GetAllPendingForRoom;

public record GetAllPendingCommentsQuery(Guid RoomId) : IQuery<IEnumerable<CommentResponse>>
{
    public RequestFlag Type => RequestFlag.Query;
}