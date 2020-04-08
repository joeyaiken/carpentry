using Carpentry.Data.QueryParameters;
using Carpentry.Logic.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Carpentry.Logic.Interfaces
{
    public interface ICardSearchService
    {
        Task<IEnumerable<MagicCard>> SearchCardsFromInventory(InventoryQueryParameter filters);

        Task<IEnumerable<MagicCard>> SearchCardsFromSet(CardSearchQueryParameter filters);

        Task<IEnumerable<MagicCard>> SearchCardsFromWeb(NameSearchQueryParameter filters);
    }
}
