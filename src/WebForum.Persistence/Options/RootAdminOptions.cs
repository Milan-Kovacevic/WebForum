using System.ComponentModel.DataAnnotations;

namespace WebForum.Persistence.Options;

public class RootAdminOptions
{
    [Required]
    public required string DisplayName { get; set; }
    [Required]
    public required string Username { get; set; }
    [Required]
    public required string Password { get; set; }
    [Required]
    public required string Email { get; set; }
}