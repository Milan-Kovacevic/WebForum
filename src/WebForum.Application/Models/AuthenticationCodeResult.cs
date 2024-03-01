namespace WebForum.Application.Models;

public class AuthenticationCodeResult
{
    public required string Code { get; set; }
    public required int Size { get; set; }
}