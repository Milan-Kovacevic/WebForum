using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using WebForum.Application.Abstractions;
using WebForum.Application.Services;

namespace WebForum.Application.Extensions;

public static class DependencyInjection
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