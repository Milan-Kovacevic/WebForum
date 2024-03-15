using WebForum.Application.Abstractions.MediatR.Base;
using WebForum.Application.Abstractions.Repositories;
using WebForum.Application.Responses;
using WebForum.Domain.Enums;
using WebForum.Domain.Shared.Errors;
using WebForum.Domain.Shared.Results;

namespace WebForum.Application.Features.Comments.GetAllPendingForRoom;

public class GetAllPendingCommentsQueryHandler(ICommentRepository commentRepository, IRoomRepository roomRepository)
    : IQueryHandler<GetAllPendingCommentsQuery, IEnumerable<CommentResponse>>
{
    public async Task<Result<IEnumerable<CommentResponse>>> Handle(GetAllPendingCommentsQuery request,
        CancellationToken cancellationToken)
    {
        if (!await roomRepository.ExistsByIdAsync(request.RoomId, cancellationToken))
            return Result.Failure<IEnumerable<CommentResponse>>(DomainErrors.Room.NotFound(request.RoomId));

        var createdComments =
            await commentRepository.GetRoomCommentsByStatusAsync(request.RoomId, CommentStatus.Created,
                cancellationToken);
        var result = createdComments.Select(x => new CommentResponse(x.CommentId, x.Content, x.DateCreated,
            x.DateUpdated, x.DatePosted, x.User.UserId, x.User.DisplayName, x.User.Role!.RoleId));
        return Result.Success(result);
    }
}