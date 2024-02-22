using FluentValidation;
using WebForum.Application;
using WebForum.Application.Abstractions.Services;
using WebForum.Application.PipelineBehaviors;
using WebForum.Application.Services;

namespace WebForum.Api.Configuration.Extensions;

public static class ConfigureApplication
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssembly(ApplicationAssemblyReference.Value);
            config.AddOpenBehavior(typeof(LoggingPipelineBehavior<,>));
            config.AddOpenBehavior(typeof(ValidationPipelineBehavior<,>));
            config.AddOpenBehavior(typeof(TransactionPipelineBehavior<,>));
        });
        services.AddValidatorsFromAssembly(ApplicationAssemblyReference.Value);
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<IEmailService, EmailService>();
        return services;
    }
}