using System.Reflection;

namespace WebForum.Application.Extensions;

public static class AssemblyReference
{
    public static Assembly Value => typeof(AssemblyReference).Assembly;
}