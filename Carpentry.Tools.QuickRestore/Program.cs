using System;
using System.IO;
using System.Threading.Tasks;
using Carpentry.Data.DataContext;
using Carpentry.Logic.Interfaces;
//using Carpentry.Data.LegacyDataContext;
//using Carpentry.Logic.Implementations;
//using Carpentry.Logic.Interfaces;
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

            //string cardDatabaseLocation = $"Data Source={appConfig.DatabaseLocation}";

            string cardDatabaseLocation = $"Data Source={appConfig.DatabaseLocation}";

            string scryDatabaseLocation = $"Data Source={appConfig.ScryDataLocation}";

            //what else
            //backup locations can be stored in some config

            var serviceProvider = new ServiceCollection()
                .AddSingleton(Configuration)
                .AddSingleton<IDataBackupConfig, BackupConfig>()

                .AddLogging(config => config.AddSerilog())

                .AddDbContext<CarpentryDataContext>(options => options.UseSqlite(cardDatabaseLocation))
                .AddDbContext<ScryfallDataContext>(options => options.UseSqlite(scryDatabaseLocation))


                //data services
                //.AddScoped<IScryfallService>
                .AddScoped<ICardDataRepo, CardDataRepo>()
                .AddScoped<IDeckDataRepo, DeckDataRepo>()
                .AddScoped<IInventoryDataRepo, InventoryDataRepo>()
                .AddScoped<IScryfallDataRepo, ScryfallRepo>()
                .AddScoped<IDataReferenceService, DataReferenceService>()

                .AddSingleton<IDataReferenceRepo, DataReferenceRepo>()

                //logic services
                .AddScoped<IDataRestoreService, DataRestoreService>()
                .AddScoped<IDataUpdateService, DataUpdateService>()
                /*
                
                IDataReferenceService dataReferenceService,
                IDataBackupConfig config,
                
                */

                .AddScoped<IScryfallService, ScryfallService>()
                .AddHttpClient<IScryfallService, ScryfallService>().Services



                //.AddDbContext<SqliteDataContext>(options => options.UseSqlite(cardDatabaseLocation))
                //.AddScoped<IDataBackupService, DataBackupService>()
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