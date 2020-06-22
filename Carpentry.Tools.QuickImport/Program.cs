using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Carpentry.Logic.Interfaces;
using Carpentry.Logic.Implementations;
using Carpentry.Logic.Models;
using System.Collections.Generic;
using Serilog;
using Serilog.Events;
using Carpentry.Data.DataContext;
using Carpentry.Data.Interfaces;
using Carpentry.Data.Implementations;
using Microsoft.EntityFrameworkCore;

namespace Carpentry.Tools.QuickImport
{
    /// <summary>
    /// The Carpentry Import Tool is a simple console app that can load text lists of decks into the Carpentry database
    /// It will just use the Logic.ImportService to handle most of the leg work
    /// This program will handle the actual reading from text files
    /// </summary>
    class Program
    {
        static async Task Main(string[] args)
        {
            var serviceProvider = BuildServiceProvider();

            var logger = serviceProvider.GetService<ILoggerFactory>().CreateLogger<Program>();

            logger.LogInformation("Beginning import tool");

            var importService = serviceProvider.GetService<ICardImportService>();

            List<DeckImportTemplate> decksToImport = new List<DeckImportTemplate>()
            {
                ////Populate EDH deck:
                //new DeckImportTemplate
                //{
                //    Name = "Primal Genesis",
                //    FormatName = "commander",
                //    FilePath = "C:\\DotNet\\carpentry-refactor\\CarpentryMigrationTool\\Imports\\Populate.txt"
                //},
                ////Knights brawl deck
                //new DeckImportTemplate
                //{
                //    Name = "Knight's Charge",
                //    FormatName = "brawl",
                //    FilePath = "C:\\DotNet\\carpentry-refactor\\CarpentryMigrationTool\\Imports\\Knight.txt"
                //},
                ////Chulane brawl deck
                //new DeckImportTemplate
                //{
                //    Name = "Wild Bounty",
                //    FormatName = "brawl",
                //    FilePath = "C:\\DotNet\\carpentry-refactor\\CarpentryMigrationTool\\Imports\\Chulane.txt"
                //},
                ////Draggo brawl deck
                //new DeckImportTemplate
                //{
                //    Name = "Savage Hunter 2",
                //    FormatName = "brawl",
                //    FilePath = "C:\\DotNet\\carpentry-refactor\\CarpentryMigrationTool\\Imports\\Draggo.txt"
                //},
            };

            if(decksToImport.Count == 0)
            {
                logger.LogInformation($"No decks specified, will now exit");
                return;
            }

            logger.LogInformation($"Will attempt to import {decksToImport.Count} decks");

            foreach(var deck in decksToImport)
            {
                logger.LogInformation($"Attempting to import deck {deck.Name}");
                string fileContents = await GetRawListFromFile(deck.FilePath);

                CardImportDto importPayload = new CardImportDto()
                {
                    ImportType = CardImportPayloadType.Arena,
                    ImportPayload = fileContents,
                };

                var validatedPayload = await importService.ValidateImport(importPayload);
                validatedPayload.DeckProps.Name = deck.Name;
                validatedPayload.DeckProps.Format = deck.FormatName;

                await importService.AddValidatedImport(validatedPayload);

                logger.LogInformation($"Successfully imported deck {deck.Name}");
            }
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

                //DB context
                .AddDbContext<ScryfallDataContext>(options => options.UseSqlServer(Configuration.GetConnectionString("ScryfallDataContext")))
                .AddDbContext<CarpentryDataContext>(options => options.UseSqlServer(Configuration.GetConnectionString("CarpentryDataContext")))



                //data services
                .AddSingleton<ICardDataRepo, CardDataRepo>()
                //.AddSingleton<IDeckDataRepo, DeckDataRepo>()
                //.AddSingleton<IInventoryDataRepo, InventoryDataRepo>()
                //.AddSingleton<IScryfallDataRepo, ScryfallRepo>()
                //.AddSingleton<IDataReferenceService, DataReferenceService>()
                //.AddSingleton<IDataReferenceRepo, DataReferenceRepo>()

                //logic services
                //.AddScoped<IDataRestoreService, DataRestoreService>()
                .AddSingleton<IDataUpdateService, DataUpdateService>()
                .AddSingleton<ICardImportService, CardImportService>()

                .AddScoped<IScryfallService, ScryfallService>()
                .AddHttpClient<IScryfallService, ScryfallService>().Services

                

                //private readonly IDataUpdateService _dataUpdateService;
                //.AddScoped<>
                //private readonly ICardDataRepo _cardDataRepo;
                //private readonly IDeckService _deckService;
                //private readonly IInventoryService _inventoryService;

                //TODO - verify that this starts (has the right services specified here)
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

        private static async Task<string> GetRawListFromFile(string directory)
        {
            string fileContents = await File.ReadAllTextAsync(directory);
            return fileContents;
        }
    }

    public class DeckImportTemplate
    {
        public string Name { get; set; }
        public string FormatName { get; set; }
        public string FilePath { get; set; }
    }
}
