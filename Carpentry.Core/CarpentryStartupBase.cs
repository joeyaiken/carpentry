using Carpentry.Logic.Implementations;
using Carpentry.Logic.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Carpentry.ScryfallData;
using Carpentry.CarpentryData;
using Carpentry.DataLogic.Interfaces;
using Carpentry.DataLogic.Implementations;

namespace Carpentry.Core
{
    public abstract class CarpentryStartupBase
    {

        public CarpentryStartupBase(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public abstract void ConfigureServices(IServiceCollection services);

        public abstract void Configure(IApplicationBuilder app, IWebHostEnvironment env);

        protected void ConfigureSqlServerDatabase(IServiceCollection services)
        {
            services.AddDbContext<ScryfallDataContext>(options => options.UseSqlServer(Configuration.GetConnectionString("ScryfallDataContext")));
            services.AddDbContext<CarpentryDataContext>(options => options.UseSqlServer(Configuration.GetConnectionString("CarpentryDataContext")));
        }

        protected void ConfigureSqliteDatabase(IServiceCollection services)
        {
            string cardDatabaseFilepath = Configuration.GetValue<string>("AppSettings:CardDatabaseFilepath");
            string scryDatabaseFilepath = Configuration.GetValue<string>("AppSettings:ScryDatabaseFilepath");
            services.AddDbContext<ScryfallDataContext>(options => options.UseSqlite($"Data Source={scryDatabaseFilepath}"));
            services.AddDbContext<CarpentryDataContext>(options => options.UseSqlite($"Data Source={cardDatabaseFilepath}"));
        }

        protected void ConfigureCarpentryServices(IServiceCollection services)
        {
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
        }

    }
}
