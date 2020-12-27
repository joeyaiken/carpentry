﻿using System.IO;
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
using Carpentry.Logic.Models;

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

            await updateService.EnsureDatabasesCreated();

            await updateService.EnsureDefaultRecordsExist();

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
                //.AddScoped<IDataRestoreService, DataRestoreService>()
                //.AddScoped<IDataUpdateService, DataUpdateService>()

                //.AddScoped<IScryfallService, ScryfallService>()
                //.AddHttpClient<IScryfallService, ScryfallService>().Services



                .AddDbContext<ScryfallDataContext>(options => options.UseSqlServer(Configuration.GetConnectionString("ScryfallDataContext")))

                //.AddDbContext<CarpentryDataContext>(options => options.UseSqlite($"Data Source={cardDatabaseFilepath}"))
                .AddDbContext<CarpentryDataContext>(options => options.UseSqlServer(Configuration.GetConnectionString("CarpentryDataContext")))


                //.AddSingleton<IDataBackupConfig, CarpentryAppConfig>()

                ////DB repos
                .AddScoped<ICardDataRepo, CardDataRepo>()
                .AddScoped<DeckDataRepo>()
                .AddScoped<IInventoryDataRepo, InventoryDataRepo>()
                .AddScoped<IScryfallDataRepo, ScryfallRepo>()
                .AddScoped<ICoreDataRepo, CoreDataRepo>()

                //Logic 6
                .AddScoped<ISearchService, SearchService>()
                .AddScoped<IDeckService, DeckService>()
                .AddScoped<IInventoryService, InventoryService>()

                .AddScoped<IDataImportService, DataImportService>()
                .AddScoped<IDataUpdateService, DataUpdateService>()
                .AddScoped<IDataExportService, DataExportService>()
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