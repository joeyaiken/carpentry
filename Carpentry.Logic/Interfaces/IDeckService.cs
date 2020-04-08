using Carpentry.Logic.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Carpentry.Logic.Interfaces
{
    public interface IDeckService
    {
        Task<int> AddDeck(DeckProperties props);

        Task UpdateDeck(DeckProperties props);

        Task DeleteDeck(int deckId);

        Task<IEnumerable<DeckProperties>> SearchDecks();

        Task<DeckDetail> GetDeckDetail(int deckId);

        Task AddDeckCard(DeckCard dto);

        Task AddDeckCardBatch(IEnumerable<DeckCard> dto);

        Task UpdateDeckCard(DeckCard card);

        Task DeleteDeckCard(int deckCardId);
    }
}
