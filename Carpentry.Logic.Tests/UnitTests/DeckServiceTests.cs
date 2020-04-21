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
    public class DeckServiceTests
    {
        [TestMethod]
        public async Task DeckService_AddDeck_Test()
        {
            //Arrange
            int mockDeckId = 1;

            var mockRepo = new Mock<IDeckDataRepo>(MockBehavior.Strict);

            mockRepo
                .Setup(p => p.AddDeck(It.IsNotNull<DeckData>()))
                .ReturnsAsync(mockDeckId);

            var mockLogger = new Mock<ILogger<DeckService>>(MockBehavior.Loose);

            var mockQueryService = new Mock<IDataQueryService>(MockBehavior.Strict);

            var mockInventoryService = new Mock<IInventoryService>(MockBehavior.Strict);

            var deckService = new DeckService(mockRepo.Object, mockQueryService.Object, mockInventoryService.Object, mockLogger.Object);
            DeckProperties props = new DeckProperties();

            //Act
            var result = await deckService.AddDeck(props);

            //Assert
            Assert.AreEqual(mockDeckId, result); 
        }

        [TestMethod]
        public async Task DeckService_UpdateDeck_Test()
        {
            //Arrange
            var mockRepo = new Mock<IDeckDataRepo>(MockBehavior.Strict);

            mockRepo
                .Setup(p => p.GetDeckById(It.IsNotNull<int>()))
                .ReturnsAsync(new DeckData());

            mockRepo
                .Setup(p => p.UpdateDeck(It.IsNotNull<DeckData>()))
                .Returns(Task.CompletedTask);

            var mockLogger = new Mock<ILogger<DeckService>>(MockBehavior.Loose);

            var mockQueryService = new Mock<IDataQueryService>(MockBehavior.Strict);

            var mockInventoryService = new Mock<IInventoryService>(MockBehavior.Strict);

            var deckService = new DeckService(mockRepo.Object, mockQueryService.Object, mockInventoryService.Object, mockLogger.Object);
            DeckProperties props = new DeckProperties();

            //Act
            await deckService.UpdateDeck(props);

            //Assert
            //Returns void, just need to know it succeeded
        }

        [TestMethod]
        public async Task DeckService_DeleteDeck_Test()
        {
            //Arrange
            var mockRepo = new Mock<IDeckDataRepo>(MockBehavior.Strict);

            mockRepo
                .Setup(p => p.DeleteDeck(It.IsNotNull<int>()))
                .Returns(Task.CompletedTask);

            var mockLogger = new Mock<ILogger<DeckService>>(MockBehavior.Loose);

            var mockQueryService = new Mock<IDataQueryService>(MockBehavior.Strict);

            var mockInventoryService = new Mock<IInventoryService>(MockBehavior.Strict);

            var deckService = new DeckService(mockRepo.Object, mockQueryService.Object, mockInventoryService.Object, mockLogger.Object);
            int idToDelete = 1;

            //Act
            await deckService.DeleteDeck(idToDelete);

            //Assert
            //Returns void, just need to know it succeeded
        }

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

            var mockInventoryService = new Mock<IInventoryService>(MockBehavior.Strict);

            DeckService deckService = new DeckService(mockRepo.Object, mockQueryService.Object, mockInventoryService.Object, mockLogger.Object);

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
            List<CardOverviewResult> dbOverviews = new List<CardOverviewResult>();

            List<InventoryCardResult> dbCardDetails = new List<InventoryCardResult>();

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
                .Setup(p => p.GetDeckCardOverviews(It.Is<int>(i => i == idToRequest)))
                .ReturnsAsync(dbOverviews);

            mockQueryService
                .Setup(p => p.GetDeckInventoryCards(It.Is<int>(i => i == idToRequest)))
                .ReturnsAsync(dbCardDetails);

            int expectedDeckCardCountResult = 60;

            mockQueryService
                .Setup(p => p.GetDeckCardCount(It.Is<int>(i => i == idToRequest)))
                .ReturnsAsync(expectedDeckCardCountResult);

            List<DeckCardStatResult> expectedStatsResult = new List<DeckCardStatResult>();

            mockQueryService
                .Setup(p => p.GetDeckCardStats(It.Is<int>(i => i == idToRequest)))
                .ReturnsAsync(expectedStatsResult);
            
            var mockInventoryService = new Mock<IInventoryService>(MockBehavior.Strict);

            var deckService = new DeckService(mockDeckRepo.Object, mockQueryService.Object, mockInventoryService.Object, mockLogger.Object);
            

            List<DeckProperties> expectedResults = new List<DeckProperties>();


            //Act
            var result = await deckService.GetDeckDetail(idToRequest);

            //Assert
            Assert.IsNotNull(result);
            //Assert.Fail("Must add more assert conditions");
        }

        [TestMethod]
        public async Task DeckService_AddDeckCard_NewInventoryCard_Test()
        {
            //Arrange
            DeckCard cardToAdd = new DeckCard()
            {
                InventoryCard = new InventoryCard()
                {
                    Id = 0,
                }
            };

            int inventoryIdToExpect = 1;

            var mockRepo = new Mock<IDeckDataRepo>(MockBehavior.Strict);

            mockRepo
                .Setup(p => p.AddDeckCard(It.IsNotNull<DeckCardData>()))
                .Returns(Task.CompletedTask);

            DeckCardData expectedDeckCardResponse = new DeckCardData();

            var mockLogger = new Mock<ILogger<DeckService>>(MockBehavior.Loose);

            var mockQueryService = new Mock<IDataQueryService>(MockBehavior.Strict);

            var mockInventoryService = new Mock<IInventoryService>(MockBehavior.Strict);

            mockInventoryService
                .Setup(p => p.AddInventoryCard(It.IsNotNull<InventoryCard>()))
                .ReturnsAsync(inventoryIdToExpect);

            var deckService = new DeckService(mockRepo.Object, mockQueryService.Object, mockInventoryService.Object, mockLogger.Object);

            //Act
            await deckService.AddDeckCard(cardToAdd);

            //Assert
            //Nothing to assert, returns void
        }

        [TestMethod]
        public async Task DeckService_AddDeckCard_FromInventory_Test()
        {
            //Arrange
            DeckCard cardToAdd = new DeckCard()
            {
                InventoryCard = new InventoryCard()
                {
                    Id = 1,
                }
            };

            DeckCardData expectedDeckCardResponse = null;

            var mockRepo = new Mock<IDeckDataRepo>(MockBehavior.Strict);

            mockRepo
                .Setup(p => p.AddDeckCard(It.IsNotNull<DeckCardData>()))
                .Returns(Task.CompletedTask);

            mockRepo
                .Setup(p => p.GetDeckCardByInventoryId(It.Is<int>(i => i == cardToAdd.InventoryCard.Id)))
                .ReturnsAsync(expectedDeckCardResponse);

            var mockLogger = new Mock<ILogger<DeckService>>(MockBehavior.Loose);

            var mockQueryService = new Mock<IDataQueryService>(MockBehavior.Strict);

            var mockInventoryService = new Mock<IInventoryService>(MockBehavior.Strict);

            var deckService = new DeckService(mockRepo.Object, mockQueryService.Object, mockInventoryService.Object, mockLogger.Object);

            //Act
            await deckService.AddDeckCard(cardToAdd);

            //Assert
            //Nothing to assert, returns void
        }


        [TestMethod]
        public async Task DeckService_AddDeckCard_FromAnotherDeck_Test()
        {
            //Arrange
            DeckCard cardToAdd = new DeckCard()
            {
                InventoryCard = new InventoryCard()
                {
                    Id = 1,
                }
            };

            DeckCardData expectedDeckCardResponse = new DeckCardData();

            var mockRepo = new Mock<IDeckDataRepo>(MockBehavior.Strict);

            mockRepo
                .Setup(p => p.UpdateDeckCard(It.IsNotNull<DeckCardData>()))
                .Returns(Task.CompletedTask);

            mockRepo
                .Setup(p => p.GetDeckCardByInventoryId(It.Is<int>(i => i == cardToAdd.InventoryCard.Id)))
                .ReturnsAsync(expectedDeckCardResponse);

            var mockLogger = new Mock<ILogger<DeckService>>(MockBehavior.Loose);

            var mockQueryService = new Mock<IDataQueryService>(MockBehavior.Strict);

            var mockInventoryService = new Mock<IInventoryService>(MockBehavior.Strict);

            var deckService = new DeckService(mockRepo.Object, mockQueryService.Object, mockInventoryService.Object, mockLogger.Object);

            //Act
            await deckService.AddDeckCard(cardToAdd);

            //Assert
            //Nothing to assert, returns void
        }

        [TestMethod]
        public async Task DeckService_AddDeckCardBatch_Test()
        {
            //Arrange
            var mockRepo = new Mock<IDeckDataRepo>(MockBehavior.Strict);

            mockRepo
                .Setup(p => p.AddDeckCard(It.IsAny<DeckCardData>()))
                .Returns(Task.CompletedTask);

            var mockLogger = new Mock<ILogger<DeckService>>(MockBehavior.Loose);

            var mockQueryService = new Mock<IDataQueryService>(MockBehavior.Strict);

            var mockInventoryService = new Mock<IInventoryService>(MockBehavior.Strict);

            var deckService = new DeckService(mockRepo.Object, mockQueryService.Object, mockInventoryService.Object, mockLogger.Object);

            List<DeckCard> cardsToAdd = new List<DeckCard>();

            //Act
            await deckService.AddDeckCardBatch(cardsToAdd);

            //Assert
            //Nothing to assert, returns void
        }

        [TestMethod]
        public async Task DeckService_UpdateDeckCard_Test()
        {
            //Arrange
            DeckCard card = new DeckCard()
            {
                Id = 1,
            };

            DeckCardData expectedDbCard = new DeckCardData()
            {
                Id = 1,
            };

            var mockRepo = new Mock<IDeckDataRepo>(MockBehavior.Strict);

            mockRepo
                .Setup(p => p.UpdateDeckCard(It.IsNotNull<DeckCardData>()))
                .Returns(Task.CompletedTask);

            mockRepo
                .Setup(p => p.GetDeckCardById(It.IsNotNull<int>()))
                .ReturnsAsync(expectedDbCard);

            var mockLogger = new Mock<ILogger<DeckService>>(MockBehavior.Loose);

            var mockQueryService = new Mock<IDataQueryService>(MockBehavior.Strict);

            var mockInventoryService = new Mock<IInventoryService>(MockBehavior.Strict);

            var deckService = new DeckService(mockRepo.Object, mockQueryService.Object, mockInventoryService.Object, mockLogger.Object);

            //Act
            await deckService.UpdateDeckCard(card);

            //Assert
            //Nothing to assert, returns void
        }

        [TestMethod]
        public async Task DeckService_DeleteDeckCard_Test()
        {
            //Arrange
            var mockRepo = new Mock<IDeckDataRepo>(MockBehavior.Strict);

            mockRepo
                .Setup(p => p.DeleteDeckCard(It.Is<int>(x => x > 0)))
                .Returns(Task.CompletedTask);

            var mockLogger = new Mock<ILogger<DeckService>>(MockBehavior.Loose);

            var mockQueryService = new Mock<IDataQueryService>(MockBehavior.Strict);

            var mockInventoryService = new Mock<IInventoryService>(MockBehavior.Strict);

            var deckService = new DeckService(mockRepo.Object, mockQueryService.Object, mockInventoryService.Object, mockLogger.Object);

            int idToDelete = 1;

            //Act
            await deckService.DeleteDeckCard(idToDelete);

            //Assert
            //Nothing to assert, returns void
        }

    }
}
