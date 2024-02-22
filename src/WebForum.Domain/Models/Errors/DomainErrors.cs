using System.Net;

namespace WebForum.Domain.Models.Errors;

public static class DomainErrors
{
    public static class Topic
    {
        public static Error NotFound(Guid id) => new($"Topic.NotFound", $"The topic with Id {id} was not found.",
            (int)HttpStatusCode.NotFound);
    }
}