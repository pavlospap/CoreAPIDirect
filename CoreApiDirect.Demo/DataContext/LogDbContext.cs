using CoreApiDirect.Demo.Entities.Logging;
using Microsoft.EntityFrameworkCore;

namespace CoreApiDirect.Demo.DataContext
{
    public class LogDbContext : DbContext
    {
        public DbSet<LogEvent> LogEvents { get; set; }
        public DbSet<LogDetail> LogDetail { get; set; }
        public DbSet<SystemInfo> SystemInfo { get; set; }

        public LogDbContext(DbContextOptions<LogDbContext> options)
            : base(options)
        {
        }
    }
}
