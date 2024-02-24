using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using WebForum.Application.Abstractions.Messaging;
using WebForum.Application.Abstractions.Providers;
using WebForum.Application.Abstractions.Repositories;
using WebForum.Infrastructure.Configuration;
using WebForum.Infrastructure.DbContext;
using WebForum.Infrastructure.Messaging;
using WebForum.Infrastructure.Options;
using WebForum.Infrastructure.Providers;
using WebForum.Infrastructure.Repositories;
using WebForum.Infrastructure.Settings;

namespace WebForum.Api.Configuration.Extensions;

public static class ConfigureInfrastructure
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services,
        IConfigurationManager configuration)
    {
        services.AddDbContext<ApplicationDbContext>(
            options =>
            {
                options.UseMySQL(configuration.GetConnectionString(Constants.Infrastructure.DatabaseConnectionString)!,
                    o => o.MigrationsHistoryTable(Database.Tables.MigrationHistory));
            });
        services.AddStackExchangeRedisCache(options =>
        {
            var connection = configuration.GetConnectionString(Constants.Infrastructure.RedisConnectionString)!;
            options.Configuration = connection;
        });
        services.AddOptions<MailOptions>().BindConfiguration(Constants.Infrastructure.EmailConfigurationSection)
            .ValidateDataAnnotations().ValidateOnStart();
        services.AddOptions<GitHubOptions>().BindConfiguration(Constants.Infrastructure.GitHubConfigurationSection)
            .ValidateDataAnnotations().ValidateOnStart();
        services.AddOptions<JwtOptions>().BindConfiguration(Constants.Infrastructure.JwtConfigurationSection)
            .ValidateDataAnnotations().ValidateOnStart();
        
        services.AddHttpClient<IGitHubClient, GitHubClient>((serviceProvider, httpClient) =>
        {
            var gitHubOptions = serviceProvider.GetRequiredService<IOptions<GitHubOptions>>().Value;
            httpClient.DefaultRequestHeaders.Add("Authorization", gitHubOptions.AccessToken);
            httpClient.DefaultRequestHeaders.Add("User-Agent", gitHubOptions.UserAgent);
            httpClient.BaseAddress = new Uri(gitHubOptions.BaseAddress);
        });
        services.AddScoped<IMailSender, MailSender>();
        services.AddScoped<IJwtProvider, JwtProvider>();
        services.AddScoped<ITwoFactorCodeProvider, TwoFactorCodeProvider>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<ITopicRepository, TopicRepository>();
        services.AddScoped<IUserTokenRepository, UserTokenRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IRegistrationRequestRepository, RegistrationRequestRepository>();
        return services;
    }
}