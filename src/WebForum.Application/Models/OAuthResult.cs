using WebForum.Domain.Entities;
using WebForum.Domain.Enums;

namespace WebForum.Application.Models;

public class OAuthResult
{
    public OAuthUser? User { get; set; }
    public required bool HasError { get; init; }
    public string? ErrorMessage { get; init; }
}