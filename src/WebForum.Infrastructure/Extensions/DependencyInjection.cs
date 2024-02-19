using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WebForum.Domain.Interfaces;
using WebForum.Infrastructure.Configuration;
using WebForum.Infrastructure.DbContext;
using WebForum.Infrastructure.Repositories;

namespace WebForum.Infrastructure.Extensions;

public static class DependencyInjection
{
    private const string DefaultConnectionString = "DefaultConnection";

    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(
            options =>
            {
                options.UseMySQL(configuration.GetConnectionString(DefaultConnectionString)!,
                    o => o.MigrationsHistoryTable(Database.Tables.MigrationHistory));
            });

        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<ITopicRepository, TopicRepository>();
        return services;
    }
}