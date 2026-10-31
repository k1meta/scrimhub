using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace ScrimHub.Infrastructure;

public sealed class ScrimHubDbContextFactory : IDesignTimeDbContextFactory<ScrimHubDbContext>
{
    public ScrimHubDbContext CreateDbContext(string[] args)
    {
        // migrations add does not open a connection. database update reads SQL_CONNECTION or .env.
        var connectionString = LocalEnv.TryReadSqlConnection()
            ?? "Server=127.0.0.1,1;Database=ScrimHub;User Id=sa;Password=unused;TrustServerCertificate=True;Connect Timeout=1;Encrypt=False";

        var options = new DbContextOptionsBuilder<ScrimHubDbContext>()
            .UseSqlServer(
                connectionString,
                sql => sql.MigrationsAssembly(typeof(ScrimHubDbContext).Assembly.GetName().Name))
            .Options;

        return new ScrimHubDbContext(options);
    }
}