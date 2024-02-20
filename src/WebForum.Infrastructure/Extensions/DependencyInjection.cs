using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WebForum.Domain.Interfaces;
using WebForum.Infrastructure.Configuration;
using WebForum.Infrastructure.DbContext;
using WebForum.Infrastructure.Messaging;
using WebForum.Infrastructure.Repositories;

namespace WebForum.Infrastructure.Extensions;

public static class DependencyInjection
{
    private const string DatabaseConnectionString = "DefaultConnection";
    private const string RedisConnectionString = "Redis";
    private const string EmailOptionsSection = "Email";

    public static IServiceCollection AddInfrastructure(this IServiceCollection services,
        IConfigurationManager configuration)
    {
        services.AddDbContext<ApplicationDbContext>(
            options =>
            {
                options.UseMySQL(configuration.GetConnectionString(DatabaseConnectionString)!,
                    o => o.MigrationsHistoryTable(Database.Tables.MigrationHistory));
            });
        services.AddStackExchangeRedisCache(options =>
        {
            var connection = configuration.GetConnectionString(RedisConnectionString)!;
            options.Configuration = connection;
        });
        services.Configure<MailOptions>(options => configuration.GetSection(EmailOptionsSection).Bind(options));
        services.AddScoped<IMailSender, MailSender>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<ITopicRepository, TopicRepository>();
        services.AddScoped<IUserTokenRepository, UserTokenRepository>();
        return services;
    }
}