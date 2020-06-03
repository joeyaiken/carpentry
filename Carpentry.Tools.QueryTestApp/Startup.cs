using Carpentry.Data.DataContext;
using Carpentry.Data.Implementations;
using Carpentry.Data.Interfaces;
using Carpentry.Logic.Implementations;
using Carpentry.Logic.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SpaServices.ReactDevelopmentServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Carpentry.Tools.QueryTestApp
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
            ////Going to actually pull the full filepaths from app settings now
            //string cardDatabaseFilepath = Configuration.GetValue<string>("AppSettings:CardDatabaseFilepath");
            //string scryDatabaseFilepath = Configuration.GetValue<string>("AppSettings:ScryDatabaseFilepath");

            //TODO - Cut out what services are added here

            ////DBs
            services.AddDbContext<ScryfallDataContext>(options => options.UseSqlServer(Configuration.GetConnectionString("ScryfallDataContext")));

            services.AddDbContext<CarpentryDataContext>(options => options.UseSqlServer(Configuration.GetConnectionString("CarpentryDataContext")));

            ////DB repos
            services.AddScoped<ICardDataRepo, CardDataRepo>();
            //services.AddScoped<IDeckDataRepo, DeckDataRepo>();
            services.AddScoped<IInventoryDataRepo, InventoryDataRepo>();
            services.AddScoped<IScryfallDataRepo, ScryfallRepo>();
            services.AddScoped<IDataReferenceRepo, DataReferenceRepo>();


            //DB services
            services.AddScoped<IDataReferenceService, DataReferenceService>();
            services.AddScoped<IDataQueryService, DataQueryService>();


            //Logic services
            services.AddScoped<ICardSearchService, CardSearchService>();
            //services.AddScoped<IDeckService, DeckService>();
            services.AddScoped<IInventoryService, InventoryService>();


            services.AddScoped<IDataUpdateService, DataUpdateService>();
            services.AddScoped<IFilterService, FilterService>();

            services.AddScoped<IScryfallService, ScryfallService>();
            services.AddHttpClient<IScryfallService, ScryfallService>();

            services.AddControllersWithViews();

            // In production, the React files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/build";
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
            app.UseSpaStaticFiles();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action=Index}/{id?}");
            });

            app.UseSpa(spa =>
            {
                spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment())
                {
                    spa.UseReactDevelopmentServer(npmScript: "start");
                }
            });
        }
    }
}
