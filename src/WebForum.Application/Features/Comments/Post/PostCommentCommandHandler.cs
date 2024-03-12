using WebForum.Application.Abstractions.MediatR.Base;
using WebForum.Application.Abstractions.Repositories;
using WebForum.Application.Responses;
using WebForum.Domain.Enums;
using WebForum.Domain.Shared.Errors;
using WebForum.Domain.Shared.Results;

namespace WebForum.Application.Features.Comments.Post;

public class PostCommentCommandHandler(ICommentRepository commentRepository) : ICommandHandler<PostCommentCommand, CommentResponse>
{
    public async Task<Result<CommentResponse>> Handle(PostCommentCommand request, CancellationToken cancellationToken)
    {
        var comment = await commentRepository.GetByIdAsync(request.CommentId, cancellationToken);
        if (comment is null)
            return Result.Failure<CommentResponse>(DomainErrors.Comment.NotFound(request.CommentId));
        if (comment.Status != CommentStatus.Created)
            return Result.Failure<CommentResponse>(DomainErrors.Comment.InvalidState(request.CommentId));

        if (request.UpdatedContent is not null)
            comment.Content = request.UpdatedContent;
        comment.DatePosted = DateTime.UtcNow;
        comment.Status = CommentStatus.Posted;
        commentRepository.Update(comment);

        return Result.Success(new CommentResponse(comment.CommentId, comment.Content, comment.DateCreated,
            comment.DateUpdated, comment.DatePosted, comment.User.UserId, comment.User.DisplayName, comment.User.Role!.RoleId));
    }
}