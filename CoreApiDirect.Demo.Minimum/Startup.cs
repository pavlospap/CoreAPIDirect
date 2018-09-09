using CoreApiDirect.Boot;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CoreApiDirect.Demo.Minimum
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCoreApiDirect()
            .AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlite("Data Source =.\\demo.db;");
            })
            .AddMvc();
        }

        public void Configure(IApplicationBuilder app, AppDbContext dbContext)
        {
            app.UseMvc();
            dbContext.Database.Migrate();
        }
    }
}
