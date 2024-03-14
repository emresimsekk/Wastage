using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Wastage.Api.Persistence.Data.DbContexts;

public class WritableDbContext : DbContext
{
    public WritableDbContext(DbContextOptions<WritableDbContext> dbContextOptions) : base(dbContextOptions)
    {
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        Database.EnsureCreated();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}
