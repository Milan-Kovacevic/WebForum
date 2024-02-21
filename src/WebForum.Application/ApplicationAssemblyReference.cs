using System.Reflection;

namespace WebForum.Application;

public static class ApplicationAssemblyReference
{
    public static Assembly Value => typeof(ApplicationAssemblyReference).Assembly;
}