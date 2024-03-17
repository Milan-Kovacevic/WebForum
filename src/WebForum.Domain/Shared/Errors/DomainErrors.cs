using System.Net;

namespace WebForum.Domain.Shared.Errors;

public static class DomainErrors
{
    public static class Room
    {
        public static Error NotFound(Guid id) => new($"Room.NotFound", $"The room with the Id {id} was not found.",
            (int)HttpStatusCode.NotFound);

        public static Error ConflictName(string name) => new($"Room.ConflictName",
            $"The room with the name {name} is already created.",
            (int)HttpStatusCode.Conflict);
    }

    public static class User
    {
        public static Error NotFound(Guid id) => new($"User.NotFound", $"The user with the Id {id} was not found.",
            (int)HttpStatusCode.NotFound);

        public static Error ConflictUsername(string username) => new($"User.ConflictUsername",
            $"The user with the username {username} is already registered.",
            (int)HttpStatusCode.Conflict);
    }

    public static class Auth
    {
        public static readonly Error InvalidExternalProvider = new($"Auth.InvalidExternalProvider",
            $"Unable to authenticate user with given provider.",
            (int)HttpStatusCode.NotFound);

        public static readonly Error InvalidLogin = new($"Auth.InvalidLogin", "Username or password is invalid.",
            (int)HttpStatusCode.NotFound);

        public static readonly Error TokenExpired = new($"Auth.TokenExpired", "Specified token is expired.",
            (int)HttpStatusCode.NotFound);

        public static readonly Error InvalidToken = new($"Auth.InvalidToken", "Specified token is invalid.",
            (int)HttpStatusCode.NotFound);
        public static Error OAuthInvalidLogin(string? message) => new($"Auth.OAuthInvalidLogin", $"{message ?? "Unknown error"}",
            (int)HttpStatusCode.NotFound);
    }

    public static class RegistrationRequest
    {
        public static Error NotFound(Guid id) => new("RegistrationRequest.NotFound",
            $"The registration request with the Id {id} was not found.",
            (int)HttpStatusCode.NotFound);
    }

    public static class Role
    {
        public static Error NotFound(int id) => new("Role.NotFound",
            $"The Role with the Id {id} was not found.",
            (int)HttpStatusCode.NotFound);
    }

    public static class Permission
    {
        public static Error NotFound(int id) => new("Permission.NotFound",
            $"The Permission with the Id {id} was not found.",
            (int)HttpStatusCode.NotFound);
        
        public static Error NotAvailable(int id) => new("Permission.NotAvailable",
            $"The Permission with the Id {id} is not available.",
            (int)HttpStatusCode.NotFound);
    }

    public static class UserPermission
    {
        public static Error NotFound(Guid userId, Guid roomId, int permissionId) => new("UserPermission.NotFound",
            $"The Permission with the Id {permissionId} was not found for user {userId} on room {roomId}.",
            (int)HttpStatusCode.NotFound);

        public static Error Conflict(Guid userId, Guid roomId, int permissionId) => new("UserPermission.Conflict",
            $"The Permission with the Id {permissionId} was already added for user {userId} on room {roomId}.",
            (int)HttpStatusCode.Conflict);
    }

    public static class Comment
    {
        public static Error NotFound(Guid id) => new("Comment.NotFound",
            $"The Comment with the Id {id} was not found.",
            (int)HttpStatusCode.NotFound);

        public static Error NotEditable(Guid id) => new("Comment.NotEditable",
            $"The Comment with the Id {id} can not be edited.",
            (int)HttpStatusCode.NotFound);

        public static Error InvalidState(Guid id) => new("Comment.InvalidState",
            $"The Comment with the Id {id} in not in invalid state.",
            (int)HttpStatusCode.NotFound);
    }
}