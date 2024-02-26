using WebForum.Domain.Entities;
using WebForum.Domain.Enums;

namespace WebForum.Persistence.Configuration;

public static class Database
{
    public static class Tables
    {
        public const string RegistrationRequest = "registration_request";
        public const string User = "user";
        public const string UserLogin = "user_login";
        public const string Room = "room";
        public const string Comment = "comment";
        public const string Permission = "permission";
        public const string UserPermission = "user_permission";
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
        public const UserRole UserRole = Domain.Enums.UserRole.Regular;
        public const bool IsUserEnabled = false;
        public const int UserAccessFailedCount = 0;
    }
}