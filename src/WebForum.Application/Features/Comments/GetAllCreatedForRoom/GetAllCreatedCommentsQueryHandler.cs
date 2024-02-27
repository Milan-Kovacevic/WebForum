using WebForum.Application.Abstractions.MediatR.Base;
using WebForum.Application.Abstractions.Repositories;
using WebForum.Application.Responses;
using WebForum.Domain.Enums;
using WebForum.Domain.Shared.Errors;
using WebForum.Domain.Shared.Results;

namespace WebForum.Application.Features.Comments.GetAllCreatedForRoom;

public class GetAllCreatedCommentsQueryHandler(ICommentRepository commentRepository, IRoomRepository roomRepository)
    : IQueryHandler<GetAllCreatedCommentsQuery, IEnumerable<CommentResponse>>
{
    public async Task<Result<IEnumerable<CommentResponse>>> Handle(GetAllCreatedCommentsQuery request,
        CancellationToken cancellationToken)
    {
        if (!await roomRepository.ExistsByIdAsync(request.RoomId, cancellationToken))
            return Result.Failure<IEnumerable<CommentResponse>>(DomainErrors.Room.NotFound(request.RoomId));

        var createdComments =
            await commentRepository.GetRoomCommentsByStatusAsync(request.RoomId, CommentStatus.Created,
                cancellationToken);
        var result = createdComments.Select(x => new CommentResponse(x.CommentId, x.Content, x.DateCreated,
            x.DateUpdated, x.DatePosted, x.User.DisplayName, x.User.Role!.Name));
        return Result.Success(result);
    }
}