using Carpentry.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Carpentry.Data.Interfaces
{
    public interface ILegacySqliteCardRepo
    {
        #region Deck related methods

        //Task<int> AddDeck(DeckProperties props);

        //Task UpdateDeck(DeckProperties props);

        Task DeleteDeck(int deckId);

        Task<IEnumerable<DeckProperties>> SearchDecks();

        Task<DeckDto> GetDeck(int deckId);

        //Task AddDeckCard(DeckCardDto dto);

        Task DeleteDeckCard(int deckCardId);

        #endregion

        #region Inventory related methods

        //Task<int> AddCard(CardDto dto);

        Task UpdateCard(CardDto dto);

        Task DeleteCard(int id);

        Task<List<InventoryQueryResult>> SearchInventory(InventoryQueryParameter param);

        #endregion

    }
}
