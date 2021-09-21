using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Events;
using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq;
using Carpentry.Logic.Models;
using Carpentry.CarpentryData;
using Carpentry.ScryfallData;
using Carpentry.Logic;

namespace Carpentry.Tools
{
    class Program
    {
        //TODO - move to AppConfig
        private static readonly int DB_UPDATE_INTERVAL_DAYS = 1; //Increase this if you only want to update data every few days

        static async Task Main(string[] args)
        {
            Console.WriteLine("Carpentry Quick Tools - Initializing");

            if (args.Length == 0) throw new NotImplementedException();
            
            else
            {
                var serviceProvider = BuildServiceProvider();
                var logger = serviceProvider.GetService<ILoggerFactory>().CreateLogger<Program>();
                var appConfig = new AppConfig(Configuration);

                var backupService = serviceProvider.GetService<IDataExportService>();
                var updateService = serviceProvider.GetService<IDataUpdateService>();
                var importService = serviceProvider.GetService<IDataImportService>();

                logger.LogInformation("Carpentry Quick Tools - Initialized Successfully");

                switch (args[0])
                {
                    case nameof(QuickToolEnum.QuickBackup):
                        logger.LogInformation("----------Carpentry Quick Backup Tool----------");
                        await backupService.BackupCollectionToDirectory(appConfig.BackupDirectory);
                        break;

                    case nameof(QuickToolEnum.QuickImport):
                        throw new NotImplementedException();

                    case nameof(QuickToolEnum.QuickRestore):
                        logger.LogInformation("Carpentry QuickRestore - Initializing...");
                        await QuickRestore(logger, appConfig, updateService, importService);
                        break;

                    case nameof(QuickToolEnum.QuickUpdate):
                        logger.LogInformation("Carpentry QuickUpdate - Initializing...");
                        await QuickUpdate(updateService, logger);
                        break;

                    default:
                        throw new NotImplementedException();
                }
            }
        }

        private static async Task QuickRestore(ILogger<Program> logger, AppConfig appConfig, 
            IDataUpdateService updateService, IDataImportService importService)
        {

            logger.LogInformation("Carpentry QuickRestore - Initializing...");
            //var appConfig = new AppConfig(Configuration);
            
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
            for (int i = 0; i < validatedDto.UntrackedSets.Count; i++)
            {
                logger.LogInformation($"Adding tracking for {validatedDto.UntrackedSets[i].SetCode} [{i}/{validatedDto.UntrackedSets.Count}]");
                await updateService.AddTrackedSet(validatedDto.UntrackedSets[i].SetId);
            }

            //import the validated object
            await importService.AddValidatedCarpentryImport(validatedDto);

            logger.LogInformation("Completed successfully");
        }

        private static async Task QuickUpdate(IDataUpdateService updateService, ILogger<Program> logger)
        {
            

            logger.LogInformation("Getting list of tracked set");

            await updateService.ValidateDatabase();

            var trackedSets = await updateService.GetTrackedSets(false, true);

            var setsToUpdate = trackedSets
                .Where(s => s.DataLastUpdated == null || s.DataLastUpdated.Value.AddDays(DB_UPDATE_INTERVAL_DAYS) < DateTime.Today.Date)
                .ToList();

            logger.LogInformation($"Found {setsToUpdate.Count} sets to update");


            for (int i = 0; i < setsToUpdate.Count; i++)
            {
                logger.LogInformation($"Updating {setsToUpdate[i].Code} [{i + 1}/{setsToUpdate.Count}]...");
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
                .MinimumLevel.Override("System.Net.Http", LogEventLevel.Warning)
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .CreateLogger();

            var serviceProvider = new ServiceCollection()
                .AddSingleton(Configuration)
                .AddSingleton<IDataBackupConfig, AppConfig>()

                .AddLogging(config => config.AddSerilog())

                .AddDbContext<ScryfallDataContext>(options => options.UseSqlServer(Configuration.GetConnectionString("ScryfallDataContext")))
                .AddDbContext<CarpentryDataContext>(options => options.UseSqlServer(Configuration.GetConnectionString("CarpentryDataContext")))

                //logic services
                .AddScoped<IDataImportService, DataImportService>()
                .AddScoped<IDataExportService, DataExportService>()
                .AddScoped<IDataUpdateService, DataUpdateService>()
                .AddScoped<IScryfallService, ScryfallService>().AddHttpClient<IScryfallService, ScryfallService>().Services
                .AddScoped<IDataIntegrityService, DataIntegrityService>()
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

    enum QuickToolEnum
    {
        QuickBackup,
        QuickImport,
        QuickRestore,
        QuickUpdate
    }

    class AppConfig : IDataBackupConfig
    {
        public AppConfig(IConfiguration appConfig)
        {
            BackupDirectory = appConfig.GetValue<string>("AppSettings:BackupFolderPath");
            DeckBackupFilename = appConfig.GetValue<string>("AppSettings:DeckBackupFilename");
            CardBackupFilename = appConfig.GetValue<string>("AppSettings:CardBackupFilename");
            PropsBackupFilename = appConfig.GetValue<string>("AppSettings:PropsBackupFilename");
        }
        public string BackupDirectory { get; set; }
        public string DeckBackupFilename { get; set; }
        public string CardBackupFilename { get; set; }
        public string PropsBackupFilename { get; set; }
    }
}
