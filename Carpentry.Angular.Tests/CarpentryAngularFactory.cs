//using Carpentry.Data.LegacyDataContext;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace Carpentry.Angular.Tests
{
    class CarpentryAngularFactory : WebApplicationFactory<Startup>
    {
        private static IConfiguration Configuration
        {
            get
            {
                return new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.Development.json", false, true)
                .AddEnvironmentVariables()
                .Build();
            }
        }

        protected override IWebHostBuilder CreateWebHostBuilder()
        {
            return WebHost.CreateDefaultBuilder()
                .UseConfiguration(Configuration)
                .UseStartup<Startup>();
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services => { });
            builder.UseContentRoot(".");
            base.ConfigureWebHost(builder);
        }
    }
}
