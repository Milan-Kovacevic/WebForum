namespace WebForum.Application.Utils;

public static class Constants
{
    public const int UserAuthenticationCodeSize = 6;
    public static readonly TimeSpan UserAuthenticationCodeExpirationTime = TimeSpan.FromMinutes(5);
    public const int DefaultLoginAccessFailCount = 0;
    public const int MaximumLoginAccessFailCount = 5;
    public static readonly TimeSpan UserLockoutPeriod = TimeSpan.FromMinutes(5);
    public const int MaximumNumberOfRecentPostedComments = 20;
    public const int MaximumCommentContentSize = 256;
}