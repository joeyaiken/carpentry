using Carpentry.Data.LegacyDataContext;
using Carpentry.Data.LegacyModels;
using Carpentry.Data.Models;
using Carpentry.Data.QueryParameters;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Carpentry.Data.Interfaces
{
    //TODO - delete this
    public interface ILegacyCardRepo
    {

        Task<CardSet> GetSetByCode(string setCode);



        #region legacy

        #region util methods

        Task<MagicFormat> GetFormatByName(string formatName);
        
        
        
        
        
        #endregion

        #region Card related methdos

        Task AddCardDefinition(ScryfallMagicCard scryfallCard);

        Task UpdateCardDefinition(ScryfallMagicCard scryfallCard);

        IQueryable<LegacyDataContext.Card> QueryCardDefinitions();

        #endregion

        #region Deck related methods
        
        Task<int> AddDeck(DeckProperties props); //Obsolete
        
        Task<int> AddDeck(Deck newDeck);
        Task UpdateDeck(DeckProperties deckDto);

        Task DeleteDeck(int deckId);

        Task AddDeckCard(DeckCardDto dto);

        Task UpdateDeckCard(int deckCardId, int deckId, char? categoryId);

        Task DeleteDeckCard(int deckCardId);

        IQueryable<DeckProperties> QueryDeckProperties();

        IQueryable<DeckCard> QueryDeckCards();

        IQueryable<InventoryCard> QueryInventoryCardsForDeck(int deckId);

        IQueryable<InventoryCard> QueryInventoryCards();

        #endregion

        #region Inventory related methods

        Task<int> AddInventoryCard(InventoryCardDto dto);

        Task AddInventoryCardBatch(IEnumerable<InventoryCardDto> dtoBatch);

        Task UpdateInventoryCard(InventoryCardDto dto);

        Task DeleteInventoryCard(int id);

        Task<IQueryable<LegacyDataContext.Card>> QueryFilteredCards(InventoryQueryParameter filters);

        Task<IQueryable<InventoryOverviewDto>> QueryInventoryOverviews(InventoryQueryParameter filters);

        #endregion

        #region Core methods

        IQueryable<FilterDescriptor> QuerySetFilters();

        IQueryable<FilterDescriptor> QueryTypeFilters();

        IQueryable<FilterDescriptor> QueryFormatFilters();

        IQueryable<FilterDescriptor> QueryManaColorFilters();

        IQueryable<FilterDescriptor> QueryRarityFilters();

        IQueryable<FilterDescriptor> QueryCardStatusFilters();

        #endregion

        #endregion
    }
}
