using System.ComponentModel.DataAnnotations;

namespace WebForum.Infrastructure.Authentication.OAuth;

public class OAuthOptions
{
    [Required] public required string ClientId { get; init; }
    [Required] public required string ClientSecret { get; init; }
    [Required, Url] public required string CallbackPath { get; init; }
    [Required, Url] public required string TokenEndpoint { get; init; }
    [Required, Url] public required string UserInformationEndpoint { get; init; }
}