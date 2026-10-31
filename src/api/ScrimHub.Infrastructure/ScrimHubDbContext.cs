using Microsoft.EntityFrameworkCore;

namespace ScrimHub.Infrastructure;

public class ScrimHubDbContext : DbContext
{
    public ScrimHubDbContext(DbContextOptions<ScrimHubDbContext> options)
        : base(options)
    {
    }
}