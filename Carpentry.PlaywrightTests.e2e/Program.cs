using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace Carpentry.PlaywrightTests.e2e
{
    /// <summary>
    /// TODO: Add a [better] general description for what this does (since it's the launching point for the app)
    /// Seeds a local sqlite DB with initial data (Might just be reference values, not other fake data)
    /// Runs a series of end-to-end tests to evaluate UI functionality
    /// The tests are designed to be ran against both the react and angular versions of the app
    /// </summary>
    class Program
    {
        static async Task Main(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            var services = new ServiceCollection();
            services.AddOptions();
            services.Configure<AppSettings>(configuration.GetSection("AppSettings"));
            services.AddScoped<TestSuite>();

            var provider = services.BuildServiceProvider();
            var testSuite = provider.GetService<TestSuite>();

            Console.WriteLine("TestSuite configured successfully.  Running tests in suite.");

            await testSuite.RunTestSuite();

            Console.WriteLine("TestSuite execution completed.");
        }
    }
}
