using System;
using System.Threading.Tasks;
using Carpentry.Data.DataContext;
using Carpentry.Data.DataContextLegacy;
using Carpentry.Data.Implementations;
using Carpentry.Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;

namespace CarpentryMigrationTool
{
    /// <summary>
    /// This console app manages the health of the Carpentry database
    /// Responsibilities include:
    ///     Ensuring all default records exist
    ///     Ensuring all scryfall data is up to date
    ///     Ensuring all prices are up to date
    ///     Ensuring no DB migrations need to be done
    ///     Ensuring any inventory changes are properlly backed up to a txt file
    /// </summary>
    class Program
    {
        static async Task Main(string[] args)
        {

            bool migrationRequired = false;
            bool backupRequired = false;
            bool restoreRequired = false;


            string dbName = "RefactorDev";
            string scryfallDbName = "ScryfallData";
            //string normalizedInventoryDbName = "InventoryData";
            string s9DbName = "S9Data";

            string dbFolderRoot = @"C:\DotNet\carpentry-refactor\Carpentry\";

            //var builder = new HostBuilder

            //var host = new HostBuilder()
            //.ConfigureHostConfiguration(x => {})

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Warning()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .CreateLogger();





            var serviceProvider = new ServiceCollection()
                .AddLogging(config => config.AddSerilog())
                //databases
                .AddDbContext<SqliteDataContext>(options => options.UseSqlite($"Data Source={dbFolderRoot}{dbName}.db"))
                .AddDbContext<ScryfallDataContext>(options => options.UseSqlite($"Data Source={dbFolderRoot}{scryfallDbName}.db"))
                .AddDbContext<LegacySqliteDataContext>(options => options.UseSqlite($"Data Source={dbFolderRoot}{s9DbName}.db"))
                //I keep forgetting I actually implemented the S9 database


            //.AddDbContext<S9RepoContext>(options => options.UseSqlite($"Data Source={dbFolderRoot}{s9DbName}.db"))
            //s9 repo
            //.AddScoped<IS9CardRepo, S9CardRepo>()

            //string repo
                .AddScoped<ICardStringRepo, ScryfallRepo>()
                .AddHttpClient<ICardStringRepo, ScryfallRepo>().Services

                //new & legacy DBs
                .AddScoped<ICardRepo, SqliteCardRepo>()


                .AddScoped<ImportService>()


                .AddScoped<DataMigrationService>()

                .AddHttpClient<DataMigrationService>().Services

                .BuildServiceProvider();


            var logger = serviceProvider.GetService<ILoggerFactory>().CreateLogger<Program>();
            //logger.LogWarning("Starting application");

            logger.LogWarning("\n--------------------------------------------\nCarpentry Data Integrity Tool - Initializing\n--------------------------------------------");




            var importService = serviceProvider.GetService<ImportService>();

            await importService.ImportDeckLists();



            Log.CloseAndFlush();
            return;


            var migrationService = serviceProvider.GetService<DataMigrationService>();
            //This SHOULD do a few things, in the following order

            //If scryfall data is present, update any sets in the scryfall DB

            //If DB versions are different (OR new DB doesn't exist at all)
            //  Ensure data is migrated successfully
            //  Don't attempt to clear anything, assume "new" db is fine

            //After a migration happens, should attempt to save to a text file
            //TODO - add this nonsense





















            //TODO: Requirement - Ensuring all scryfall data is up to date
            //Before I worry about running this again, I need to handle the fact that there are 3 instances of m13
            //await migrationService.UpdateScryfallData();


            //Need to start migrating things to the latest card repo

            //TODO: Requirement - Ensuring all default records exist

            //All of these DB sets should be populated somewhere along the way:


            #region reset button

            //if (true)
            //{
            //    migrationService.ClearDb();
            //}

            #endregion


            await migrationService.ForceUpdateScryfallSet("eld");

            




            #region Default Records

            //Rarities
            migrationService.EnsureDbRaritiesExist();

            //ManaTypes
            migrationService.EnsureDbManaTypesExist();

            //MagicFormats
            migrationService.EnsureDbMagicFormatsExist();

            //VariantTypes
            migrationService.EnsureDbVariantTypesExist();

            //CardStatuses
            migrationService.EnsureDbCardStatusesExist();

            #endregion

            #region Migrated Records

            //How the fuck do I migrate the rest of this??

            //Lets try this order:

            //Sets
            migrationService.MigrateLegacySets();

            //Cards, ColorIdentities, CardColors, CardLegalities, CardVariants
            migrationService.MigrateLegacyCards();

            //InventoryCards
            migrationService.MigrateLegacyInventoryCards();

            //Decks
            migrationService.MigrateLegacyDecks();

            //DeckCards
            migrationService.MigrateLegacyDeckCards();

            logger.LogWarning("\n------------\nSUCCESS!!!!!\n------------");

            #endregion


            Log.CloseAndFlush();

        }
    }
}
