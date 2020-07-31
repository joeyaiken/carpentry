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
    public class DeckServiceDeckCardTests
    {
        [TestMethod]
        public async Task DeckService_AddDeckCard_NewInventoryCard_Test()
        {
            //Arrange
            DeckCardDto cardToAdd = new DeckCardDto()
            {
                InventoryCard = new InventoryCardDto()
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
                .Setup(p => p.AddInventoryCard(It.IsNotNull<InventoryCardDto>()))
                .ReturnsAsync(inventoryIdToExpect);

            var mockReferenceService = new Mock<IDataReferenceService>(MockBehavior.Strict);

            var deckService = new DeckService(mockRepo.Object, mockQueryService.Object, mockInventoryService.Object, mockLogger.Object, mockReferenceService.Object);

            //Act
            await deckService.AddDeckCard(cardToAdd);

            //Assert
            //Nothing to assert, returns void
        }

        [TestMethod]
        public async Task DeckService_AddDeckCard_FromInventory_Test()
        {
            //Arrange
            DeckCardDto cardToAdd = new DeckCardDto()
            {
                InventoryCard = new InventoryCardDto()
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

            var mockReferenceService = new Mock<IDataReferenceService>(MockBehavior.Strict);

            var deckService = new DeckService(mockRepo.Object, mockQueryService.Object, mockInventoryService.Object, mockLogger.Object, mockReferenceService.Object);

            //Act
            await deckService.AddDeckCard(cardToAdd);

            //Assert
            //Nothing to assert, returns void
        }

        [TestMethod]
        public async Task DeckService_AddDeckCard_FromAnotherDeck_Test()
        {
            //Arrange
            DeckCardDto cardToAdd = new DeckCardDto()
            {
                InventoryCard = new InventoryCardDto()
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

            var mockReferenceService = new Mock<IDataReferenceService>(MockBehavior.Strict);

            var deckService = new DeckService(mockRepo.Object, mockQueryService.Object, mockInventoryService.Object, mockLogger.Object, mockReferenceService.Object);

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

            var mockReferenceService = new Mock<IDataReferenceService>(MockBehavior.Strict);

            var deckService = new DeckService(mockRepo.Object, mockQueryService.Object, mockInventoryService.Object, mockLogger.Object, mockReferenceService.Object);

            List<DeckCardDto> cardsToAdd = new List<DeckCardDto>();

            //Act
            await deckService.AddDeckCardBatch(cardsToAdd);

            //Assert
            //Nothing to assert, returns void
        }

        [TestMethod]
        public async Task DeckService_UpdateDeckCard_Test()
        {
            //Arrange
            DeckCardDto card = new DeckCardDto()
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

            var mockReferenceService = new Mock<IDataReferenceService>(MockBehavior.Strict);

            var deckService = new DeckService(mockRepo.Object, mockQueryService.Object, mockInventoryService.Object, mockLogger.Object, mockReferenceService.Object);

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

            var mockReferenceService = new Mock<IDataReferenceService>(MockBehavior.Strict);

            var deckService = new DeckService(mockRepo.Object, mockQueryService.Object, mockInventoryService.Object, mockLogger.Object, mockReferenceService.Object);

            int idToDelete = 1;

            //Act
            await deckService.DeleteDeckCard(idToDelete);

            //Assert
            //Nothing to assert, returns void
        }
    }
}
