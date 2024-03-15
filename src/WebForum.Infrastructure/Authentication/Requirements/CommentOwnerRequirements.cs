using WebForum.Domain.Entities;

namespace WebForum.Infrastructure.Authentication.Requirements;

public static class CommentOwnerRequirements
{
    public static readonly CommentOwnerRequirement RegularUserOwner =
        new([UserRole.Regular], [UserRole.Moderator, UserRole.Admin, UserRole.RootAdmin]);
}