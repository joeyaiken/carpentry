using Carpentry.Data.LegacyModels;
using Carpentry.Data.QueryParameters;
using Carpentry.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Carpentry.Interfaces
{
    public interface ICarpentryService
    {
        Task EnsureCardDefinitionExists(int multiverseId);

        #region Deck related methods

        Task<int> AddDeck(DeckProperties props);

        Task UpdateDeck(DeckProperties props);

        Task DeleteDeck(int deckId);

        Task<IEnumerable<DeckProperties>> SearchDecks();

        Task<DeckDto> GetDeckDetail(int deckId);

        Task AddDeckCard(DeckCardDto dto);

        Task AddDeckCardBatch(IEnumerable<DeckCardDto> dto);

        Task UpdateDeckCard(DeckCardDto card);

        Task DeleteDeckCard(int deckCardId);

        #endregion

        #region Inventory related methods

        Task<int> AddInventoryCard(InventoryCardDto dto);

        Task AddInventoryCardBatch(IEnumerable<InventoryCardDto> cards);

        Task UpdateInventoryCard(InventoryCardDto dto);

        Task DeleteInventoryCard(int id);

        Task<IEnumerable<InventoryOverviewDto>> GetInventoryOverviews(InventoryQueryParameter param);

        Task<InventoryDetailDto> GetInventoryDetailByName(string name);

        #endregion

        #region Card Search related methods

        Task<IEnumerable<ScryfallMagicCard>> SearchCardsFromInventory(InventoryQueryParameter filters);

        Task<IEnumerable<ScryfallMagicCard>> SearchCardsFromSet(CardSearchQueryParameter filters);

        Task<IEnumerable<ScryfallMagicCard>> SearchCardsFromWeb(NameSearchQueryParameter filters);

        #endregion

        #region Core related methods

        Task<FilterOptionDto> GetAppFilterValues();

        #endregion
    }
}
