﻿using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using System.IO;
using Carpentry.Core;

namespace Carpentry.Tests
{
    public class CarpentryFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup : CarpentryStartupBase
    {
        private static IConfiguration Configuration
        {
            get
            {
                return new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddEnvironmentVariables()
                    .Build();
            }
        }

        protected override IWebHostBuilder CreateWebHostBuilder()
        {
            return WebHost.CreateDefaultBuilder()
                .UseConfiguration(Configuration)
                .UseStartup<TStartup>();
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            //ILogger logger = new LoggerConfiguration()
            //           .ReadFrom.Configuration(Configuration)
            //           .Enrich.FromLogContext()
            //           .CreateLogger();
            //Log.Logger = logger;

            //builder.ConfigureServices(services =>
            //{
            //    //Reminder - I need this in order for the service to properly see it
            //    //I'm not doing this just to try and override Startup that ALSO references the same AppSetting path
            //    //string cardDatabaseFilepath = Configuration.GetValue<string>("AppSettings:CardDatabaseFilepath");
            //    //string scryDatabaseFilepath = Configuration.GetValue<string>("AppSettings:ScryDatabaseFilepath");
            //    //string legacyDatabaseFilepath = Configuration.GetValue<string>("AppSettings:LegacyDatabaseFilepath");

            //    //services.AddDbContext<ScryfallDataContext>(options => options.UseSqlite($"Data Source={scryDatabaseFilepath}"));
            //    ////services.AddDbContext<LegacySqliteDataContext>(options => options.UseSqlite($"Data Source={legacyDatabaseFilepath}"));
            //    //services.AddDbContext<SqliteDataContext>(options => options.UseSqlite($"Data Source={cardDatabaseFilepath}"));

            //    //Only want this test service when running unit tests
            //    //services.AddScoped<TestDataService>();
            //    //services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            //});


            builder.UseContentRoot(".");
            base.ConfigureWebHost(builder);
        }
    }
}
