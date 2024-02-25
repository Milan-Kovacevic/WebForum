using FluentValidation;
using Microsoft.EntityFrameworkCore;
using WebForum.Application;
using WebForum.Application.Abstractions.Messaging;
using WebForum.Application.Abstractions.Providers;
using WebForum.Application.Abstractions.Repositories;
using WebForum.Application.Abstractions.Services;
using WebForum.Application.PipelineBehaviors;
using WebForum.Application.Services;
using WebForum.Infrastructure.Messaging;
using WebForum.Infrastructure.Options;
using WebForum.Infrastructure.Providers;
using WebForum.Infrastructure.Settings;
using WebForum.Persistence.Configuration;
using WebForum.Persistence.DbContext;
using WebForum.Persistence.Repositories;

namespace WebForum.Api.Configuration.Extensions;

public static class ModulesExtensions
{
    public static IHostApplicationBuilder AddModules(this IHostApplicationBuilder builder)
    {
        builder.Services.AddControllers();
        builder.Services.AddApplication();
        builder.Services.AddPersistence(builder.Configuration);
        builder.Services.AddInfrastructure();
        return builder;
    }

    private static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssembly(ApplicationAssemblyReference.Value);
            config.AddOpenBehavior(typeof(LoggingPipelineBehavior<,>));
            config.AddOpenBehavior(typeof(ValidationPipelineBehavior<,>));
            config.AddOpenBehavior(typeof(TransactionPipelineBehavior<,>));
        });
        services.AddValidatorsFromAssembly(ApplicationAssemblyReference.Value);
        services.AddScoped<IEmailService, EmailService>();
        return services;
    }

    private static IServiceCollection AddPersistence(this IServiceCollection services,
        IConfigurationManager configuration)
    {
        services.AddDbContext<ApplicationDbContext>(
            options =>
            {
                options.UseMySQL(configuration.GetConnectionString(Constants.Persistence.DatabaseConnectionString)!,
                    o => o.MigrationsHistoryTable(Database.Tables.MigrationHistory));
            });
        services.AddStackExchangeRedisCache(options =>
        {
            var connection = configuration.GetConnectionString(Constants.Persistence.RedisConnectionString)!;
            options.Configuration = connection;
        });
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<ITopicRepository, TopicRepository>();
        services.AddScoped<IUserTokenRepository, UserTokenRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IRegistrationRequestRepository, RegistrationRequestRepository>();
        return services;
    }

    private static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddOptions<MailOptions>().BindConfiguration(Constants.Infrastructure.EmailConfigurationSection)
            .ValidateDataAnnotations().ValidateOnStart();
        services.AddOptions<GitHubOptions>().BindConfiguration(Constants.Infrastructure.OAuthGitHubConfigurationSection)
            .ValidateDataAnnotations().ValidateOnStart();
        services.AddOptions<JwtOptions>().BindConfiguration(Constants.Infrastructure.JwtConfigurationSection)
            .ValidateDataAnnotations().ValidateOnStart();

        services.AddHttpClient<IGitHubClient, GitHubClient>();
        services.AddScoped<IMailSender, MailSender>();
        services.AddScoped<IJwtProvider, JwtProvider>();
        services.AddScoped<ITwoFactorCodeProvider, TwoFactorCodeProvider>();
        return services;
    }
}