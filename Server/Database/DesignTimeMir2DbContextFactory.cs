using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Server.Database;

/// <summary>
/// Enables `dotnet ef` migrations without relying on runtime configuration.
/// </summary>
public sealed class DesignTimeMir2DbContextFactory : IDesignTimeDbContextFactory<Mir2DbContext>
{
    public Mir2DbContext CreateDbContext(string[] args)
    {
        // Default dev/migrations DB file; runtime may override via Settings.
        var connectionString = "Data Source=mir2.db";

        var builder = new DbContextOptionsBuilder<Mir2DbContext>()
            .UseSqlite(connectionString);

        return new Mir2DbContext(builder.Options);
    }
}

