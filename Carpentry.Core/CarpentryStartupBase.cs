using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Carpentry.ScryfallData;
using Carpentry.CarpentryData;
using Carpentry.Logic;
using Microsoft.Extensions.Hosting;
using System;

namespace Carpentry.Core
{
    public abstract class CarpentryStartupBase
    {
        //public IConfiguration Configuration { get; }
        protected readonly IConfiguration Configuration; //{ get; }
        protected readonly IWebHostEnvironment Env;

        public CarpentryStartupBase(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            Env = env;
        }

        public abstract void ConfigureServices(IServiceCollection services);

        public abstract void Configure(IApplicationBuilder app, IWebHostEnvironment env);

        protected void ConfigureBase(IServiceCollection services)
        {

            if (Env.IsDevelopment())
            {
                string cardDatabaseFilepath = Configuration.GetValue<string>("AppSettings:CardDatabaseFilepath");
                services.AddDbContext<CarpentryDataContext>(options => options.UseSqlite($"Data Source={cardDatabaseFilepath}"));
            }
            else if (Env.IsProduction())
            {
                services.AddDbContext<CarpentryDataContext>(options => options.UseSqlServer(Configuration.GetConnectionString("CarpentryDataContext")));
            }
            else
            {
                throw new Exception("Unexpected environment configuration");
            }

            //Both use a scryfall DB
            services.AddDbContext<ScryfallDataContext>(options => options.UseSqlServer(Configuration.GetConnectionString("ScryfallDataContext")));

            //Everything uses the same set of services
            services.AddSingleton<IDataBackupConfig, CarpentryAppConfig>();

            //Logic services
            services.AddScoped<ISearchService, SearchService>();
            services.AddScoped<IDeckService, DeckService>();
            services.AddScoped<IInventoryService, InventoryService>();
            services.AddScoped<IDataIntegrityService, DataIntegrityService>();

            services.AddScoped<IDataImportService, DataImportService>();
            services.AddScoped<IDataUpdateService, DataUpdateService>();
            services.AddScoped<IDataExportService, DataExportService>();
            services.AddScoped<IFilterService, FilterService>();

            services.AddScoped<IScryfallService, ScryfallService>();
            services.AddHttpClient<IScryfallService, ScryfallService>();
        }

        //protected void ConfigureSqlServerDatabase(IServiceCollection services)
        //{
        //    services.AddDbContext<ScryfallDataContext>(options => options.UseSqlServer(Configuration.GetConnectionString("ScryfallDataContext")));
        //    services.AddDbContext<CarpentryDataContext>(options => options.UseSqlServer(Configuration.GetConnectionString("CarpentryDataContext")));
        //}

        //protected void ConfigureSqliteDatabase(IServiceCollection services)
        //{
        //    string cardDatabaseFilepath = Configuration.GetValue<string>("AppSettings:CardDatabaseFilepath");
        //    string scryDatabaseFilepath = Configuration.GetValue<string>("AppSettings:ScryDatabaseFilepath");
        //    services.AddDbContext<ScryfallDataContext>(options => options.UseSqlite($"Data Source={scryDatabaseFilepath}"));
        //    services.AddDbContext<CarpentryDataContext>(options => options.UseSqlite($"Data Source={cardDatabaseFilepath}"));
        //}

        //protected void ConfigureCarpentryServices(IServiceCollection services)
        //{
        //    services.AddSingleton<IDataBackupConfig, CarpentryAppConfig>();

        //    //Logic services
        //    services.AddScoped<ISearchService, SearchService>();
        //    services.AddScoped<IDeckService, DeckService>();
        //    services.AddScoped<IInventoryService, InventoryService>();
        //    services.AddScoped<IDataIntegrityService, DataIntegrityService>();

        //    services.AddScoped<IDataImportService, DataImportService>();
        //    services.AddScoped<IDataUpdateService, DataUpdateService>();
        //    services.AddScoped<IDataExportService, DataExportService>();
        //    services.AddScoped<IFilterService, FilterService>();

        //    services.AddScoped<IScryfallService, ScryfallService>();
        //    services.AddHttpClient<IScryfallService, ScryfallService>();
        //}

    }
}
