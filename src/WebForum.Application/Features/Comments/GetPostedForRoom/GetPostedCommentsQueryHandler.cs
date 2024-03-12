using WebForum.Application.Abstractions.MediatR.Base;
using WebForum.Application.Abstractions.Repositories;
using WebForum.Application.Responses;
using WebForum.Application.Utils;
using WebForum.Domain.Shared.Errors;
using WebForum.Domain.Shared.Results;

namespace WebForum.Application.Features.Comments.GetPostedForRoom;

public class GetPostedCommentsQueryHandler(ICommentRepository commentRepository, IRoomRepository roomRepository)
    : IQueryHandler<GetPostedCommentsQuery, IEnumerable<CommentResponse>>
{
    public async Task<Result<IEnumerable<CommentResponse>>> Handle(GetPostedCommentsQuery request,
        CancellationToken cancellationToken)
    {

        if (!await roomRepository.ExistsByIdAsync(request.RoomId, cancellationToken))
            return Result.Failure<IEnumerable<CommentResponse>>(DomainErrors.Room.NotFound(request.RoomId));

        var postedComments = await commentRepository.GetPostedRoomCommentsForUserAsync(request.RoomId, request.UserId,
            Constants.MaximumNumberOfRecentPostedComments, cancellationToken);
        var result = postedComments.Select(x => new CommentResponse(x.CommentId, x.Content, x.DateCreated,
            x.DateUpdated, x.DatePosted, x.User.UserId, x.User.DisplayName, x.User.Role!.RoleId));
        return Result.Success(result);
    }
}