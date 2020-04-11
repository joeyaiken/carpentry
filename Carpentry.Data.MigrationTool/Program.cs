using System;
using System.IO;
using System.Threading.Tasks;
using Carpentry.Data.LegacyDataContext;
using Carpentry.Data.Implementations;
using Carpentry.Data.Interfaces;
//using Carpentry.Data.MigrationTool.Services;
using Carpentry.Implementations;
using Carpentry.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Http;
using Serilog;
using Serilog.Events;


//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using Microsoft.AspNetCore;
//using Microsoft.AspNetCore.Hosting;


namespace Carpentry.Data.MigrationTool
{
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
            var appConfig = new MigrationToolConfig(Configuration);

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Warning()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .CreateLogger();


            //string cardDbDataSourceString = $"Data Source={appConfig.BackupSourceDbLocation}";
            string cardDbDataSourceString = $"Data Source={appConfig.RestoreDestinationCardDbLocation}";
            string scryDbDataSourceString = $"Data Source={appConfig.RestoreDestinationScryDbLocation}";

            var serviceProvider = new ServiceCollection()
                .AddSingleton(Configuration)
                .AddSingleton<MigrationToolConfig>()

                .AddLogging(config => config.AddSerilog())

                .AddDbContext<SqliteDataContext>(options => options.UseSqlite(cardDbDataSourceString))
                .AddDbContext<ScryfallDataContext>(options => options.UseSqlite(scryDbDataSourceString))



                .AddScoped<LegacyScryfallRepo>()
                .AddHttpClient<LegacyScryfallRepo>().Services

                .AddScoped<SqliteCardRepo>()

                .AddScoped<IDataMigrationService, DataMigrationService>()

                //.AddScoped<ICarpentryService, CarpentryService>()

                .BuildServiceProvider();

            /*
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
             
             
             */


            var logger = serviceProvider.GetService<ILoggerFactory>().CreateLogger<Program>();
            
            logger.LogWarning("\n--------------------------------------------\nCarpentry Data Integrity Tool - Initializing\n--------------------------------------------");

            var migrationService = serviceProvider.GetService<IDataMigrationService>();

            //await migrationService.RestoreDB();

            //await migrationService.RefreshDB();

            Log.CloseAndFlush();
            return;
        }
    }
}
