using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using WebForum.Persistence.Configuration;

namespace WebForum.Persistence.DbContext;

public class ApplicationDesignDbContext() : IDesignTimeDbContextFactory<ApplicationDbContext>
{
    private const string ConnectionString =
        "server=localhost;port=3306;database=web-forum;user=student;password=student";

    // Used only in design time for code first approach of building database and performing migrations
    public ApplicationDbContext CreateDbContext(string[] args)
    {
        var options = new DbContextOptionsBuilder()
            .UseMySQL(ConnectionString, 
                o => o.MigrationsHistoryTable(Database.Tables.MigrationHistory))
            .Options;
        return new ApplicationDbContext(options);
    }
}