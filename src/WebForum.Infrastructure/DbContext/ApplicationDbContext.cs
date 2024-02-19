using Microsoft.EntityFrameworkCore;
using WebForum.Domain.Entities;

namespace WebForum.Infrastructure.DbContext;

public class ApplicationDbContext(DbContextOptions options) : Microsoft.EntityFrameworkCore.DbContext(options)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
    
    public DbSet<Topic> Topics { get; set; }
}