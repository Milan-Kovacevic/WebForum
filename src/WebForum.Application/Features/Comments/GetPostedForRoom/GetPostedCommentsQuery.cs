using WebForum.Application.Abstractions.MediatR.Base;
using WebForum.Application.Responses;

namespace WebForum.Application.Features.Comments.GetPostedForRoom;

public record GetPostedCommentsQuery(Guid RoomId) : IQuery<IEnumerable<CommentResponse>>
{
    public RequestFlag Type => RequestFlag.Query;
}