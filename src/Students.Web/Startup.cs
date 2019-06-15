using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Students.Logic;
using Students.Web.Utils;
using System.Collections.Generic;

namespace Students.Web
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

            services.AddDbContextPool<StudentContext>(
                options => options.UseSqlServer(
                    Configuration.GetConnectionString("StudentsConnection")));

            services.AddTransient<StudentContext>();
            services.AddTransient<StudentRepository>();
            services.AddTransient<CourseRepository>();
            services.AddTransient<ICommandHandler<EditPersonalInfoCommand>>(provider =>
                new AuditLoggingDecorator<EditPersonalInfoCommand>(
                    new DatabaseRetryDecorator<EditPersonalInfoCommand>(
                        new EditPersonalInfoCommandHandler(provider.GetService<StudentRepository>()))));
            services.AddTransient<IQueryHandler<GetListQuery, List<StudentDto>>, GetListQueryHandler>();
            services.AddTransient<ICommandHandler<DisenrollCommand>, DisenrollCommandHandler>();
            services.AddTransient<ICommandHandler<EnrollCommand>,EnrollCommandHandler>();
            services.AddTransient<ICommandHandler<RegisterCommand>,RegisterCommandHandler>();
            services.AddTransient<ICommandHandler<TransferCommand>,TransferCommandHandler>();
            services.AddTransient<ICommandHandler<UnregisterCommand>,UnregisterCommandHandler>();
            services.AddSingleton<Messages>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<StudentContext>();
                context?.Database.Migrate();
            }

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseStaticFiles();

            app.UseMiddleware<ExceptionHandler>();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}