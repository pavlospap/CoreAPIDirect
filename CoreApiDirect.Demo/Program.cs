using System;
using CoreApiDirect.Demo.DataContext;
using CoreApiDirect.Demo.DataContext.Seeding;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace CoreApiDirect.Demo
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateWebHostBuilder(args).Build();

            using (var scope = host.Services.CreateScope())
            {
                try
                {
                    SetupDatabase(scope.ServiceProvider);
                }
                catch (Exception ex)
                {
                    LogException(scope.ServiceProvider, ex);
                }
            }

            host.Run();
        }

        private static void SetupDatabase(IServiceProvider services)
        {
            var appDbContext = services.GetService<AppDbContext>();
            var logDbContext = services.GetService<LogDbContext>();
            var dataSeeder = services.GetService<IDataSeeder>();

            appDbContext.Database.Migrate();
            logDbContext.Database.Migrate();

            dataSeeder.SeedDataAsync().Wait();
        }

        private static void LogException(IServiceProvider services, Exception ex)
        {
            var logger = services.GetRequiredService<ILogger<Program>>();
            logger.LogError(ex, "Database error.");
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}
