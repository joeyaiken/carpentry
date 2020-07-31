using Carpentry.Data.DataModels;
using Carpentry.Data.Interfaces;
using Carpentry.Data.QueryResults;
using Carpentry.Logic.Implementations;
using Carpentry.Logic.Interfaces;
using Carpentry.Logic.Models;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carpentry.Logic.Tests.UnitTests
{
    [TestClass]
    public class DeckServiceSearchTests
    {
        [TestMethod]
        public async Task DeckService_GetDeckOverviews_Test()
        {
            //Arrange
            List<DeckData> deckDbResult = new List<DeckData>()
            {
                new DeckData() { Id = 1, Format = new MagicFormatData(){ Id = 1 } },
                new DeckData() { Id = 2, Format = new MagicFormatData(){ Id = 1 } },
                new DeckData() { Id = 3, Format = new MagicFormatData(){ Id = 1 } },
                new DeckData() { Id = 4, Format = new MagicFormatData(){ Id = 1 } },
                new DeckData() { Id = 5, Format = new MagicFormatData(){ Id = 1 } },
            };

            DeckData deckDetailResult = new DeckData() { Id = 1, Format = new MagicFormatData() { Name = "Commander" } };

            var mockRepo = new Mock<IDeckDataRepo>(MockBehavior.Strict);

            int expectedDeckCardCountResult = 100;

            mockRepo
                .Setup(p => p.GetAllDecks())
                .ReturnsAsync(deckDbResult);

            mockRepo
                .Setup(p => p.GetDeckById(It.Is<int>(i => i > 0)))
                .ReturnsAsync(deckDetailResult);

            var mockLogger = new Mock<ILogger<DeckService>>(MockBehavior.Loose);

            var mockQueryService = new Mock<IDataQueryService>(MockBehavior.Strict);

            mockQueryService
                .Setup(p => p.GetDeckCardCount(It.Is<int>(i => i > 0)))
                .ReturnsAsync(expectedDeckCardCountResult);


            mockQueryService
                .Setup(p => p.GetDeckColorIdentity(It.Is<int>(i => i > 0)))
                .ReturnsAsync(new List<string>() { "U", "R", "G" });

            var mockInventoryService = new Mock<IInventoryService>(MockBehavior.Strict);

            var mockReferenceService = new Mock<IDataReferenceService>(MockBehavior.Strict);

            var deckService = new DeckService(mockRepo.Object, mockQueryService.Object, mockInventoryService.Object, mockLogger.Object, mockReferenceService.Object);

            //Act
            var result = await deckService.GetDeckOverviews();

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(deckDbResult.Count, result.Count());
            //Assert.Fail("want this to keep failing untill I have more conditions - like number of expected records"); //want this to keep failing
        }

        [TestMethod]
        public async Task DeckService_GetDeckDetail_Test()
        {
            //Arrange
            List<DeckCardResult> dbCards = new List<DeckCardResult>();

            DeckData expectedDbDeck = new DeckData()
            {
                Format = new MagicFormatData()
                {
                    Name = "standard"
                }
            };

            int idToRequest = 1;

            var mockDeckRepo = new Mock<IDeckDataRepo>(MockBehavior.Strict);

            mockDeckRepo
                .Setup(p => p.GetDeckById(It.Is<int>(i => i == idToRequest)))
                .ReturnsAsync(expectedDbDeck);

            var mockLogger = new Mock<ILogger<DeckService>>(MockBehavior.Loose);

            var mockQueryService = new Mock<IDataQueryService>(MockBehavior.Strict);

            mockQueryService
                .Setup(p => p.GetDeckCards(It.Is<int>(i => i == idToRequest)))
                .ReturnsAsync(dbCards);

            int expectedDeckCardCountResult = 60;

            mockQueryService
                .Setup(p => p.GetDeckCardCount(It.Is<int>(i => i == idToRequest)))
                .ReturnsAsync(expectedDeckCardCountResult);

            List<DeckCardStatResult> expectedStatsResult = new List<DeckCardStatResult>();

            mockQueryService
                .Setup(p => p.GetDeckCardStats(It.Is<int>(i => i == idToRequest)))
                .ReturnsAsync(expectedStatsResult);

            var mockInventoryService = new Mock<IInventoryService>(MockBehavior.Strict);

            var mockReferenceService = new Mock<IDataReferenceService>(MockBehavior.Strict);

            var deckService = new DeckService(mockDeckRepo.Object, mockQueryService.Object, mockInventoryService.Object, mockLogger.Object, mockReferenceService.Object);


            List<DeckPropertiesDto> expectedResults = new List<DeckPropertiesDto>();


            //Act
            var result = await deckService.GetDeckDetail(idToRequest);

            //Assert
            Assert.IsNotNull(result);
            //Assert.Fail("Must add more assert conditions");
        }

    }
}
