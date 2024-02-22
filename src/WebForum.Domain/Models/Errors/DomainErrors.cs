using System.Net;

namespace WebForum.Domain.Models.Errors;

public static class DomainErrors
{
    public static class Topic
    {
        public static Error NotFound(Guid id) => new($"Topic.NotFound", $"The topic with Id {id} was not found.",
            (int)HttpStatusCode.NotFound);

        public static Error ConflictName(string name) => new($"Topic.ConflictName",
            $"The topic with name {name} is already created.",
            (int)HttpStatusCode.Conflict);
    }
}