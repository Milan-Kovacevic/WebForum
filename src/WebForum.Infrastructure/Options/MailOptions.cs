using System.ComponentModel.DataAnnotations;

namespace WebForum.Infrastructure.Settings;

public class MailOptions
{
    [Required]
    public required string SmtpHost { get; init; }
    [Required]
    public required int SmtpPort { get; init; }
    [Required]
    public required string Username { get; init; }
    [Required]
    public required string Secret { get; init; }
}