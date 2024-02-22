using System.ComponentModel.DataAnnotations;

namespace WebForum.Infrastructure.Settings;

public class GitHubSettings
{
    [Required]
    public required string AccessToken { get; init; }
    [Required]
    public required string UserAgent { get; init; }
    [Required, Url]
    public required string BaseAddress { get; init; }
}