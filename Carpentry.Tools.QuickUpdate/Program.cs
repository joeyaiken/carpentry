using System;
using System.IO;
using System.Threading.Tasks;
using Carpentry.Data.Interfaces;
using Carpentry.Logic.Implementations;
using Carpentry.Logic.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;
using Carpentry.Data.Implementations;
using Carpentry.Data.DataContext;
using System.Linq;

namespace Carpentry.Tools.QuickUpdate
{
    class Program
    {
        private static readonly int DB_UPDATE_INTERVAL_DAYS = 0; //Increase this if you only want to update data every few days

        static async Task Main(string[] args)
        {
            var serviceProvider = BuildServiceProvider();

            var logger = serviceProvider.GetService<ILoggerFactory>().CreateLogger<Program>();

            logger.LogInformation("Carpentry QuickUpdate - Initializing...");

            var updateService = serviceProvider.GetService<IDataUpdateService>();

            logger.LogInformation("Getting list of tracked set");

            var trackedSets = await updateService.GetTrackedSets(false, true);

            var setsToUpdate = trackedSets
                .Where(s => s.DataLastUpdated == null || s.DataLastUpdated.Value.AddDays(DB_UPDATE_INTERVAL_DAYS) < DateTime.Today.Date)
                .ToList();

            logger.LogInformation($"Found {setsToUpdate.Count} sets to update");


            for (int i = 0; i < setsToUpdate.Count; i++)
            {
                logger.LogInformation($"Updating {setsToUpdate[i].Code} [{i+1}/{setsToUpdate.Count}]...");
                await updateService.UpdateTrackedSet(setsToUpdate[i].SetId);
                logger.LogInformation($"Updating {setsToUpdate[i].Code} complete!");
            }

            logger.LogInformation("Carpentry QuickUpdate - Completed successfully");
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

                .AddDbContext<ScryfallDataContext>(options => options.UseSqlServer(Configuration.GetConnectionString("ScryfallDataContext")))
                .AddDbContext<CarpentryDataContext>(options => options.UseSqlServer(Configuration.GetConnectionString("CarpentryDataContext")))

                //data services
                .AddSingleton<ICardDataRepo, CardDataRepo>()
                .AddSingleton<IScryfallDataRepo, ScryfallRepo>()
                .AddScoped<ICoreDataRepo, CoreDataRepo>()

                //logic services
                .AddScoped<IDataUpdateService, DataUpdateService>()

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
