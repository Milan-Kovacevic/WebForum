using FluentValidation;
using WebForum.Application;
using WebForum.Application.Abstractions.Services;
using WebForum.Application.Services;

namespace WebForum.Api.Configuration.Extensions;

public static class ConfigureApplication
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(config => config.RegisterServicesFromAssembly(ApplicationAssemblyReference.Value));
        services.AddValidatorsFromAssembly(ApplicationAssemblyReference.Value);
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<IEmailService, EmailService>();
        return services;
    }
}