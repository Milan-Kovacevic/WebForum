using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WebForum.Infrastructure.Settings;

public class JwtOptions
{
    [Required] public required string Issuer { get; init; }
    [Required] public required string Audience { get; init; }
    [Required, MinLength(128)] public required string SecretKey { get; init; }

    [Required, Description("Expiration time of jwt token in minutes")]
    public required int ExpirationTime { get; init; }
}