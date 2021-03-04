using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

//I'm just going to use this as a general task section because it gets picked up by VS
//TODO - Add a list/table view to the card search (and others) to help streamline adding cards without a ton of scrolling

//TODO consider implementing a config-based approach to serilog
//TODO add logging to everything

//TODO Implement loading indicators on all container components

//TODO - Make sure all client-side containers contain 0 references to Material-UI
//  This helps with pushing visual stuff to functional components

//TODO - Validate I have the expected # of copies of (that green sorcery that draws gates).  I think I pulled some from an existing deck, instead of buying more

//TODO - Resolve those...ultimate masters?? cards that may have been mixed up
//TODO - add those rares in the end of the big binder

namespace Carpentry
{
    public class Program
    {
        //TODO - ensure serilog is working for this...
        //private static   Configuration
        //{
        //    get
        //    {
        //        return new ConfigurationBuilder()
        //            .SetBasePath(Directory.GetCurrentDirectory())
        //            .AddJsonFile("appsettings.json")
        //            .AddEnvironmentVariables()
        //            .Build();
        //    }
        //}

        public static void Main(string[] args)
        {
            //CreateHostBuilder(args).Build().Run();
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                //.ConfigureAppConfiguration((context, config) =>
                //{
                //    if (context.HostingEnvironment.IsDevelopment())
                //    {
                //        config.AddUserSecrets<Program>();
                //    }
                //})
                .UseStartup<Startup>();

        //public static IHostBuilder CreateHostBuilder(string[] args) =>
        //    Host.CreateDefaultBuilder(args)
        //        .ConfigureWebHostDefaults(webBuilder =>
        //        {
        //            webBuilder.UseStartup<Startup>();
        //        });
    }
}
