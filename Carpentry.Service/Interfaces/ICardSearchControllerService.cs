using Carpentry.Service.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Carpentry.Service.Interfaces
{
    public interface ICardSearchControllerService
    {
        Task<IEnumerable<MagicCardDto>> SearchCardsFromInventory(InventoryQueryParameter filters);

        Task<IEnumerable<MagicCardDto>> SearchCardsFromSet(CardSearchQueryParameter filters);

        Task<IEnumerable<MagicCardDto>> SearchCardsFromWeb(NameSearchQueryParameter filters);
    }
}
