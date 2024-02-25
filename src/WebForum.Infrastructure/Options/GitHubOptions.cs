using System.ComponentModel.DataAnnotations;

namespace WebForum.Infrastructure.Options;

public class GitHubOptions
{
    [Required]
    public required string ClientId { get; init; }
    [Required]
    public required string ClientSecret { get; init; }
    [Required]
    public required string CallbackPath { get; init; }
}