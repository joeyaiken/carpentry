using Carpentry.CarpentryData;
using Carpentry.CarpentryData.Models;
using Carpentry.Logic.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Carpentry.Logic.Tests
{
    [TestClass]
    public class DeckServiceTests : CarpentryServiceTestBase
    {
        private DeckService _deckService;
        private CarpentryDataContext _cardContext;
        private DbContextOptions<CarpentryDataContext> _cardContextOptions;

        //private ScryfallService _scryService;
        //private ScryfallDataContext _scryContext;
        //private DbContextOptions<ScryfallDataContext> _scryContextOptions;

        protected override void ResetContextChild()
        {
            var mockDbLogger = new Mock<ILogger<CarpentryDataContext>>();
            _cardContext = new CarpentryDataContext(_cardContextOptions, mockDbLogger.Object);

            var mockInventoryService = new Mock<IInventoryService>();
            var mockLogger = new Mock<ILogger<DeckService>>();
            
            _deckService = new DeckService(mockInventoryService.Object, mockLogger.Object, _cardContext);
        }

        protected override async Task BeforeEachChild()
        {
            _cardContextOptions = new DbContextOptionsBuilder<CarpentryDataContext>()
                .UseSqlite("Filename=CarpentryData.db").Options;
            ResetContext();
            await _cardContext.EnsureDatabaseCreated(false);
            ResetContext();
        }

        protected override async Task AfterEachChild()
        {
            await _cardContext.Database.EnsureDeletedAsync();
            await _cardContext.DisposeAsync();
        }

        //[TestMethod]

        #region Deck Props

        //public async Task<int> AddDeck(DeckPropertiesDto props)
        [TestMethod]
        public async Task AddDeck_Works_Test()
        {
            var expectedDeckId = 1;
            var deckToAdd = new DeckPropertiesDto()
            {
                Format = "commander"
            };

            var response = await _deckService.AddDeck(deckToAdd);

            ResetContext();

            var newDeck = _cardContext.Decks.SingleAsync();

            Assert.AreEqual(expectedDeckId, response);
            Assert.IsNotNull(newDeck);
            Assert.AreEqual(expectedDeckId, newDeck.Id);
        }

        //public async Task AddImportedDeckBatch(List<DeckPropertiesDto> decks)

        //public async Task UpdateDeck(DeckPropertiesDto deckDto)

        //public async Task DeleteDeck(int deckId)

        //public async Task DissassembleDeck(int deckId)

        //public async Task<int> CloneDeck(int deckId)

        #endregion Deck Props

        #region Deck Cards

        //public async Task AddDeckCard(DeckCardDto dto)

        //public async Task AddDeckCardBatch(IEnumerable<DeckCardDto> dtoBatch)

        //public async Task UpdateDeckCard(DeckCardDto card)

        //public async Task DeleteDeckCard(int deckCardId)

        #endregion Deck Cards

        #region Card Tags

        //public async Task<CardTagDetailDto> GetCardTagDetails(int deckId, int cardId)

        //public async Task AddCardTag(CardTagDto cardTag)

        //public async Task RemoveCardTag(int cardTagId)

        #endregion Card Tags

        #region Search

        //public async Task<List<DeckOverviewDto>> GetDeckOverviews(string format = null, string sortBy = null, bool includeDissasembled = false)

        //public async Task<DeckDetailDto> GetDeckDetail(int deckId)

        #endregion Search

        #region Import / Export

        //public async Task<string> GetDeckListExport(int deckId, string exportType)

        #endregion Import / Export

    }
}
