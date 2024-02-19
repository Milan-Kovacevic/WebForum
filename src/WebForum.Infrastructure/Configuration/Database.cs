namespace WebForum.Infrastructure.Configuration;

public static class Database
{
    public static class Tables
    {
        public const string Topic = "topic";
        public const string Post = "post";
        public const string User = "user";
        public const string MigrationHistory = "ef_migrations";
    }

    public static class Constants
    {
        public const int MaxNameLength = 128;
        public const int MaxLongTextLength = 512;
        public const int MinPasswordLength = 4;
    }
}