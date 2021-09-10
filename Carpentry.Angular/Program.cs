using Carpentry.CarpentryData;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Carpentry.Angular
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var webHost = CreateWebHostBuilder(args).Build();
            await webHost.InitializeCarpentryDatabase();
            webHost.Run();
        }

        //public static IHostBuilder CreateHostBuilder(string[] args) =>
        //    Host.CreateDefaultBuilder(args)
        //        .ConfigureWebHostDefaults(webBuilder =>
        //        {
        //            webBuilder.UseStartup<Startup>();
        //        });

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
           WebHost.CreateDefaultBuilder(args)
            .UseStartup<Startup>();
    }

    public static class WebHostExtensions
    {
        public static async Task InitializeCarpentryDatabase(this IWebHost webHost)
        {
            //await Task.CompletedTask;

            var serviceScopeFactory = (IServiceScopeFactory)webHost.Services.GetService(typeof(IServiceScopeFactory));
            using (var scope = serviceScopeFactory.CreateScope())
            {
                var services = scope.ServiceProvider;
                var dbContext = services.GetRequiredService<CarpentryDataContext>();


                await dbContext.EnsureDatabaseCreated(false);

            }



            //var dbContext = webHost.Services.GetService<CarpentryDataContext>();

            //return webHost;
        }
    }
}
