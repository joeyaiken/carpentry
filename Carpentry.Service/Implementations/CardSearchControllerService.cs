using Carpentry.Service.Interfaces;
using Carpentry.Service.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Carpentry.Service.Implementations
{
    public class CardSearchControllerService : ICardSearchControllerService
    {
        public CardSearchControllerService()
        {

        }

        public async Task<IEnumerable<MagicCardDto>> SearchCardsFromInventory(InventoryQueryParameter filters)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<MagicCardDto>> SearchCardsFromSet(CardSearchQueryParameter filters)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<MagicCardDto>> SearchCardsFromWeb(NameSearchQueryParameter filters)
        {
            throw new NotImplementedException();
        }
    }
}
