using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using WebForum.Application.Abstractions.Services;
using WebForum.Domain.Entities;
using WebForum.Persistence.DbContext;
using WebForum.Persistence.Options;

namespace WebForum.Api.Configuration.Extensions;

public static class MigrationExtensions
{
    public static IApplicationBuilder ApplyMigrations(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<ApplicationDbContext>>();
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        dbContext.Database.Migrate();
        logger.LogInformation("Applied migration for the database context at {Time}", DateTime.UtcNow);
        return app;
    }

    public static void ApplyDataSeed(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        if (dbContext.Set<User>().Any())
            return;
        
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<ApplicationDbContext>>();
        var authService = scope.ServiceProvider.GetRequiredService<IAuthService>();
        var userData = scope.ServiceProvider.GetRequiredService<IOptions<RootAdminOptions>>().Value;
        
        var rootAdmin = new User()
        {
            DisplayName = userData.DisplayName,
            Username = userData.Username,
            PasswordHash = authService.ComputePasswordHash(userData.Password),
            Email = userData.Email,
            IsEnabled = true,
            AccessFailedCount = 0,
            RoleId = UserRole.RootAdmin.RoleId,
        };
        dbContext.Set<User>().Add(rootAdmin);
        dbContext.SaveChanges();
        logger.LogInformation("Applied seeding of the database at {Time}", DateTime.UtcNow);
    }
}