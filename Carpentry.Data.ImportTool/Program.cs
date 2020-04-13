using System;
using System.IO;
using System.Threading.Tasks;
using Carpentry.Data.LegacyDataContext;
using Carpentry.Data.Implementations;
using Carpentry.Data.Interfaces;
//using Carpentry.Data.MigrationTool.Services;
//using Carpentry.Implementations;
//using Carpentry.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Http;
using Serilog;
using Serilog.Events;



namespace Carpentry.Data.ImportTool
{
    /// <summary>
    /// The Carpentry Import Tool is a simple console app that can load text lists of decks into the Carpentry database
    /// It should load config information, as well as decks to be imported, from appsettings
    /// </summary>
    class Program
    {
        //RN this would only be needeed for Serilog
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

        static async Task Main(string[] args)
        {
            var appConfig = new ImportToolConfig(Configuration);

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Warning()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .CreateLogger();


            //string cardDbDataSourceString = $"Data Source={appConfig.BackupSourceDbLocation}";
            //string cardDbDataSourceString = $"Data Source={appConfig.RestoreDestinationCardDbLocation}";
            //string scryDbDataSourceString = $"Data Source={appConfig.RestoreDestinationScryDbLocation}";

            var serviceProvider = new ServiceCollection()
                .AddSingleton(Configuration)
                .AddSingleton<ImportToolConfig>()

                .AddLogging(config => config.AddSerilog())

                //.AddDbContext<SqliteDataContext>(options => options.UseSqlite(cardDbDataSourceString))
                //.AddDbContext<ScryfallDataContext>(options => options.UseSqlite(scryDbDataSourceString))



                .AddScoped<ICardStringRepo, LegacyScryfallRepo>()
                .AddHttpClient<ICardStringRepo, LegacyScryfallRepo>().Services

                .AddScoped<ILegacyCardRepo, SqliteCardRepo>()



                //.AddScoped<DataBackupService>()
                //.AddScoped<DataRestoreService>()

                //.AddScoped<ICarpentryService, CarpentryService>()



                .BuildServiceProvider();

  
            var logger = serviceProvider.GetService<ILoggerFactory>().CreateLogger<Program>();

            logger.LogWarning("\n--------------------------------------------\nCarpentry Data Import Tool - Initializing\n--------------------------------------------");

            //var backupService = serviceProvider.GetService<DataBackupService>();
            //backupService.SaveDb();

            //var restoreService = serviceProvider.GetService<DataRestoreService>();
            //await restoreService.RestoreDb();

            Log.CloseAndFlush();
            return;
        }
    }
}
