using Carpentry.Logic.Interfaces;
using Carpentry.Logic.Models;
using Carpentry.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Carpentry.Service.Implementations
{
    public class CarpentryDeckService : ICarpentryDeckService
    {        
        //Should have no access to data context classes, only repo classes
        private readonly IDeckService _deckService;
        private readonly ICardImportService _cardImportService;
        private readonly IDataExportService _exportService;

        public CarpentryDeckService(
            IDeckService deckService,
            ICardImportService cardImportService,
            IDataExportService exportService
            )
        {
            _deckService = deckService;
            _cardImportService = cardImportService;
            _exportService = exportService;
        }

        #region Public methods

        #region Deck Props

        public async Task<int> AddDeck(DeckPropertiesDto props)
        {
            var result = await _deckService.AddDeck(props);
            return result;
        }

        public async Task UpdateDeck(DeckPropertiesDto deckDto)
        {
            await _deckService.UpdateDeck(deckDto);
        }

        public async Task DeleteDeck(int deckId)
        {
            await _deckService.DeleteDeck(deckId);
        }

        #endregion Deck Props

        #region Deck Cards

        /// <summary>
        /// Adds a card to a deck
        /// If the dto references an existing deck card, and that card is ALREADY in a deck, no new card is added
        ///     Instead, the existing card is moved to this deck
        /// If the card exists, but isn't in a deck, then a new deck card is created
        /// If the inventory card doesn't exist, then a new one is mapped
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task AddDeckCard(DeckCardDto dto)
        {
            await _deckService.AddDeckCard(dto);
        }

        //I think I only end up adding NEW deck cards with a batch
        //IDR where exactly this gets called outside of console apps
        public async Task AddDeckCardBatch(IEnumerable<DeckCardDto> dtoBatch)
        {
            await _deckService.AddDeckCardBatch(dtoBatch);
        }

        public async Task UpdateDeckCard(DeckCardDto card)
        {
            await _deckService.UpdateDeckCard(card);
        }

        public async Task DeleteDeckCard(int deckCardId)
        {
            await _deckService.DeleteDeckCard(deckCardId);
        }

        #endregion Deck Cards

        #region Search

        public async Task<List<DeckOverviewDto>> GetDeckOverviews()
        {
            var result = await _deckService.GetDeckOverviews();
            return result;
        }

        //TODO - A DeckDTO shouldn't really contain an InventoryOverviewDto/InventoryCardDto,
        //it should contain a specific DeckDetail and DeckOverview DTO instead, that contains fields relevant to that container
        public async Task<DeckDetailDto> GetDeckDetail(int deckId)
        {
            var result = await _deckService.GetDeckDetail(deckId);
            return result;
        }

        #endregion Search

        #region Import / Export

        public async Task<ValidatedDeckImportDto> ValidateDeckImport(CardImportDto dto)
        {
            var validatedResult = await _cardImportService.ValidateDeckImport(dto);
            return validatedResult;
        }

        public async Task AddValidatedDeckImport(ValidatedDeckImportDto validatedDto)
        {
            await _cardImportService.AddValidatedDeckImport(validatedDto);
        }

        public async Task<string> ExportDeckList(int deckId)
        {
            var result = await _exportService.GetDeckListExport(deckId);
            return result;
        }

        #endregion Import / Export

        #endregion
    }
}