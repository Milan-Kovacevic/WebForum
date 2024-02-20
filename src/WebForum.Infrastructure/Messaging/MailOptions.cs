namespace WebForum.Infrastructure.Messaging;

public class MailOptions
{
    public required string SmtpHost { get; init; }
    public required int SmtpPort { get; init; }
    public required string Username { get; init; }
    public required string Secret { get; init; }
}