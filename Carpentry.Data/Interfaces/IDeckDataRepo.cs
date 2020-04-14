using Carpentry.Data.DataModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Carpentry.Data.Interfaces
{
    //This is one of the DataRepo classes that will communicate with the DB
    //TODO - Ensure methods of this interface don't return anything belonging to the Carpentry.DataModels namespace
    public interface IDeckDataRepo
    {
        Task<int> AddDeck(DeckData newDeck);

        Task UpdateDeck(DeckData deck);

        Task DeleteDeck(int deckId);



        Task AddDeckCard(DeckCardData deckCard);
        
        Task UpdateDeckCard(DeckCardData deckCard);

        Task DeleteDeckCard(int deckCardId);
        

        Task<DeckData> GetDeckById(int deckid);

        Task<DeckCardData> GetDeckCardById(int deckCardId);
        

    }
}
