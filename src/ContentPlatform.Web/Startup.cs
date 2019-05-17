using ContentPlatform.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace ContentPlatform.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddLogging(options =>
                options.AddConsole().AddFilter(
                    DbLoggerCategory.Database.Command.Name, level => level == LogLevel.Information));

            var loggerFactory = services.BuildServiceProvider().GetService<ILoggerFactory>();

            services.AddDbContextPool<ContentPlatformContext>(
                options => options.UseSqlServer(
                    Configuration.GetConnectionString("ContentPlatformConnection"), 
                    providerOptions =>
                    {
                        providerOptions.CommandTimeout(60);
                        providerOptions.MaxBatchSize(100);
                        //providerOptions.MigrationsAssembly("");
                        providerOptions.MinBatchSize(6);
                        //providerOptions.MigrationsHistoryTable("");
                    })
                    .UseLoggerFactory(loggerFactory)
                    .EnableDetailedErrors()
                    .EnableSensitiveDataLogging());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<ContentPlatformContext>();
                context?.Database.Migrate();
            }

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}