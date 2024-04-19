namespace WebForum.Api.Configuration;

public static class Constants
{
    public static class RateLimiter
    {
        public const string PolicyName = "limiter";
        public static readonly TimeSpan Window = TimeSpan.FromSeconds(10);
        public const int PermitLimit = 100;
    }

    public static class Infrastructure
    {
        public const string EmailConfigurationSection = "Email";
        public const string OAuthGitHubConfigurationSection = "OAuth:GitHub";
        public const string OAuthGoogleConfigurationSection = "OAuth:Google"; 
        public const string OAuthFacebookConfigurationSection = "OAuth:Facebook"; 
        public const string JwtConfigurationSection = "Jwt";
    }

    public static class Persistence
    {
        public const string DatabaseConnectionString = "DefaultConnection";
        public const string RedisConnectionString = "Redis";
        public const string RootAdminDataSection = "RootAdminData";
    }

    public static class Cors
    {
        public const string ClientConfigurationSection = "ApplicationServer";
        public const string AllowWebClientPolicyName = "WebForumClientPolicy";
    }
}