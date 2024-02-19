namespace WebForum.Api.Configuration;

public static class Constants
{
    public static class RateLimiter
    {
        public const string PolicyName = "limiter";
        public static readonly TimeSpan Window = TimeSpan.FromSeconds(10);
        public const int PermitLimit = 2;
    }
    
}