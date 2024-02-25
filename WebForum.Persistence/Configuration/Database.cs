namespace WebForum.Persistence.Configuration;

public static class Database
{
    public static class Tables
    {
        public const string Topic = "topic";
        public const string Post = "post";
        public const string User = "user";
        public const string UserLogin = "user_login";
        public const string RegistrationRequest = "registration_request";
        public const string MigrationHistory = "ef_migrations";
    }

    public static class Constants
    {
        public const int MaxNameLength = 128;
        public const int MaxEmailLength = 192;
        public const int MaxLongTextLength = 512;
    }
    
    public static class Defaults
    {
        public const bool IsUserEnabled = false;
        public const int UserAccessFailCount = 0;
    }
}