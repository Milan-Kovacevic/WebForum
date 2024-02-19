using MicroElements.Swashbuckle.FluentValidation.AspNetCore;

namespace WebForum.Api.Configuration.Extensions;

public static class ConfigureSwagger
{
    public static IServiceCollection AddSwagger(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        services.AddFluentValidationRulesToSwagger();
        return services;
    }
}