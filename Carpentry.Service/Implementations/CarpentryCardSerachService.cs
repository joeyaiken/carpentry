using Carpentry.Data.QueryParameters;
using Carpentry.Logic.Interfaces;
using Carpentry.Logic.Models;
using Carpentry.Logic.Search;
using Carpentry.Service.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Carpentry.Service.Implementations
{
    public class CarpentryCardSearchService : ICarpentryCardSearchService
    {
        private readonly ISearchService _searchService;
        private readonly IScryfallService _scryService;
        
        public CarpentryCardSearchService(
            ISearchService searchService,
            IScryfallService scryService
            )
        {
            _searchService = searchService;
            _scryService = scryService;
        }

        #region Card Search related methods

        /// <summary>
        /// Returns a list of Card Search (overview) objects, taken from cards in the inventory
        /// </summary>
        /// <param name="filters"></param>
        /// <returns></returns>
        public async Task<List<CardSearchResultDto>> SearchInventory(CardSearchQueryParameter filters)
        {
            var result = await _searchService.SearchCardDefinitions(filters);
            return result;
        }

        public async Task<List<MagicCardDto>> SearchWeb(NameSearchQueryParameter filters)
        {
            //TODO - add some way of indicating that a card's set isn't tracked in the DB
            var result = await _scryService.SearchScryfallByName(filters.Name, filters.Exclusive);
            var mappedResult = result.Select(card => card.ToMagicCard()).ToList();
            return mappedResult;
        }

        #endregion
    }
}