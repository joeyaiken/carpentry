using Carpentry.Data.Implementations;
using Carpentry.Data.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SpaServices;
using Microsoft.AspNetCore.SpaServices.ReactDevelopmentServer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
//using Carpentry.Data.LegacyDataContextLegacy;
//using Carpentry.Data.LegacyDataContext;
using Carpentry.Logic.Interfaces;
using Carpentry.Logic.Implementations;
//using Carpentry.Interfaces;
//using Carpentry.Implementations;
using Microsoft.Extensions.Hosting;
using Carpentry.Data.DataContext;
using Carpentry.UI.Util;

namespace Carpentry.UI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            //Going to actually pull the fill filepaths from app settings now
            //TODO Eventually I could make a static config class and just read from that class
            //TODO Everything else still needs this DB pattern
            string cardDatabaseFilepath = Configuration.GetValue<string>("AppSettings:CardDatabaseFilepath");
            string scryDatabaseFilepath = Configuration.GetValue<string>("AppSettings:ScryDatabaseFilepath");

            //DBs
            services.AddDbContext<ScryfallDataContext>(options => options.UseSqlite($"Data Source={scryDatabaseFilepath}"));
            services.AddDbContext<CarpentryDataContext>(options => options.UseSqlite($"Data Source={cardDatabaseFilepath}"));

            //DB repos
            services.AddScoped<ICardDataRepo, CardDataRepo>();
            services.AddScoped<IDeckDataRepo, DeckDataRepo>();
            services.AddScoped<IInventoryDataRepo, InventoryDataRepo>();
            services.AddScoped<IScryfallDataRepo, ScryfallRepo>();

            //DB services
            services.AddScoped<IDataReferenceService, DataReferenceService>();




            //private readonly IInventoryDataRepo _inventoryRepo;


            ////string repo & repo's HTTP client
            //services.AddScoped<ICardStringRepo, ScryfallRepo>();
            //services.AddHttpClient<ICardStringRepo, ScryfallRepo>();

            //card DB repo
            //services.AddScoped<ILegacyCardRepo, SqliteCardRepo>();



            //Logic services
            services.AddScoped<ICardSearchService, CardSearchService>();
            services.AddScoped<IDeckService, DeckService>();
            services.AddScoped<IInventoryService, InventoryService>();
            services.AddScoped<IFilterService, FilterService>();

            //Util services
            services.AddScoped<IMapperService, MapperService>();


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

            //app.UseMvc(routes =>
            //{
            //    routes.MapRoute(
            //        name: "default",
            //        template: "{controller}/{action=Index}/{id?}");
            //});

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action=Index}/{id?}");
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
