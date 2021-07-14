using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Carpentry.Logic.Models;
using System.Collections.Generic;
using Serilog;
using Serilog.Events;
using Microsoft.EntityFrameworkCore;
using Carpentry.ScryfallData;
using Carpentry.CarpentryData;
using Carpentry.Logic;

namespace Carpentry.Tools.QuickImport
{
    /// <summary>
    /// The Carpentry Import Tool is a simple console app that can load text lists of decks into the Carpentry database
    /// It will just use the Logic.ImportService to handle most of the leg work
    /// This program will handle the actual reading from text files
    /// It should be removed when the UI implementation of implementing a deck is complete
    /// </summary>
    class Program
    {
        static async Task Main(string[] args)
        {
            //var SAVE_DECKS = false;


            var serviceProvider = BuildServiceProvider();

            var logger = serviceProvider.GetService<ILoggerFactory>().CreateLogger<Program>();

            logger.LogInformation("Beginning import tool");

            var importService = serviceProvider.GetService<IDataImportService>();
            //var deckRepo = serviceProvider.GetService<DeckDataRepo>();

            var decksToImport = Get2ndJmpBox();

            if (decksToImport.Count == 0)
            {
                logger.LogInformation($"No decks specified, will now exit");
                return;
            }

            logger.LogInformation($"Will attempt to import {decksToImport.Count} decks");

            foreach (var deck in decksToImport)
            {
                logger.LogInformation($"Attempting to import deck {deck.Name}");

                //check if the deck already exists
                //var existingDeck = await deckRepo.GetDeckByName(deck.Name);
                //if(existingDeck != null)
                //{
                //    logger.LogInformation($"Deck {deck.Name} already exists in the database, skipping...");
                //    continue;
                //}


                string fileContents = await GetRawListFromFile(deck.FilePath);

                CardImportDto importPayload = new CardImportDto()
                {
                    ImportType = CardImportPayloadType.Arena,
                    ImportPayload = fileContents,
                };

                var validatedPayload = await importService.ValidateDeckImport(importPayload);
                validatedPayload.DeckProps.Name = deck.Name;
                validatedPayload.DeckProps.Format = deck.FormatName;

                if(validatedPayload.UntrackedSets.Count > 0)
                {
                    throw new Exception("Untracked set encountered (not automatically adding for now)");
                }

                if (!validatedPayload.IsValid)
                {
                    //throw new Exception("Invalid import, won't add");
                    logger.LogInformation($"Invalid import, won't add {deck.Name}");
                }

                //if (SAVE_DECKS)
                //{
                //await importService.AddValidatedDeckImport(validatedPayload);
                //}

                logger.LogInformation($"Successfully 'imported' deck {deck.Name}");
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

                ////DB context
                .AddDbContext<ScryfallDataContext>(options => options.UseSqlServer(Configuration.GetConnectionString("ScryfallDataContext")))
                .AddDbContext<CarpentryDataContext>(options => options.UseSqlServer(Configuration.GetConnectionString("CarpentryDataContext")))


            ////data services
            //.AddSingleton<ICardDataRepo, CardDataRepo>()
            ////.AddSingleton<IDeckDataRepo, DeckDataRepo>()
            ////.AddSingleton<IInventoryDataRepo, InventoryDataRepo>()
            ////.AddSingleton<IScryfallDataRepo, ScryfallRepo>()
            ////.AddSingleton<IDataReferenceService, DataReferenceService>()
            ////.AddSingleton<IDataReferenceRepo, DataReferenceRepo>()

            ////logic services
            ////.AddScoped<IDataRestoreService, DataRestoreService>()
            .AddSingleton<IDataUpdateService, DataUpdateService>()
            //.AddSingleton<IDataImportService, DataImportService>()

            //.AddScoped<IScryfallService, ScryfallService>()
            //.AddHttpClient<IScryfallService, ScryfallService>().Services



            ////private readonly IDataUpdateService _dataUpdateService;
            ////.AddScoped<>
            ////private readonly ICardDataRepo _cardDataRepo;
            ////private readonly IDeckService _deckService;
            ////private readonly IInventoryService _inventoryService;




            .AddSingleton<IDataBackupConfig, FakeAppConfig>()

            //Logic services
            //.AddScoped<ISearchService, SearchService>()
            .AddScoped<IDeckService, DeckService>()
            .AddScoped<IInventoryService, InventoryService>()

            .AddScoped<IDataImportService, DataImportService>()
            .AddScoped<IDataUpdateService, DataUpdateService>()
            //.AddScoped<IDataExportService, DataExportService>()
            //.AddScoped<IFilterService, FilterService>()
            .AddScoped<IDataIntegrityService, DataIntegrityService>()

            .AddScoped<IScryfallService, ScryfallService>()
            .AddHttpClient<IScryfallService, ScryfallService>().Services

            //.AddScoped<ICollectionBuilderService, CollectionBuilderService>()
            //.AddScoped<ITrimmingTipsService, TrimmingTipsService>()

            // ^


            //Service-layer
            //.AddScoped<ICarpentryCardSearchService, CarpentryCardSearchService>()
            //.AddScoped<ICarpentryCoreService, CarpentryCoreService>()
            //.AddScoped<ICarpentryDeckService, CarpentryDeckService>()
            //.AddScoped<ICarpentryInventoryService, CarpentryInventoryService>()


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

        private static List<DeckImportTemplate> Get2ndJmpBox()
        {
            var deckTemplates = new List<DeckImportTemplate>();

            var jumpstartDecks = new List<string>()
            {
                "Minotaurs_2",
                "Dragons_2",
                "Devilish_2",
                "Devilish_4",
                "Pirates_2",
                "Spirits_1",
                "Archaeology_1",
                "Archaeology_2",
                "Archaeology_3",
                "WellRead_2",
                "WellRead_3",
                "Walls",
                "Lands_1",
                "Predatory_1",
                "Dogs_1",
                "Dogs_2",
                "Doctor_3",
                "FeatheredFriends_1",
                "Legion_1",
                "Minions_4",
                "Vampires_1",
                "Vampires_3",
                "Vampires_4",
                "Rainbow",
            };

            foreach (var deck in jumpstartDecks)
            {
                deckTemplates.Add(GenerateJumpstartTemplate(deck));
            }

            return deckTemplates;
        }

        private static List<DeckImportTemplate> GetImports()
        {
            var deckTemplates = new List<DeckImportTemplate>();

            //var jumpstartDecks = new List<string>()
            //{
            //    "AboveTheClouds_4",
            //    "Archaeology_3",
            //    "Devilish_2",
            //    "Discarding_1",
            //    "Goblins_2",
            //    "HeavilyArmored_2",
            //    "HeavilyArmored_3",
            //    "Liliana",
            //    "Minions_2",
            //    "Minions_4",
            //    "PlusOne_1",
            //    "PlusOne_4",
            //    "Spellcasting_1",
            //    "Spirits_2",
            //    "Spooky_2",
            //    "Vampires_3",
            //    "WellRead_2",
            //    "WellRead_4",
            //    "Witchcraft_2",
            //    "Wizards_1",
            //    "Wizards_2",
            //};

            //var commanderDecks = new List<string>()
            //{
            //    "C21_LoreholdLegacies",
            //    "C21_QuantumQuandrix",
            //    "KLD_ElvenEmpire",
            //    "KLD_PhantomPremonition",
            //};

            //foreach(var deck in jumpstartDecks)
            //{
            //    deckTemplates.Add(GenerateJumpstartTemplate(deck));
            //}

            //foreach(var deck in commanderDecks)
            //{
            //    deckTemplates.Add(GenerateCommanderDeckTemplate(deck));
            //}

            return deckTemplates;
        }

        private static List<DeckImportTemplate> GetImports_Legacy()
        {
            List<DeckImportTemplate> decksToImport = new List<DeckImportTemplate>()
            {
                //new DeckImportTemplate()
                //{
                //    Name = "Well-Read 4",
                //    FormatName = "jumpstart",
                //    FilePath = "C:\\DotNet\\Carpentry\\Carpentry.Tools.QuickImport\\JMP\\WellRead_4.txt",
                //},
                //new DeckImportTemplate()
                //{
                //    Name = "Dogs 2",
                //    FormatName = "jumpstart",
                //    FilePath = "C:\\DotNet\\Carpentry\\Carpentry.Tools.QuickImport\\JMP\\Dogs_2.txt",
                //},
                //new DeckImportTemplate()
                //{
                //    Name = "Well-Read 3",
                //    FormatName = "jumpstart",
                //    FilePath = "C:\\DotNet\\Carpentry\\Carpentry.Tools.QuickImport\\JMP\\WellRead_3.txt",
                //},
            };


            List<DeckImportTemplate> moredecksToImport = new List<DeckImportTemplate>()
            {
                //new DeckImportTemplate()
                //{
                //    Name = "Goblins_3_1_jmp",
                //    FormatName = "jumpstart",
                //    FilePath = "C:\\DotNet\\Carpentry\\Carpentry.Tools.QuickImport\\JMP\\Goblins_3.txt",
                //},
                //new DeckImportTemplate()
                //{
                //    Name = "Predatory_2_jmp",
                //    FormatName = "jumpstart",
                //    FilePath = "C:\\DotNet\\Carpentry\\Carpentry.Tools.QuickImport\\JMP\\Predatory_2.txt",
                //},
                //new DeckImportTemplate()
                //{
                //    Name = "Legion_4_jmp",
                //    FormatName = "jumpstart",
                //    FilePath = "C:\\DotNet\\Carpentry\\Carpentry.Tools.QuickImport\\JMP\\Legion_4.txt",
                //},
                //new DeckImportTemplate()
                //{
                //    Name = "Elves_1_jmp",
                //    FormatName = "jumpstart",
                //    FilePath = "C:\\DotNet\\Carpentry\\Carpentry.Tools.QuickImport\\JMP\\Elves_1.txt",
                //},
                //new DeckImportTemplate()
                //{
                //    Name = "Basri_jmp",
                //    FormatName = "jumpstart",
                //    FilePath = "C:\\DotNet\\Carpentry\\Carpentry.Tools.QuickImport\\JMP\\Basri.txt",
                //},
                //new DeckImportTemplate()
                //{
                //    Name = "Predatory_4_jmp",
                //    FormatName = "jumpstart",
                //    FilePath = "C:\\DotNet\\Carpentry\\Carpentry.Tools.QuickImport\\JMP\\Predatory_4.txt",
                //},
                //new DeckImportTemplate()
                //{
                //    Name = "Goblins_3_2_jmp",
                //    FormatName = "jumpstart",
                //    FilePath = "C:\\DotNet\\Carpentry\\Carpentry.Tools.QuickImport\\JMP\\Goblins_3.txt",
                //},
                //new DeckImportTemplate()
                //{
                //    Name = "Archaeology_3_jmp",
                //    FormatName = "jumpstart",
                //    FilePath = "C:\\DotNet\\Carpentry\\Carpentry.Tools.QuickImport\\JMP\\Archaeology_3.txt",
                //},
                //new DeckImportTemplate()
                //{
                //    Name = "Lightning_1_jmp",
                //    FormatName = "jumpstart",
                //    FilePath = "C:\\DotNet\\Carpentry\\Carpentry.Tools.QuickImport\\JMP\\Lightning_1.txt",
                //},
                //new DeckImportTemplate()
                //{
                //    Name = "Cats_1_jmp",
                //    FormatName = "jumpstart",
                //    FilePath = "C:\\DotNet\\Carpentry\\Carpentry.Tools.QuickImport\\JMP\\Cats_1.txt",
                //},
                //new DeckImportTemplate()
                //{
                //    Name = "Legion_1_jmp",
                //    FormatName = "jumpstart",
                //    FilePath = "C:\\DotNet\\Carpentry\\Carpentry.Tools.QuickImport\\JMP\\Legion_1.txt",
                //},
                //new DeckImportTemplate()
                //{
                //    Name = "Vampires_4_jmp",
                //    FormatName = "jumpstart",
                //    FilePath = "C:\\DotNet\\Carpentry\\Carpentry.Tools.QuickImport\\JMP\\Vampires_4.txt",
                //},
                //new DeckImportTemplate()
                //{
                //    Name = "Devilish_2_jmp",
                //    FormatName = "jumpstart",
                //    FilePath = "C:\\DotNet\\Carpentry\\Carpentry.Tools.QuickImport\\JMP\\Devilish_2.txt",
                //},
                //new DeckImportTemplate()
                //{
                //    Name = "Spirits_1_jmp",
                //    FormatName = "jumpstart",
                //    FilePath = "C:\\DotNet\\Carpentry\\Carpentry.Tools.QuickImport\\JMP\\Spirits_1.txt",
                //},
            };

            List<DeckImportTemplate> otherDecksToImport = new List<DeckImportTemplate>()
            {
                //new DeckImportTemplate()
                //{
                //    Name = "Archaeology_3",
                //    FormatName = "jumpstart",
                //    FilePath = "C:\\DotNet\\Carpentry\\Carpentry.Tools.QuickImport\\JMP\\Archaeology_3.txt",
                //},
                //new DeckImportTemplate()
                //{
                //    Name = "Devilish_1",
                //    FormatName = "jumpstart",
                //    FilePath = "C:\\DotNet\\Carpentry\\Carpentry.Tools.QuickImport\\JMP\\Devilish_1.txt",
                //},
                //new DeckImportTemplate()
                //{
                //    Name = "PlusOne_4",
                //    FormatName = "jumpstart",
                //    FilePath = "C:\\DotNet\\Carpentry\\Carpentry.Tools.QuickImport\\JMP\\PlusOne_4.txt",
                //},
                //new DeckImportTemplate()
                //{
                //    Name = "Rogues_1",
                //    FormatName = "jumpstart",
                //    FilePath = "C:\\DotNet\\Carpentry\\Carpentry.Tools.QuickImport\\JMP\\Rogues_1.txt",
                //},
                //new DeckImportTemplate()
                //{
                //    Name = "Rogues_2",
                //    FormatName = "jumpstart",
                //    FilePath = "C:\\DotNet\\Carpentry\\Carpentry.Tools.QuickImport\\JMP\\Rogues_2.txt",
                //},
                //new DeckImportTemplate()
                //{
                //    Name = "Spooky_2",
                //    FormatName = "jumpstart",
                //    FilePath = "C:\\DotNet\\Carpentry\\Carpentry.Tools.QuickImport\\JMP\\Spooky_2.txt",
                //},
                //new DeckImportTemplate()
                //{
                //    Name = "TreeHugging_1",
                //    FormatName = "jumpstart",
                //    FilePath = "C:\\DotNet\\Carpentry\\Carpentry.Tools.QuickImport\\JMP\\TreeHugging_1.txt",
                //},

                //new DeckImportTemplate()
                //{
                //    Name = "Devilish (1)",
                //    FormatName = "jumpstart",
                //    FilePath = "C:\\DotNet\\Carpentry\\Carpentry.Tools.QuickImport\\JMP\\Devilish_1.txt",
                //},
                //new DeckImportTemplate()
                //{
                //    Name = "Goblins (1)",
                //    FormatName = "jumpstart",
                //    FilePath = "C:\\DotNet\\Carpentry\\Carpentry.Tools.QuickImport\\JMP\\Goblins_1.txt",
                //},
                //new DeckImportTemplate()
                //{
                //    Name = "Minions (3)",
                //    FormatName = "jumpstart",
                //    FilePath = "C:\\DotNet\\Carpentry\\Carpentry.Tools.QuickImport\\JMP\\Minions_3.txt",
                //},
                //new DeckImportTemplate()
                //{
                //    Name = "Minotaurs (1)",
                //    FormatName = "jumpstart",
                //    FilePath = "C:\\DotNet\\Carpentry\\Carpentry.Tools.QuickImport\\JMP\\Minotaurs_1.txt",
                //},
                //new DeckImportTemplate()
                //{
                //    Name = "Rogues (2)",
                //    FormatName = "jumpstart",
                //    FilePath = "C:\\DotNet\\Carpentry\\Carpentry.Tools.QuickImport\\JMP\\Rogues_2.txt",
                //},
                //new DeckImportTemplate()
                //{
                //    Name = "Spooky (4)",
                //    FormatName = "jumpstart",
                //    FilePath = "C:\\DotNet\\Carpentry\\Carpentry.Tools.QuickImport\\JMP\\Spooky_4.txt",
                //},
                //new DeckImportTemplate()
                //{
                //    Name = "Tree-Hugging (4)",
                //    FormatName = "jumpstart",
                //    FilePath = "C:\\DotNet\\Carpentry\\Carpentry.Tools.QuickImport\\JMP\\TreeHugging_4.txt",
                //},
                //new DeckImportTemplate()
                //{
                //    Name = "Wizards (2)",
                //    FormatName = "jumpstart",
                //    FilePath = "C:\\DotNet\\Carpentry\\Carpentry.Tools.QuickImport\\JMP\\Wizards_2.txt",
                //},
            };

            List<DeckImportTemplate> legacyDecksToImport = new List<DeckImportTemplate>()
            {
                //new DeckImportTemplate()
                //{
                //    Name = "Arm for Battle",
                //    FormatName = "commander",
                //    FilePath = "C:\\DotNet\\Carpentry\\Carpentry.Tools.QuickImport\\Imports\\CMR_ArmForBattle.txt",
                //},
                //new DeckImportTemplate()
                //{
                //    Name = "Reap the Tides",
                //    FormatName = "commander",
                //    FilePath = "C:\\DotNet\\Carpentry\\Carpentry.Tools.QuickImport\\Imports\\CMR_ReapTheTides.txt",
                //},
                //new DeckImportTemplate()
                //{
                //    Name = "Lands Wrath",
                //    FormatName = "commander",
                //    FilePath = "C:\\DotNet\\Carpentry\\Carpentry.Tools.QuickImport\\Imports\\ZNR_LandsWrath.txt",
                //},
                //new DeckImportTemplate()
                //{
                //    Name = "Sneak Attack",
                //    FormatName = "commander",
                //    FilePath = "C:\\DotNet\\Carpentry\\Carpentry.Tools.QuickImport\\Imports\\ZNR_SneakAttack.txt",
                //},

                //New EDH
                //new DeckImportTemplate()
                //{
                //    Name = "Arcane Maelstrom",
                //    FormatName = "commander",
                //    FilePath = "C:\\DotNet\\Carpentry\\Carpentry.Tools.QuickImport\\Imports\\ArcaneMaelstrom_EDH.txt",
                //},





                //4 JMP decks
                //new DeckImportTemplate()
                //{
                //    Name = "Enchanted (1)",
                //    FormatName = "jumpstart",
                //    FilePath = "C:\\DotNet\\Carpentry\\Carpentry.Tools.QuickImport\\Imports\\Enchanted1_JMP.txt",
                //},
                //new DeckImportTemplate()
                //{
                //    Name = "Witchcraft (2)",
                //    FormatName = "jumpstart",
                //    FilePath = "C:\\DotNet\\Carpentry\\Carpentry.Tools.QuickImport\\Imports\\Witchcraft2_JMP.txt",
                //},
                //new DeckImportTemplate()
                //{
                //    Name = "Vampires (2)",
                //    FormatName = "jumpstart",
                //    FilePath = "C:\\DotNet\\Carpentry\\Carpentry.Tools.QuickImport\\Imports\\Vampires2_JMP.txt",
                //},
                //new DeckImportTemplate()
                //{
                //    Name = "Ranimated (1)",
                //    FormatName = "jumpstart",
                //    FilePath = "C:\\DotNet\\Carpentry\\Carpentry.Tools.QuickImport\\Imports\\Ranimated1_JMP.txt",
                //},

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

            return decksToImport;
        }

        private static DeckImportTemplate GenerateCommanderDeckTemplate(string fileName)
        {
            string deckName = fileName.Split('_')[1];
            return new DeckImportTemplate()
            {
                Name = deckName,
                FormatName = "commander",
                FilePath = $"C:\\DotNet\\Carpentry\\Carpentry.Tools.QuickImport\\Imports\\{fileName}.txt",
            };
        }

        private static DeckImportTemplate GenerateJumpstartTemplate(string fileName)
        {
            return new DeckImportTemplate()
            {
                Name = fileName,
                FormatName = "jumpstart",
                FilePath = $"C:\\DotNet\\Carpentry\\Carpentry.Tools.QuickImport\\JMP\\{fileName}.txt",
            };
        }
    }

    public class DeckImportTemplate
    {
        public string Name { get; set; }
        public string FormatName { get; set; }
        public string FilePath { get; set; }
    }
}
