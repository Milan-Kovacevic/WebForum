using System.Net;

namespace WebForum.Domain.Shared.Errors;

public static class DomainErrors
{
    public static class Topic
    {
        public static Error NotFound(Guid id) => new($"Topic.NotFound", $"The topic with the Id {id} was not found.",
            (int)HttpStatusCode.NotFound);

        public static Error ConflictName(string name) => new($"Topic.ConflictName",
            $"The topic with the name {name} is already created.",
            (int)HttpStatusCode.Conflict);
    }

    public static class User
    {
        public static Error NotFound(Guid id) => new($"User.NotFound", $"The user with the Id {id} was not found.",
            (int)HttpStatusCode.NotFound);

        public static readonly Error InvalidLogin = new($"User.InvalidLogin", $"Username or password is invalid.",
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
    }
}