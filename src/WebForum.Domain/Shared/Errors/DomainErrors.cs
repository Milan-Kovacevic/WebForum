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

        public static readonly Error InvalidLogin = new($"Auth.InvalidLogin", $"Username or password is invalid.",
            (int)HttpStatusCode.NotFound);
    }

    public static class RegistrationRequest
    {
        public static Error NotFound(Guid id) => new($"RegistrationRequest.NotFound",
            $"The registration request with the Id {id} was not found.",
            (int)HttpStatusCode.NotFound);
    }
}