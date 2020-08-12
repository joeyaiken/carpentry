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
        //All methods should return a model specific to THIS project, not the data project (evevntually)

        //What if all data layer models were either
        //1 -   A DB/DataContext model
        //2 -   A DTO that contains either IDs or values but not the associations

        //Should have no access to data context classes, only repo classes
        //private readonly ILogger<DeckService> _logger;

        //private readonly IDeckDataRepo _deckRepo;

        //private readonly IInventoryService _inventoryService;

        //public ICoreDataRepo _coreDataRepo;

        //public ICardImportService _cardImportService;

        public CarpentryDeckService(
            //IDeckDataRepo deckRepo,
            //IInventoryService inventoryService,
            //ILogger<DeckService> logger,
            //ICoreDataRepo coreDataRepo,
            //ICardImportService cardImportService
            )
        {
            //_deckRepo = deckRepo;
            //_inventoryService = inventoryService;
            //_logger = logger;
            //_coreDataRepo = coreDataRepo;
            //_cardImportService = cardImportService;
        }

        #region Public methods

        #region Deck Props

        public async Task<int> AddDeck(DeckPropertiesDto props)
        {
            throw new NotImplementedException();
        }

        public async Task UpdateDeck(DeckPropertiesDto deckDto)
        {
            throw new NotImplementedException();
        }

        public async Task DeleteDeck(int deckId)
        {
            throw new NotImplementedException();
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
            throw new NotImplementedException();
        }

        //I think I only end up adding NEW deck cards with a batch
        //IDR where exactly this gets called outside of console apps
        public async Task AddDeckCardBatch(IEnumerable<DeckCardDto> dtoBatch)
        {
            throw new NotImplementedException();
        }

        public async Task UpdateDeckCard(DeckCardDto card)
        {
            throw new NotImplementedException();
        }

        public async Task DeleteDeckCard(int deckCardId)
        {
            throw new NotImplementedException();
        }

        #endregion Deck Cards

        #region Search

        public async Task<IEnumerable<DeckOverviewDto>> GetDeckOverviews()
        {
            throw new NotImplementedException();
        }

        //TODO - A DeckDTO shouldn't really contain an InventoryOverviewDto/InventoryCardDto,
        //it should contain a specific DeckDetail and DeckOverview DTO instead, that contains fields relevant to that container
        public async Task<DeckDetailDto> GetDeckDetail(int deckId)
        {
            throw new NotImplementedException();
        }

        #endregion Search

        #region Import / Export

        public async Task<ValidatedDeckImportDto> ValidateDeckImport(CardImportDto dto)
        {
            throw new NotImplementedException();
        }

        public async Task AddValidatedDeckImport(ValidatedDeckImportDto validatedDto)
        {
            throw new NotImplementedException();
        }

        public async Task<string> ExportDeckList(int deckId)
        {
            //This can't be implemented until I add Set Number to CardData, and properly track that in the DB
            throw new NotImplementedException();
        }

        #endregion Import / Export

        #endregion
    }
}