using Carpentry.Data.DataContext;
using Carpentry.Data.Implementations;
using Carpentry.Data.Interfaces;
using Carpentry.Logic.Implementations;
using Carpentry.Logic.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SpaServices.ReactDevelopmentServer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.SpaServices.AngularCli;

namespace Carpentry.Angular
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
            //DBs

            services.AddDbContext<ScryfallDataContext>(options => options.UseSqlServer(Configuration.GetConnectionString("ScryfallDataContext")));

            //services.AddDbContext<CarpentryDataContext>(options => options.UseSqlite($"Data Source={cardDatabaseFilepath}"));
            services.AddDbContext<CarpentryDataContext>(options => options.UseSqlServer(Configuration.GetConnectionString("CarpentryDataContext")));

            services.AddSingleton<IDataBackupConfig, CarpentryAppConfig>();

            //DB repos
            services.AddScoped<ICardDataRepo, CardDataRepo>();
            services.AddScoped<DeckDataRepo>();
            services.AddScoped<IInventoryDataRepo, InventoryDataRepo>();
            services.AddScoped<IScryfallDataRepo, ScryfallRepo>();
            services.AddScoped<ICoreDataRepo, CoreDataRepo>();

            //Logic services
            services.AddScoped<ISearchService, SearchService>();
            services.AddScoped<IDeckService, DeckService>();
            services.AddScoped<IInventoryService, InventoryService>();

            services.AddScoped<IDataImportService, DataImportService>();
            services.AddScoped<IDataUpdateService, DataUpdateService>();
            services.AddScoped<IDataExportService, DataExportService>();
            services.AddScoped<IFilterService, FilterService>();

            services.AddScoped<IScryfallService, ScryfallService>();
            services.AddHttpClient<IScryfallService, ScryfallService>();

            services.AddControllersWithViews();
            // In production, the Angular files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/dist";
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
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
