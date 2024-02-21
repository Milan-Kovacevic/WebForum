using WebForum.Api.Handlers;

namespace WebForum.Api.Configuration.Extensions;

public static class ConfigureExceptionHandling
{
    public static IServiceCollection AddGlobalExceptionHandler(this IServiceCollection services)
    {
        services.AddExceptionHandler<GlobalExceptionHandler>();
        services.AddProblemDetails();
        return services;
    }
}