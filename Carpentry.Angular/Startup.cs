using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Carpentry.Core;
using System.Linq;

namespace Carpentry.Angular
{
    public class Startup : CarpentryStartupBase
    {
        private readonly IConfiguration _config;

        public Startup(IConfiguration configuration, IWebHostEnvironment env) : base(configuration, env) 
        {
            _config = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public override void ConfigureServices(IServiceCollection services)
        {
            //var keys = _config.AsEnumerable().ToList();
            ////var test = _config.GetSection("Args");
            ////var test3 = _config.GetSection("TEST");
            ////var test4 = _config.GetSection("-Args");
            ////var test2 = _config.GetValue<string>("Args");
            //var really = _config.GetValue<string>("ANCM_LAUNCHER_ARGS");

            //var runAs = _config.GetValue<string>("RunAs");

            ////This works
            ////Lesson learned: Maybe I should just read this from an AppSettings instead of trying to figure this nonsense out
            //var okay = _config.GetValue<string>("Env");
            ////_config.GetSection("")
            //int breakpoint = 1;




            ConfigureBase(services);
            services.AddControllersWithViews();
            // In production, the Angular files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/dist";
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public override void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }



            app.UseHttpsRedirection();
            app.UseStaticFiles();
            if (!env.IsDevelopment())
            {
                app.UseSpaStaticFiles();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action=Index}/{id?}");
            });

            app.UseSpa(spa =>
            {
                // To learn more about options for serving an Angular SPA from ASP.NET Core,
                // see https://go.microsoft.com/fwlink/?linkid=864501

                spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment())
                {
                    spa.UseAngularCliServer(npmScript: "start");
                }
            });
        }
    }
}
