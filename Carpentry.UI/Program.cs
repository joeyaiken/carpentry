using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
/*
 App thoughts:
	What if the UI layer models all had a DTO prefix - $"{Model}Dto"
		Controllers all exclusively send DTOs to/from the client app
		Ensures DB models don't get to the client app
		Have a goal of keeping UI models in-sync with ClientApp typings (someday generate typings from the models?)
     */

//I'm just going to use this as a general task section because it gets picked up by VS
//TODO - Implement all deck notes
//TODO - Add a list/table view to the card search (and others) to help streamline adding cards without a ton of scrolling
//TODO - Set a clear goal of when this refactor is complete
//TODO - Clean up old backups (Repositories)
//TODO - Clean up old backups (Text files)
//TODO - Clean up old backups (DB files)
//TODO consider implementing a config-based approach to serilog
//TODO add logging to everything
//TODO Consider moving more business logic to the controller layer
//TODO add multiple views to inventory
//  A view would be something like "search by set" or "web search"
//  In this case it'll include "By Quantity" "By Name" or "By Printing"
//TODO Implement loading indicators on all container components
//TODO - Refactor deck editor to match new design
//TODO - Actually implement use case of adding Cavalcade & Cat Oven
//TODO - New deck modal needs finished implementation

//TODO - Ensure inventory Total Count View can filter by set, then trim down fat packs

//TODO - Make sure all client-side containers contain 0 references to Material-UI
//  This helps with pushing visual stuff to functional components

//TODO - Sort cardSearch set list by release date (desc)

//TODO - Inventory should be able to sort / filter by price (and I guess a way of listing all variant / foil individually)

//TODO - Validate I have the expected # of copies of (that green sorcery that draws gates).  I think I pulled some from an existing deck, instead of buying more

//Some thoughts - I should only need one console app for backup/restore/migrations/imports: the Data Migration Service
//The actual Program.cs shouldn't contain any business logic, it should just spin everything up, pull out a specific service, and call a public script/routine/something
//Goal would to have individual public "BackupDatabase"/"RestoreDatabase"/"MigrateData"/"ImportDeckLists" methods
//what if I had a generic service interface with the expected public method?
//  IMigrationToolService

//TODO - Resolve those...ultimate masters?? cards that may have been mixed up
//TODO - add those rares in the end of the big binder


//TODO - implement a way of figuring out what cards to remove
//TODO - implement a way of searching for cards not owned
//          This could/should be accomplished by removing the Scry Repo (at least from this service),
//          and keeping a full set's definitions in the carpentry DB
//TODO - Remove ScryRepo from the Carpentry service, maintain an entire set's definition, not exclusively owned cards

namespace Carpentry.UI
{
    public class Program
    {
        //TODO - ensure serilog is working for this...
        private static IConfiguration Configuration
        {
            get
            {
                return new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json")
                    .AddEnvironmentVariables()
                    .Build();
            }
        }

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
