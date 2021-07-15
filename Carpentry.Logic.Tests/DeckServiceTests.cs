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

        #region Deck Props

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

        [TestMethod]
        public async Task AddImportedDeckBatch_Works_Test()
        {
            //public async Task AddImportedDeckBatch(List<DeckPropertiesDto> decks)
            Assert.Fail("Test not implemented");
        }

        [TestMethod]
        public async Task UpdateDeck_Works_Test()
        {
            //public async Task UpdateDeck(DeckPropertiesDto deckDto)
            Assert.Fail("Test not implemented");
        }

        [TestMethod]
        public async Task DeleteDeck_Works_Test()
        {
            //public async Task DeleteDeck(int deckId)
            Assert.Fail("Test not implemented");
        }

        [TestMethod]
        public async Task DissassembleDeck_Works_Test()
        {
            //public async Task DissassembleDeck(int deckId)
            Assert.Fail("Test not implemented");
        }

        [TestMethod]
        public async Task CloneDeck_Works_Test()
        {
            //public async Task<int> CloneDeck(int deckId)
            Assert.Fail("Test not implemented");
        }

        #endregion Deck Props

        #region Deck Cards

        [TestMethod]
        public async Task AddDeckCard_Works_Test()
        {
            //public async Task AddDeckCard(DeckCardDto dto)
            Assert.Fail("Test not implemented");
        }

        [TestMethod]
        public async Task AddDeckCardBatch_Works_Test()
        {
            //public async Task AddDeckCardBatch(IEnumerable<DeckCardDto> dtoBatch)
            Assert.Fail("Test not implemented");
        }

        [TestMethod]
        public async Task UpdateDeckCard_Works_Test()
        {
            //public async Task UpdateDeckCard(DeckCardDto card)
            Assert.Fail("Test not implemented");
        }

        [TestMethod]
        public async Task DeleteDeckCard_Works_Test()
        {
            //public async Task DeleteDeckCard(int deckCardId)
            Assert.Fail("Test not implemented");
        }

        #endregion Deck Cards

        #region Card Tags

        [TestMethod]
        public async Task GetCardTagDetails_Works_Test()
        {
            //public async Task<CardTagDetailDto> GetCardTagDetails(int deckId, int cardId)
            Assert.Fail("Test not implemented");
        }

        [TestMethod]
        public async Task AddCardTag_Works_Test()
        {
            //public async Task AddCardTag(CardTagDto cardTag)
            Assert.Fail("Test not implemented");
        }

        [TestMethod]
        public async Task RemoveCardTag_Works_Test()
        {
            //public async Task RemoveCardTag(int cardTagId)
            Assert.Fail("Test not implemented");
        }

        #endregion Card Tags

        #region Search

        [TestMethod]
        public async Task GetDeckOverviews_Works_Test()
        {
            //public async Task<List<DeckOverviewDto>> GetDeckOverviews(string format = null, string sortBy = null, bool includeDissasembled = false)
            Assert.Fail("Test not implemented");
        }

        [TestMethod]
        public async Task GetDeckDetail_Works_Test()
        {
            //public async Task<DeckDetailDto> GetDeckDetail(int deckId)
            Assert.Fail("Test not implemented");
        }

        #endregion Search

        #region Import / Export

        [TestMethod]
        public async Task GetDeckListExport_Works_Test()
        {
            //public async Task<string> GetDeckListExport(int deckId, string exportType)
            Assert.Fail("Test not implemented");
        }

        #endregion Import / Export
    }
}
