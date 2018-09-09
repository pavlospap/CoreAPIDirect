using System.Collections.Generic;
using System.Globalization;
using AutoMapper;
using CoreApiDirect.Boot;
using CoreApiDirect.Demo.DataContext;
using CoreApiDirect.Demo.DataContext.Seeding;
using CoreApiDirect.Demo.Entities;
using CoreApiDirect.Demo.Mapping;
using CoreApiDirect.Demo.Repositories;
using CoreApiDirect.Entities;
using CoreApiDirect.Mapping;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CoreApiDirect.Demo
{
    public class Startup
    {
        private readonly IConfigurationRoot _config;

        public Startup(IHostingEnvironment env)
        {
            _config = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json")
                .Build();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCoreApiDirect(options =>
            {
                options.ValidateRoute = true;
                options.ValidateQueryString = true;
                options.RelatedDataLevel = 5;
                options.MaxPageSize = 25;
                options.PageSize = 15;
            })
            .AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlite(_config["ConnectionStrings:AppConnection"]);
            })
            .AddDbContext<LogDbContext>(options =>
            {
                options.UseSqlite(_config["ConnectionStrings:LogConnection"]);
            })
            .AddAutoMapper(typeof(AppProfile))
            .AddScoped<IApiMapper, ApiMapper>()
            .AddScoped<ISchoolsRepository, SchoolsRepository>()
            .AddSingleton<IEntityLocalizer, EntityLocalizer>()
            .AddSingleton<IQueryParamsMapper, QueryParamsMapper>()
            .AddTransient<IDataSeeder, DataSeeder>()
            .AddLocalization(options => options.ResourcesPath = "Resources")
            .AddMvc()
            .AddDataAnnotationsLocalization(options =>
            {
                options.DataAnnotationLocalizerProvider = (type, factory) =>
                    factory.Create(typeof(SharedResource));
            });
        }

        public void Configure(
            IApplicationBuilder app,
            IHostingEnvironment env,
            AppDbContext appDbContext,
            LogDbContext logDbContext,
            IDataSeeder dataSeeder)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage()
                    .UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler();
            }

            var cultures = new List<CultureInfo>()
            {
                new CultureInfo("en"),
                new CultureInfo("el")
            };

            app.UseRequestLocalization(new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture("en"),
                SupportedCultures = cultures,
                SupportedUICultures = cultures
            })
            .UseMvc();

            appDbContext.Database.Migrate();
            logDbContext.Database.Migrate();

            dataSeeder.SeedDataAsync().Wait();
        }
    }
}
