using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using WebForum.Application.Abstractions.Messaging;
using WebForum.Application.Abstractions.Repositories;
using WebForum.Infrastructure.Configuration;
using WebForum.Infrastructure.DbContext;
using WebForum.Infrastructure.Messaging;
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
        services.AddOptions<MailSettings>().BindConfiguration(Constants.Infrastructure.EmailConfigurationSection)
            .ValidateDataAnnotations().ValidateOnStart();
        services.AddOptions<GitHubSettings>().BindConfiguration(Constants.Infrastructure.GitHubConfigurationSection)
            .ValidateDataAnnotations().ValidateOnStart();
        services.AddHttpClient<IGitHubClient, GitHubClient>((serviceProvider, httpClient) =>
        {
            var githubSettings = serviceProvider.GetRequiredService<IOptions<GitHubSettings>>().Value;
            httpClient.DefaultRequestHeaders.Add("Authorization", githubSettings.AccessToken);
            httpClient.DefaultRequestHeaders.Add("User-Agent", githubSettings.UserAgent);
            httpClient.BaseAddress = new Uri(githubSettings.BaseAddress);
        });
        services.AddScoped<IMailSender, MailSender>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<ITopicRepository, TopicRepository>();
        services.AddScoped<IUserTokenRepository, UserTokenRepository>();
        return services;
    }
}