using Microsoft.EntityFrameworkCore;

namespace CoreApiDirect.Demo.Minimum
{
    public class AppDbContext : DbContext
    {
        public DbSet<Article> Articles { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }
    }
}
