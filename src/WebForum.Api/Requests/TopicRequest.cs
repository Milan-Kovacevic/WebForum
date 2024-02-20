namespace WebForum.Api.Requests;

public class TopicRequest
{
    public required string Name { get; set; }
    public string? Description { get; set; }
}