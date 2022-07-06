using Microsoft.AspNetCore.Hosting;
//using Carpentry.Data.LegacyDataContext;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
//using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Carpentry.Core;
//using Carpentry.Data.LegacyDataContextLegacy;
namespace Carpentry.Tests
{
    //class CarpentryAbstractFactory : WebApplicationFactory<IStartup>
    //{
    //    private static IConfiguration Configuration
    //    {
    //        get
    //        {
    //            return new ConfigurationBuilder()
    //            .SetBasePath(Directory.GetCurrentDirectory())
    //            //.AddJsonFile("appsettings.json", false, true)
    //            .AddJsonFile("appsettings.Development.json", false, true)
    //            //.AddUserSecrets<CarpentryFactory>()
    //            .AddEnvironmentVariables()
    //            .Build();
    //        }
    //    }

    //    protected override IWebHostBuilder CreateWebHostBuilder()
    //    {
    //        return WebHost.CreateDefaultBuilder()
    //            .UseConfiguration(Configuration)
    //            .UseStartup<Startup>();
    //    }

    //    protected override void ConfigureWebHost(IWebHostBuilder builder)
    //    {
    //        builder.UseContentRoot(".");
    //        base.ConfigureWebHost(builder);
    //    }
    //}


    public class CarpentryAbstractFactory<T> : WebApplicationFactory<T> where T : CarpentryStartupBase
    {
        //public CarpentryAbstractFactory<T>()
        

        private static IConfiguration Configuration
        {
            get
            {
                return new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                //.AddJsonFile("appsettings.json", false, true)
                .AddJsonFile("appsettings.Development.json", false, true)
                //.AddUserSecrets<CarpentryFactory>()
                .AddEnvironmentVariables()
                .Build();
            }
        }

        protected override IWebHostBuilder CreateWebHostBuilder()
        {
            return WebHost.CreateDefaultBuilder()
                .UseConfiguration(Configuration)
                .UseStartup<T>();
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseContentRoot(".");
            base.ConfigureWebHost(builder);
        }
    }

    class CarpentryFactory : WebApplicationFactory<Startup>
    {
        private static IConfiguration Configuration
        {
            get
            {
                return new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                //.AddJsonFile("appsettings.json", false, true)
                .AddJsonFile("appsettings.Development.json", false, true)
                //.AddUserSecrets<CarpentryFactory>()
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
            builder.UseContentRoot(".");
            base.ConfigureWebHost(builder);
        }
    }

    class CarpentryAngularFactory : WebApplicationFactory<Angular.Startup>
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
                .UseStartup<Angular.Startup>();
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseContentRoot(".");
            base.ConfigureWebHost(builder);
        }
    }
    
    // class CarpentryLegacyFactory : WebApplicationFactory<Legacy.Startup>
    // {
    //     private static IConfiguration Configuration
    //     {
    //         get
    //         {
    //             return new ConfigurationBuilder()
    //             .SetBasePath(Directory.GetCurrentDirectory())
    //             //.AddJsonFile("appsettings.json", false, true)
    //             .AddJsonFile("appsettings.Development.json", false, true)
    //             //.AddUserSecrets<CarpentryFactory>()
    //             .AddEnvironmentVariables()
    //             .Build();
    //         }
    //     }
    //
    //     protected override IWebHostBuilder CreateWebHostBuilder()
    //     {
    //         return WebHost.CreateDefaultBuilder()
    //             .UseConfiguration(Configuration)
    //             .UseStartup<Legacy.Startup>();
    //     }
    //
    //     protected override void ConfigureWebHost(IWebHostBuilder builder)
    //     {
    //         builder.UseContentRoot(".");
    //         base.ConfigureWebHost(builder);
    //     }
    // }

    interface IStartup
    {

    }
}
