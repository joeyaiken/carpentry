using System.IO;
using System.Threading.Tasks;
using Carpentry.Data.DataContext;
using Carpentry.Logic.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;
using Carpentry.Logic.Implementations;
using Carpentry.Data.Implementations;
using Carpentry.Data.Interfaces;

namespace Carpentry.Tools.QuickRestore
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var serviceProvider = BuildServiceProvider();

            var logger = serviceProvider.GetService<ILoggerFactory>().CreateLogger<Program>();

            logger.LogInformation("----------Carpentry Quick Backup Tool - Initializing----------");

            var restoreService = serviceProvider.GetService<IDataRestoreService>();

            logger.LogInformation("initialized successfully");

            await restoreService.RestoreDatabase();

            logger.LogInformation("Completed successfully");
        }


        private static ServiceProvider BuildServiceProvider()
        {
            var appConfig = new BackupConfig(Configuration);

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .MinimumLevel.Override("Microsoft.EntityFrameworkCore", LogEventLevel.Warning)
                .MinimumLevel.Override("System.Net.Http", LogEventLevel.Warning)
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .CreateLogger();

            string cardDatabaseLocation = $"Data Source={appConfig.DatabaseLocation}";

            string scryDatabaseLocation = $"Data Source={appConfig.ScryDataLocation}";

            var serviceProvider = new ServiceCollection()
                .AddSingleton(Configuration)
                .AddSingleton<IDataBackupConfig, BackupConfig>()

                .AddLogging(config => config.AddSerilog())

                //.AddDbContext<CarpentryDataContext>(options => options.UseSqlite(cardDatabaseLocation))
                //.AddDbContext<ScryfallDataContext>(options => options.UseSqlite(scryDatabaseLocation))

                //.AddDbContext<ScryfallDataContext>(options => options.UseSqlServer(Configuration.GetConnectionString("ScryfallDataContext")))
                //.AddDbContext<CarpentryDataContext>(options => options.UseSqlServer(Configuration.GetConnectionString("CarpentryDataContext")))

                ////data services
                //.AddSingleton<ICardDataRepo, CardDataRepo>()
                //.AddSingleton<IDeckDataRepo, DeckDataRepo>()
                //.AddSingleton<IInventoryDataRepo, InventoryDataRepo>()
                //.AddSingleton<IScryfallDataRepo, ScryfallRepo>()
                //.AddSingleton<IDataReferenceService, DataReferenceService>()
                //.AddSingleton<IDataReferenceRepo, DataReferenceRepo>()

                ////logic services
                .AddScoped<IDataRestoreService, DataRestoreService>()
                //.AddScoped<IDataUpdateService, DataUpdateService>()

                //.AddScoped<IScryfallService, ScryfallService>()
                //.AddHttpClient<IScryfallService, ScryfallService>().Services



                .AddDbContext<ScryfallDataContext>(options => options.UseSqlServer(Configuration.GetConnectionString("ScryfallDataContext")))

                //.AddDbContext<CarpentryDataContext>(options => options.UseSqlite($"Data Source={cardDatabaseFilepath}"))
                .AddDbContext<CarpentryDataContext>(options => options.UseSqlServer(Configuration.GetConnectionString("CarpentryDataContext")))


                //.AddSingleton<IDataBackupConfig, CarpentryAppConfig>()

                ////DB repos
                .AddScoped<ICardDataRepo, CardDataRepo>()
                .AddScoped<IDeckDataRepo, DeckDataRepo>()
                .AddScoped<IInventoryDataRepo, InventoryDataRepo>()
                .AddScoped<IScryfallDataRepo, ScryfallRepo>()
                .AddScoped<IDataReferenceRepo, DataReferenceRepo>()


                //DB 
                .AddScoped<IDataReferenceService, DataReferenceService>()
                .AddScoped<IDataQueryService, DataQueryService>()


                //Logic 6
                .AddScoped<ICardSearchService, CardSearchService>()
                .AddScoped<IDeckService, DeckService>()
                .AddScoped<IInventoryService, InventoryService>()

                .AddScoped<ICardImportService, CardImportService>()
                .AddScoped<IDataUpdateService, DataUpdateService>()
                .AddScoped<IDataBackupService, DataBackupService>()
                .AddScoped<IFilterService, FilterService>()

                .AddScoped<IScryfallService, ScryfallService>()
                .AddHttpClient<IScryfallService, ScryfallService>().Services

                .BuildServiceProvider();

            return serviceProvider;
        }

        private static IConfiguration Configuration
        {
            get
            {
                return new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json")
                    .Build();
            }
        }
    }

}