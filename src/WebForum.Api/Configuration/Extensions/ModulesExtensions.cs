using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using WebForum.Application.Utils;
using WebForum.Application.Abstractions.Repositories;
using WebForum.Application.Abstractions.Services;
using WebForum.Application.PipelineBehaviors;
using WebForum.Infrastructure.Authentication.Handlers;
using WebForum.Infrastructure.Authentication.OAuth;
using WebForum.Infrastructure.Authentication.OAuth.Handlers.GitHub;
using WebForum.Infrastructure.Authentication.OAuth.Handlers.Google;
using WebForum.Infrastructure.Authentication.Providers;
using WebForum.Infrastructure.Options;
using WebForum.Infrastructure.Services;
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
        IConfiguration configuration)
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
        services.AddScoped<IRoomRepository, RoomRepository>();
        services.AddScoped<IUserTokenRepository, UserTokenRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IUserLoginRepository, UserLoginRepository>();
        services.AddScoped<IRegistrationRequestRepository, RegistrationRequestRepository>();
        services.AddScoped<IRoleRepository, RoleRepository>();
        services.AddScoped<IPermissionRepository, PermissionRepository>();
        services.AddScoped<IUserPermissionRepository, UserPermissionRepository>();
        services.AddScoped<ICommentRepository, CommentRepository>();
        return services;
    }

    private static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddOptions<MailOptions>().BindConfiguration(Constants.Infrastructure.EmailConfigurationSection)
            .ValidateDataAnnotations().ValidateOnStart();
        services.AddOptions<GitHubOptions>().BindConfiguration(Constants.Infrastructure.OAuthGitHubConfigurationSection)
            .ValidateDataAnnotations().ValidateOnStart();
        services.AddOptions<GoogleOptions>().BindConfiguration(Constants.Infrastructure.OAuthGoogleConfigurationSection)
            .ValidateDataAnnotations().ValidateOnStart();
        services.AddOptions<JwtOptions>().BindConfiguration(Constants.Infrastructure.JwtConfigurationSection)
            .ValidateDataAnnotations().ValidateOnStart();
        services.AddHttpClient();
        services.AddSingleton<IAuthorizationHandler, RoleAuthorizationHandler>();
        services.AddSingleton<IAuthorizationHandler, RoomPermissionAuthorizationHandler>();
        services.AddSingleton<IAuthorizationHandler, CommentOwnerAuthorizationHandler>();
        services.AddSingleton<IAuthorizationPolicyProvider, RoomPermissionAuthorizationPolicyProvider>();
        services.AddSingleton<TokenHandler, AuthenticationJwtHandler>();

        services.AddScoped<IGitHubOAuthHandler, GitHubOAuthHandler>();
        services.AddScoped<IGoogleOAuthHandler, GoogleOAuthHandler>();
        services.AddScoped<IRedisCacheService, RedisCacheService>();
        services.AddScoped<IEmailService, EmailService>();
        services.AddScoped<IJwtService, JwtService>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IResourceResolverService, ResourceResolverService>();
        return services;
    }
}