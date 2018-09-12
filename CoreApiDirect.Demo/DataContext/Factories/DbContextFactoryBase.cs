using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace CoreApiDirect.Demo.DataContext.Factories
{
    public abstract class DbContextFactoryBase<TContext> : IDesignTimeDbContextFactory<TContext>
        where TContext : DbContext
    {
        public TContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<TContext>();
            builder.UseSqlite("dummy", optionsBuilder => optionsBuilder.MigrationsAssembly(typeof(TContext).Assembly.GetName().Name));

            return (TContext)Activator.CreateInstance(typeof(TContext), builder.Options);
        }
    }
}
