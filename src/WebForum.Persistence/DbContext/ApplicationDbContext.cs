using Microsoft.EntityFrameworkCore;

namespace WebForum.Persistence.DbContext;

public class ApplicationDbContext(DbContextOptions options) : Microsoft.EntityFrameworkCore.DbContext(options)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }
}