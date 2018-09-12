using System;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace CoreApiDirect.Demo.Minimum
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
            var dbContext = services.GetService<AppDbContext>();
            dbContext.Database.Migrate();
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
