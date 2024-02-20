namespace WebForum.Domain.Models;

public class SimpleEmail
{
    public required string SendTo { get; init; }
    public required string Subject { get; init; }
    public required string Body { get; init; }
}