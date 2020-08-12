using Carpentry.Data.QueryParameters;
using Carpentry.Logic.Models;
using Carpentry.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Carpentry.Service.Implementations
{
    public class CarpentryCardSearchService : ICarpentryCardSearchService
    {
        //Should have no access to data context classes, only repo classes
        //private readonly ICardDataRepo _cardRepo;
        //private readonly IScryfallService _scryService;
        //private readonly IInventoryDataRepo _inventoryRepo;
        ////private readonly ICardStringRepo _scryRepo;
        ////private readonly ILogger<CarpentryService> _logger;
        //private readonly IInventoryDataRepo _inventoryRepo;

        public CarpentryCardSearchService(
            //IInventoryDataRepo inventoryRepo,
            //IScryfallService scryService

            //ICardDataRepo cardRepo
            //, ICardStringRepo scryRepo
            //, ILogger<CardSearchService> logger
            )
        {
            //_inventoryRepo = inventoryRepo;
            //_scryService = scryService;

            //_cardRepo = cardRepo;

            //_scryRepo = scryRepo;
            //_logger = logger;
        }

        #region Card Search related methods

        /// <summary>
        /// Returns a list of Card Search (overview) objects, taken from cards in the inventory
        /// </summary>
        /// <param name="filters"></param>
        /// <returns></returns>
        public async Task<IEnumerable<MagicCardDto>> SearchInventory(InventoryQueryParameter filters)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<MagicCardDto>> SearchWeb(NameSearchQueryParameter filters)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}