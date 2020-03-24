using System;
using System.Threading.Tasks;
using Carpentry.Data.DataContext;
using Carpentry.Data.DataContextLegacy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;


namespace Carpentry.Data.BackupTool
{
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
                //database
                .AddDbContext<SqliteDataContext>(options => options.UseSqlite($"Data Source={dbFolderRoot}{dbName}.db"))

                //.AddDbContext<S9RepoContext>(options => options.UseSqlite($"Data Source={dbFolderRoot}{s9DbName}.db"))
                //s9 repo
                //.AddScoped<IS9CardRepo, S9CardRepo>()

                .AddScoped<DataBackupService>()

                .BuildServiceProvider();


            var logger = serviceProvider.GetService<ILoggerFactory>().CreateLogger<Program>();
            //logger.LogWarning("Starting application");

            logger.LogWarning("\n--------------------------------------------\nCarpentry Data Integrity Tool - Initializing\n--------------------------------------------");

            var migrationService = serviceProvider.GetService<DataBackupService>();

            migrationService.SaveDb();

            Log.CloseAndFlush();
            return;
        }
    }
}
