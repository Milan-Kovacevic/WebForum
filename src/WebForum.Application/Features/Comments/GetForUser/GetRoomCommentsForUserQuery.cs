using WebForum.Application.Abstractions.MediatR.Base;
using WebForum.Application.Responses;

namespace WebForum.Application.Features.Comments.GetForUser;

public record GetRoomCommentsForUserQuery(Guid RoomId, Guid UserId): IQuery<IEnumerable<CommentResponse>>
{
    public RequestFlag Type => RequestFlag.Query;
}