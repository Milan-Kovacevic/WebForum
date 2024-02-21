using Microsoft.EntityFrameworkCore;
using WebForum.Application.Abstractions.Messaging;
using WebForum.Application.Abstractions.Repositories;
using WebForum.Domain.Options;
using WebForum.Infrastructure.Configuration;
using WebForum.Infrastructure.DbContext;
using WebForum.Infrastructure.Messaging;
using WebForum.Infrastructure.Repositories;

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
        services.AddOptions<MailOptions>().BindConfiguration(Constants.Infrastructure.EmailOptionsSection)
            .ValidateDataAnnotations().ValidateOnStart();

        services.AddScoped<IMailSender, MailSender>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<ITopicRepository, TopicRepository>();
        services.AddScoped<IUserTokenRepository, UserTokenRepository>();
        return services;
    }
}