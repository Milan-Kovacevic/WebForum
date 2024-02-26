using System.Reflection;

namespace WebForum.Application.Utils;

public static class ApplicationAssemblyReference
{
    public static Assembly Value => typeof(ApplicationAssemblyReference).Assembly;
}