using Carpentry.Data.Implementations;
using Carpentry.Data.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SpaServices.ReactDevelopmentServer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
//using Carpentry.Data.DataContextLegacy;
using Carpentry.Data.DataContext;
using Carpentry.Interfaces;
using Carpentry.Implementations;

namespace Carpentry
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
            //Going to actually pull the fill filepaths from app settings now
            //TODO Eventually I could make a static config class and just read from that class
            //TODO Everything else still needs this DB pattern
            string cardDatabaseFilepath = Configuration.GetValue<string>("AppSettings:CardDatabaseFilepath");
            string scryDatabaseFilepath = Configuration.GetValue<string>("AppSettings:ScryDatabaseFilepath");
            //string legacyDatabaseFilepath = Configuration.GetValue<string>("AppSettings:LegacyDatabaseFilepath");

            services.AddDbContext<ScryfallDataContext>(options => options.UseSqlite($"Data Source={scryDatabaseFilepath}"));
            //services.AddDbContext<LegacySqliteDataContext>(options => options.UseSqlite($"Data Source={legacyDatabaseFilepath}"));
            services.AddDbContext<SqliteDataContext>(options => options.UseSqlite($"Data Source={cardDatabaseFilepath}"));

            //string repo
            services.AddScoped<ICardStringRepo, ScryfallRepo>();
            services.AddHttpClient<ICardStringRepo, ScryfallRepo>();

            //new & legacy DBs
            services.AddScoped<ICardRepo, SqliteCardRepo>();
            //services.AddScoped<ILegacySqliteCardRepo, LegacySqliteCardRepo>();



            //carpentry service
            services.AddScoped<ICarpentryService, CarpentryService>();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            // In production, the React files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/build";
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
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
            app.UseSpaStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller}/{action=Index}/{id?}");
            });

            //app.UseCors();

            app.UseSpa(spa =>
            {
                spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment())
                {
                    spa.UseReactDevelopmentServer(npmScript: "start");
                    //spa.UseProxyToSpaDevelopmentServer("http://localhost:3000");
                }
            });
        }
    }
}
