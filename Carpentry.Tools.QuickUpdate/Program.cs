using System;

using System.IO;
using System.Threading.Tasks;
//using Carpentry.Data.DataContext;
//using Carpentry.Data.Implementations;
//using Carpentry.Data.Interfaces;
//using Carpentry.Implementations;
//using Carpentry.Interfaces;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.Extensions.Configuration;
//using Microsoft.Extensions.DependencyInjection;
//using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Http;
//using Serilog;
//using Serilog.Events;

namespace Carpentry.Tools.QuickUpdate
{
    class Program
    {



        static async Task Main(string[] args)
        {
            Console.WriteLine("Hello World!");
        }



        private void BuildServiceProvider()
        {
            //var appConfig = new MigrationToolConfig(Configuration);

            //Log.Logger = new LoggerConfiguration()
            //    .MinimumLevel.Warning()
            //    .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
            //    .Enrich.FromLogContext()
            //    .WriteTo.Console()
            //    .CreateLogger();


            //string cardDbDataSourceString = $"Data Source={appConfig.BackupSourceDbLocation}";
            //string cardDbDataSourceString = $"Data Source={appConfig.RestoreDestinationCardDbLocation}";

            //string scryDbDataSourceString = $"Data Source={appConfig.RestoreDestinationScryDbLocation}";


            //var serviceProvider = new ServiceCollection()
            //    .AddSingleton(Configuration)
            //    .AddSingleton<MigrationToolConfig>()

            //    .AddLogging(config => config.AddSerilog())

            //    .AddDbContext<SqliteDataContext>(options => options.UseSqlite(cardDbDataSourceString))
            //    .AddDbContext<ScryfallDataContext>(options => options.UseSqlite(scryDbDataSourceString))



            //    .AddScoped<ScryfallRepo>()
            //    .AddHttpClient<ScryfallRepo>().Services

            //    .AddScoped<SqliteCardRepo>()

            //    .AddScoped<IDataMigrationService, DataMigrationService>()

            //    //.AddScoped<ICarpentryService, CarpentryService>()

            //    .BuildServiceProvider();

        }

        //private static IConfiguration Configuration
        //{
        //    get
        //    {
        //        return new ConfigurationBuilder()
        //            .SetBasePath(Directory.GetCurrentDirectory())
        //            .AddJsonFile("appsettings.json")
        //            .Build();
        //    }
        //}
    }
}
