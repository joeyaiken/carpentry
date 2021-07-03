using Carpentry.CarpentryData.Models;
using Carpentry.DataLogic.QueryResults;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Carpentry.DataLogic.Interfaces
{
    //This is one of the DataRepo classes that will communicate with the DB
    //TODO - Ensure methods of this interface don't return anything belonging to the Carpentry.DataModels namespace (WHY?)

    [Obsolete]
    public interface IDeckDataRepo
    {
        //Deck CRUD
        [Obsolete]
        Task<int> AddDeck(DeckData newDeck);
        [Obsolete]
        Task AddImportedDeckBatch(IEnumerable<DeckData> deckList);
        [Obsolete]
        Task UpdateDeck(DeckData deck);
        [Obsolete]
        Task DeleteDeck(int deckId);
        [Obsolete]
        Task<DeckData> GetDeckById(int deckid);
        [Obsolete]
        Task<DeckData> GetDeckByName(string deckName);
        [Obsolete]
        Task<IEnumerable<DeckData>> GetAllDecks();

        //Deck Card CRUD
        [Obsolete]
        Task AddDeckCard(DeckCardData deckCard);
        [Obsolete]
        Task UpdateDeckCard(DeckCardData deckCard);
        [Obsolete]
        Task DeleteDeckCard(int deckCardId);
        [Obsolete]
        Task<DeckCardData> GetDeckCardById(int deckCardId);
        [Obsolete]
        Task<DeckCardData> GetDeckCardByInventoryId(int inventoryCardId);

        //Queries
        [Obsolete]
        Task<List<DeckCardResult>> GetDeckCards(int deckId);
        [Obsolete]
        Task<int> GetDeckCardCount(int deckId);
        [Obsolete]
        Task<List<char>> GetDeckColorIdentity(int deckId);
        [Obsolete]
        Task<IEnumerable<DeckCardStatResult>> GetDeckCardStats(int deckId);
    }
}
