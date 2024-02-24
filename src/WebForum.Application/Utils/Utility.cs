namespace WebForum.Application.Utils;

public static class Utility
{
    public static string ComputeHash(string text)
    {
        return BCrypt.Net.BCrypt.HashPassword(text);
    }
    
    public static bool ValidateHash(string text, string hashText)
    {
        return BCrypt.Net.BCrypt.Verify(text, hashText);
    }
}