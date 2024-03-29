﻿using Carpentry.CarpentryData;
using Carpentry.CarpentryData.Models;
using Carpentry.Logic.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carpentry.Logic.Tests
{
    [TestClass]
    public class DeckServiceTests : CarpentryServiceTestBase
    {
        protected override bool SeedViews => false;
        private DeckService _deckService;


        //private CarpentryDataContext CardContext;
        //private DbContextOptions<CarpentryDataContext> CardContextOptions;

        
        protected override void ResetContextChild()
        {
            //Should mocks be BeforeEach instead of ResetContext?
            var mockInventoryService = new Mock<IInventoryService>();
            var mockLogger = new Mock<ILogger<DeckService>>();
            
            _deckService = new DeckService(mockInventoryService.Object, mockLogger.Object, CardContext);
        }

        [TestInitialize]
        public async Task BeforeEach()
        {
            await BeforeEachBase();
        }

        [TestCleanup]
        public async Task AfterEach()
        {
            await AfterEachBase();
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

            var newDeck = CardContext.Decks.SingleAsync();

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
            CardContext.Add(StaticSeedData.Deck1);
            await CardContext.SaveChangesAsync();
            ResetContext();

            var updatePayload = new DeckPropertiesDto()
            {
                Id = 1,
                BasicW = 4,
                BasicR = 6,
                Format = "modern",
                Name = "testDeck",
            };

            await _deckService.UpdateDeck(updatePayload);
            ResetContext();

            var updatedDeck = await CardContext.Decks.SingleAsync();

            Assert.AreEqual(updatePayload.BasicW, updatedDeck.BasicW);
            Assert.AreEqual(updatePayload.BasicR, updatedDeck.BasicR);
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
            CardContext.Add(SeedData.StormCrowSet);
            CardContext.Add(SeedData.TripleThreatDeck);
            await CardContext.SaveChangesAsync();
            
            //TODO - get references to seed data IDs working
            var result = await _deckService.GetDeckDetail(1); //TODO - remove hardcoded int
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Cards.Count);
            Assert.AreEqual(3, result.Cards.Single().Count);
            //TODO - Remove hardcoded string
            Assert.AreEqual("Storm Crow", result.Cards.Single().Name);
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
