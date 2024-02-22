using System.ComponentModel.DataAnnotations;

namespace WebForum.Infrastructure.Options;

public class GitHubOptions
{
    [Required]
    public required string AccessToken { get; init; }
    [Required]
    public required string UserAgent { get; init; }
    [Required, Url]
    public required string BaseAddress { get; init; }
}