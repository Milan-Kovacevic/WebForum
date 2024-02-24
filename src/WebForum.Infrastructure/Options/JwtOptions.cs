using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WebForum.Infrastructure.Options;

public class JwtOptions
{
    [Required] public required string Issuer { get; init; }
    [Required] public required string Audience { get; init; }
    [Required, MinLength(128)] public required string AccessTokenSigningKey { get; init; }
    [Required, MinLength(128)] public required string RefreshTokenSigningKey { get; init; }

    [Required, Description("Expiration time of jwt access token in minutes")]
    public required long AccessTokenExpirationTime { get; init; }

    [Required, Description("Expiration time of jwt refresh token in minutes")]
    public required long RefreshTokenExpirationTime { get; init; }
}