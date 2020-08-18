using Carpentry.Data.QueryParameters;
using Carpentry.Logic.Models;
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
        Task<List<MagicCardDto>> SearchCards(InventoryQueryParameter filters);
        

        //Task<IEnumerable<MagicCardDto>> SearchCardsFromInventory(InventoryQueryParameter filters);

        //Task<IEnumerable<MagicCardDto>> SearchCardsFromSet(CardSearchQueryParameter filters);

        //Task<IEnumerable<MagicCardDto>> SearchCardsFromWeb(NameSearchQueryParameter filters);
    }
}
