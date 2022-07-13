using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;

namespace Carpentry.UI.Tests;

public class CarpentryFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
{
    // TODO - Play with this class and see what pieces are really necessary
    
    private static IConfiguration Configuration =>
        new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddEnvironmentVariables()
            .Build();

    protected override IWebHostBuilder CreateWebHostBuilder()
    {
        return WebHost.CreateDefaultBuilder()
            .UseConfiguration(Configuration)
            .UseStartup<TStartup>();
    }
    
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseContentRoot(".");
        base.ConfigureWebHost(builder);
    }
    
}