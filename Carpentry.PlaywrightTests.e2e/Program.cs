using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Carpentry.PlaywrightTests.e2e.Tests;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;
using Serilog.AspNetCore;

namespace Carpentry.PlaywrightTests.e2e
{
    /// <summary>
    /// TODO: Add a [better] general description for what this does (since it's the launching point for the app)
    /// Seeds a local sqlite DB with initial data (Might just be reference values, not other fake data)
    /// Runs a series of end-to-end tests to evaluate UI functionality
    /// The tests are designed to be ran against both the react and angular versions of the app
    /// </summary>
    static class Program
    {
        static async Task Main(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .MinimumLevel.Override("Microsoft.EntityFrameworkCore", LogEventLevel.Warning)
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .CreateLogger();

            var services = new ServiceCollection();
            services.AddOptions();
            services.Configure<AppSettings>(configuration.GetSection("AppSettings"));
            services.AddLogging(config => config.AddSerilog());
            services.AddSingleton<TestSuite>();
            services.AddSingleton(Log.Logger);

            var provider = services.BuildServiceProvider();
            var logger = provider.GetService<ILogger>();
            var testSuite = provider.GetService<TestSuite>();
            
            logger.Information("Carpentry Playwright e2e tests implemented successfully.  Beginning test execution");
            await testSuite.RunTestSuite();
            logger.Information("TestSuite execution completed.");
        }
    }
}
