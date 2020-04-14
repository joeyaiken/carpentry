using System;
using System.IO;
using System.Threading.Tasks;
using Carpentry.Data.Interfaces;
using Carpentry.Logic.Implementations;
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
using Carpentry.Data.Implementations;

namespace Carpentry.Tools.QuickUpdate
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var serviceProvider = BuildServiceProvider();

            var logger = serviceProvider.GetService<ILoggerFactory>().CreateLogger<Program>();

            logger.LogInformation("----------Carpentry Quick Backup Tool - Initializing----------");

            var updateService = serviceProvider.GetService<IDataUpdateService>();

            await updateService.UpdateAllSets();

            logger.LogInformation("Completed successfully");
        }


        private static ServiceProvider BuildServiceProvider()
        {
            //var appConfig = new BackupToolConfig(Configuration);

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .MinimumLevel.Override("Microsoft.EntityFrameworkCore", LogEventLevel.Warning)
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .CreateLogger();

            //string cardDatabaseLocation = $"Data Source={appConfig.DatabaseLocation}";

            var serviceProvider = new ServiceCollection()
                .AddSingleton(Configuration)
                //.AddSingleton<IDataBackupConfig, BackupToolConfig>()

                .AddLogging(config => config.AddSerilog())

                //.AddDbContext<SqliteDataContext>(options => options.UseSqlite(cardDatabaseLocation))
                .AddScoped<IDataUpdateService, DataUpdateService>()

                //IScryfallService scryService,
                .AddScoped<IScryfallService, ScryfallService>()
                .AddHttpClient<IScryfallService, ScryfallService>().Services

                //ICardRepo cardRepo,
                //.AddScoped<ICardRepo, CarpentryCardRepo>() //Name TBD, this hasn't been implemented yet
                //IScryfallRepo scryfallRepo
                .AddScoped<IScryfallDataRepo, ScryfallRepo>()






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
