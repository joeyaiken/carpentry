using Carpentry.CarpentryData.Models;
using Carpentry.DataLogic.QueryResults;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Carpentry.DataLogic.Interfaces
{
    //This is one of the DataRepo classes that will communicate with the DB
    //TODO - Ensure methods of this interface don't return anything belonging to the Carpentry.DataModels namespace (WHY?)
    public interface IDeckDataRepo
    {
        //Deck CRUD
        Task<int> AddDeck(DeckData newDeck);
        Task AddImportedDeckBatch(IEnumerable<DeckData> deckList);
        Task UpdateDeck(DeckData deck);
        Task DeleteDeck(int deckId);
        Task<DeckData> GetDeckById(int deckid);
        Task<DeckData> GetDeckByName(string deckName);
        Task<IEnumerable<DeckData>> GetAllDecks();

        //Deck Card CRUD
        Task AddDeckCard(DeckCardData deckCard);
        Task UpdateDeckCard(DeckCardData deckCard);
        Task DeleteDeckCard(int deckCardId);
        Task<DeckCardData> GetDeckCardById(int deckCardId);
        Task<DeckCardData> GetDeckCardByInventoryId(int inventoryCardId);

        //Queries
        Task<List<DeckCardResult>> GetDeckCards(int deckId);
        Task<int> GetDeckCardCount(int deckId);
        Task<List<char>> GetDeckColorIdentity(int deckId);
        Task<IEnumerable<DeckCardStatResult>> GetDeckCardStats(int deckId);
    }
}
