using WebForum.Application.Abstractions.MediatR.Base;
using WebForum.Application.Abstractions.Repositories;
using WebForum.Application.Responses;
using WebForum.Domain.Entities;
using WebForum.Domain.Enums;
using WebForum.Domain.Shared.Errors;
using WebForum.Domain.Shared.Results;

namespace WebForum.Application.Features.Comments.Create;

public class CreateCommentCommandHandler(
    ICommentRepository commentRepository,
    IUserRepository userRepository,
    IRoomRepository roomRepository) : ICommandHandler<CreateCommentCommand, CommentResponse>
{
    public async Task<Result<CommentResponse>> Handle(CreateCommentCommand request, CancellationToken cancellationToken)
    {
        var room = await roomRepository.GetByIdAsync(request.RoomId, cancellationToken);
        if (room is null)
            return Result.Failure<CommentResponse>(DomainErrors.Room.NotFound(request.RoomId));
        var user = await userRepository.GetByIdAsync(request.UserId, cancellationToken);
        if (user is null)
            return Result.Failure<CommentResponse>(DomainErrors.User.NotFound(request.UserId));

        var comment = new Comment()
        {
            Content = request.Content,
            Status = CommentStatus.Created,
            DateCreated = DateTime.UtcNow,
            User = user,
            UserId = user.UserId,
            RoomId = room.RoomId
        };
        await commentRepository.InsertAsync(comment, cancellationToken);
        return Result.Success(new CommentResponse(comment.CommentId, comment.Content, comment.DateCreated,
            comment.DateUpdated, comment.DatePosted, comment.User.DisplayName, comment.User.Role!.Name));
    }
}