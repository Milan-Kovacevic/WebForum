using System.Reflection;

namespace WebForum.Application.Extensions;

public static class ApplicationAssemblyReference
{
    public static Assembly Value => typeof(ApplicationAssemblyReference).Assembly;
}