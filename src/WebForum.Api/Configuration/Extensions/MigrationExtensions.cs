using Microsoft.EntityFrameworkCore;
using WebForum.Persistence.DbContext;

namespace WebForum.Api.Configuration.Extensions;

public static class MigrationExtensions
{
    public static IApplicationBuilder ApplyMigrations(this IApplicationBuilder app)
    {
        var scope = app.ApplicationServices.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        dbContext.Database.Migrate();
        return app;
    }
}