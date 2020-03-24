using System;
using System.Linq;
using Carpentry.Data.DataContext;
using Carpentry.Data.Interfaces;
using Carpentry.Data.MigrationTool.Models;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;

namespace Carpentry.Data.MigrationTool.Services
{
    //this class / service will update the price & legality information of cards
    //  involves both updating scryfall data, and updating card repo data
    public class DataRefreshService
    {

        private readonly ILogger<DataRefreshService> _logger;
        private readonly ICardRepo _cardRepo;
        private readonly ICardStringRepo _scryRepo;
        
        
        //readonly SqliteDataContext _cardContext;
        //readonly MigrationToolConfig _config;
        
        

        public DataRefreshService(
            ILoggerFactory loggerFactory,
            ICardRepo cardRepo,
            ICardStringRepo scryRepo
            )
        {
            _logger = loggerFactory.CreateLogger<DataRefreshService>();
            _cardRepo = cardRepo;
            _scryRepo = scryRepo;
        }

        //refreshes card pricing and legality data from ScryFall
        public void RefreshCardData()
        {
            _logger.LogInformation("DataRefreshService - RefreshCardData...");
            
            //First, refresh all scryfall data strings
            //  basically itterate over each scry set
            //  for each set, if it's out of date, update
            
            //  update includes both updating scryfall AND the matching Carpentry set
            
            
            
            
            
            //Then, parse scryfall cards from data set
            
            //Then, update Carpentry card data from Scryfall data

            _logger.LogInformation("DataRefreshService - RefreshCardData...completed successfully");
        }

    }
}
