using Carpentry.UI;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Carpentry.LiveDataTests
{
    public class LiveDataTestFactory : WebApplicationFactory<Startup>
    {
        private static IConfiguration Configuration
        {
            get
            {
                return new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", false, true)
                //.AddUserSecrets<CarpentryFactory>()
                .AddEnvironmentVariables()
                .Build();
            }
        }

        protected override IWebHostBuilder CreateWebHostBuilder()
        {
            return WebHost.CreateDefaultBuilder()
                .UseConfiguration(Configuration) // Necessary for properly loading service endpoints
                                                 //.UseSerilog()
                .UseStartup<Startup>();
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            //ILogger logger = new LoggerConfiguration()
            //           .ReadFrom.Configuration(Configuration)
            //           .Enrich.FromLogContext()
            //           .CreateLogger();
            //Log.Logger = logger;


            builder.UseContentRoot(".");
            base.ConfigureWebHost(builder);
        }
    }
}
