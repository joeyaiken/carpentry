using System.IO;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;
using Carpentry.Logic.Models;
using Carpentry.ScryfallData;
using Carpentry.CarpentryData;
using Carpentry.Logic;

namespace Carpentry.Tools.QuickRestore
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var serviceProvider = BuildServiceProvider();

            var logger = serviceProvider.GetService<ILoggerFactory>().CreateLogger<Program>();

            logger.LogInformation("Carpentry QuickRestore - Initializing...");
            var appConfig = new BackupConfig(Configuration);

            var updateService = serviceProvider.GetService<IDataUpdateService>();

            var importService = serviceProvider.GetService<IDataImportService>();

            //Verify DBs & default records exist

            logger.LogInformation("Checking for default records");

            await updateService.ValidateDatabase();

            logger.LogInformation("Updating set definitions");

            //get collection of sets.
            await updateService.TryUpdateAvailableSets();

            logger.LogInformation("Validating import DTO");

            //validate the backup object
            var importDto = new CardImportDto()
            {
                ImportType = CardImportPayloadType.Carpentry,
                ImportPayload = appConfig.BackupDirectory,
            };

            ValidatedCarpentryImportDto validatedDto = await importService.ValidateCarpentryImport(importDto);

            logger.LogInformation($"Found {validatedDto.UntrackedSets.Count} untracked sets to add");

            //add all sets in the validated object
            for(int i = 0; i < validatedDto.UntrackedSets.Count; i++)
            {
                logger.LogInformation($"Adding tracking for {validatedDto.UntrackedSets[i].SetCode} [{i}/{validatedDto.UntrackedSets.Count}]");
                await updateService.AddTrackedSet(validatedDto.UntrackedSets[i].SetId);
            }

            //import the validated object
            await importService.AddValidatedCarpentryImport(validatedDto);

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

                .AddDbContext<ScryfallDataContext>(options => options.UseSqlServer(Configuration.GetConnectionString("ScryfallDataContext")))
                .AddDbContext<CarpentryDataContext>(options => options.UseSqlServer(Configuration.GetConnectionString("CarpentryDataContext")))

                .AddScoped<ISearchService, SearchService>()
                .AddScoped<IDeckService, DeckService>()
                .AddScoped<IInventoryService, InventoryService>()
                .AddScoped<IDataImportService, DataImportService>()
                .AddScoped<IDataUpdateService, DataUpdateService>()
                .AddScoped<IDataExportService, DataExportService>()
                .AddScoped<IFilterService, FilterService>()
                .AddScoped<IDataIntegrityService, DataIntegrityService>()
                .AddScoped<IScryfallService, ScryfallService>().AddHttpClient<IScryfallService, ScryfallService>().Services

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