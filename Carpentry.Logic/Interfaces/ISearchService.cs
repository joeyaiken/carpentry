using Carpentry.Data.QueryParameters;
using Carpentry.Logic.Models;
using Carpentry.Logic.Search;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Carpentry.Logic.Interfaces
{
    public interface ISearchService
    {
        //SearchInventory
        Task<List<InventoryOverviewDto>> SearchInventory(InventoryQueryParameter param);

        //SearchCards
        Task<List<CardSearchResultDto>> SearchCards(CardSearchQueryParameter filters);
        

        //Task<IEnumerable<MagicCardDto>> SearchCardsFromInventory(InventoryQueryParameter filters);

        //Task<IEnumerable<MagicCardDto>> SearchCardsFromSet(CardSearchQueryParameter filters);

        //Task<IEnumerable<MagicCardDto>> SearchCardsFromWeb(NameSearchQueryParameter filters);
    }
}
