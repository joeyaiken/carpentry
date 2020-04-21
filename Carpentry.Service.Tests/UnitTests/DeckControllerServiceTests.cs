using Carpentry.Data.DataModels;
using Carpentry.Data.Interfaces;
using Carpentry.Data.QueryResults;
using Carpentry.Logic.Implementations;
using Carpentry.Logic.Interfaces;
using Carpentry.Logic.Models;
using Carpentry.Service.Implementations;
using Carpentry.Service.Interfaces;
using Carpentry.Service.Models;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Carpentry.Service.Tests.UnitTests
{
    [TestClass]
    public class DeckControllerServiceTests
    {
        //private readonly DeckControllerService _deckService;

        //private readonly int _mockDeckId;
        public DeckControllerServiceTests()
        {
            //_mockDeckId = 1;

            ////deckRepo
            //var mockRepo = new Mock<IDeckDataRepo>(MockBehavior.Strict);

            //mockRepo
            //    .Setup(p => p.AddDeck(It.IsNotNull<DeckData>()))
            //    .ReturnsAsync(_mockDeckId);

            //mockRepo
            //    .Setup(p => p.GetDeckById(It.IsNotNull<int>()))
            //    .ReturnsAsync(new DeckData());

            //mockRepo
            //    .Setup(p => p.UpdateDeck(It.IsNotNull<DeckData>()))
            //    .Returns(Task.CompletedTask);

            //mockRepo
            //    .Setup(p => p.DeleteDeck(It.IsNotNull<int>()))
            //    .Returns(Task.CompletedTask);

            ////mockRepo
            ////    .Setup(p => p.(It.IsAny<>()))
            ////    .Returns();

            ////mockRepo
            ////    .Setup(p => p.(It.IsAny<>()))
            ////    .Returns();

            ////mockRepo
            ////    .Setup(p => p.(It.IsAny<>()))
            ////    .Returns();

            ////mockRepo
            ////    .Setup(p => p.(It.IsAny<>()))
            ////    .Returns();

            ////mockRepo
            ////    .Setup(p => p.(It.IsAny<>()))
            ////    .Returns();

            //mockRepo
            //    .Setup(p => p.GetDeckCardById(It.IsNotNull<int>()))
            //    .ReturnsAsync(new DeckCardData());

            //mockRepo
            //    .Setup(p => p.UpdateDeckCard(It.IsNotNull<DeckCardData>()))
            //    .Returns(Task.CompletedTask);

            //mockRepo
            //    .Setup(p => p.DeleteDeckCard(It.Is<int>(x => x > 0)))
            //    .Returns(Task.CompletedTask);


            ////logger
            //var mockLogger = new Mock<ILogger<DeckControllerService>>(MockBehavior.Loose);

            //var mockQueryService = new Mock<IDataQueryService>(MockBehavior.Strict);

            //var mockInventoryService = new Mock<IInventoryControllerService>(MockBehavior.Strict);

            ////_deckService = new DeckService(mockRepo.Object, mockQueryService.Object, mockInventoryService.Object, mockLogger.Object);

            
        }

        [TestMethod]
        public async Task DeckControllerService_AddDeck_Test()
        {
            //Arrange
            int expectedNewDeckId = 1;

            var mockReferenceService = new Mock<IDataReferenceService>(MockBehavior.Strict);

            DataReferenceValue<int> formatResult = new DataReferenceValue<int>() { Id = 1 };

            mockReferenceService
                .Setup(p => p.GetMagicFormat(It.IsAny<string>()))
                .ReturnsAsync(formatResult);

            var mockDeckService = new Mock<IDeckService>(MockBehavior.Strict);

            mockDeckService
                .Setup(p => p.AddDeck(It.IsNotNull<DeckProperties>()))
                .ReturnsAsync(expectedNewDeckId);

            var deckService = new DeckControllerService(mockReferenceService.Object, mockDeckService.Object);

            DeckPropertiesDto props = new DeckPropertiesDto();

            //Act
            var result = await deckService.AddDeck(props);

            //Assert
            Assert.AreEqual(expectedNewDeckId, result); 
        }

        [TestMethod]
        public async Task DeckControllerService_UpdateDeck_Test()
        {
            //Arrange
            var mockReferenceService = new Mock<IDataReferenceService>(MockBehavior.Strict);

            DataReferenceValue<int> formatResult = new DataReferenceValue<int>() { Id = 1 };

            mockReferenceService
                .Setup(p => p.GetMagicFormat(It.IsAny<string>()))
                .ReturnsAsync(formatResult);

            var mockDeckService = new Mock<IDeckService>(MockBehavior.Strict);

            mockDeckService
                .Setup(p => p.UpdateDeck(It.IsNotNull<DeckProperties>()))
                .Returns(Task.CompletedTask);

            var deckService = new DeckControllerService(mockReferenceService.Object, mockDeckService.Object);

            DeckPropertiesDto props = new DeckPropertiesDto();

            //Act
            await deckService.UpdateDeck(props);

            //Assert
            //Returns void, just need to know it succeeded
        }

        [TestMethod]
        public async Task DeckControllerService_DeleteDeck_Test()
        {
            //Arrange
            var mockReferenceService = new Mock<IDataReferenceService>(MockBehavior.Strict);

            var mockDeckService = new Mock<IDeckService>(MockBehavior.Strict);

            int idToDelete = 1;

            mockDeckService
                .Setup(p => p.DeleteDeck(It.Is<int>(i => i == idToDelete)))
                .Returns(Task.CompletedTask);

            var deckService = new DeckControllerService(mockReferenceService.Object, mockDeckService.Object);

            //Act
            await deckService.DeleteDeck(idToDelete);

            //Assert
            //Returns void, just need to know it succeeded
        }

        [TestMethod]
        public async Task DeckControllerService_GetDeckOverviews_Test()
        {
            //Arrange
            var mockReferenceService = new Mock<IDataReferenceService>(MockBehavior.Strict);

            var mockDeckService = new Mock<IDeckService>(MockBehavior.Strict);

            var deckService = new DeckControllerService(mockReferenceService.Object, mockDeckService.Object);

            //Act
            var result = await deckService.GetDeckOverviews();

            //Assert
            Assert.IsNotNull(result);
            
            Assert.Fail("want this to keep failing untill I have more conditions - like number of expected records"); //want this to keep failing
        }

        [TestMethod]
        public async Task DeckControllerService_GetDeckDetail_Test()
        {
            //Arrange
            var mockReferenceService = new Mock<IDataReferenceService>(MockBehavior.Strict);

            var mockDeckService = new Mock<IDeckService>(MockBehavior.Strict);

            var deckService = new DeckControllerService(mockReferenceService.Object, mockDeckService.Object);

            int idToRequest = 1;

            //Act
            var result = await deckService.GetDeckDetail(idToRequest);

            //Assert
            Assert.IsNotNull(result);
            Assert.Fail("Must add more assert conditions");
        }

        [TestMethod]
        public async Task DeckControllerService_AddDeckCard_Test()
        {
            //Arrange
            var mockReferenceService = new Mock<IDataReferenceService>(MockBehavior.Strict);

            var mockDeckService = new Mock<IDeckService>(MockBehavior.Strict);

            mockDeckService
                .Setup(p => p.AddDeckCard(It.IsAny<DeckCard>()))
                .Returns(Task.CompletedTask);

            var deckService = new DeckControllerService(mockReferenceService.Object, mockDeckService.Object);

            DeckCardDto cardToAdd = new DeckCardDto()
            {
                InventoryCard = new InventoryCardDto(),
            };

            //Act
            await deckService.AddDeckCard(cardToAdd);

            //Assert
            //Nothing to assert, returns void
        }

        [TestMethod]
        public async Task DeckControllerService_AddDeckCardBatch_Test()
        {
            //Arrange
            var mockReferenceService = new Mock<IDataReferenceService>(MockBehavior.Strict);

            var mockDeckService = new Mock<IDeckService>(MockBehavior.Strict);

            mockDeckService
                .Setup(p => p.AddDeckCardBatch(It.IsNotNull<IEnumerable<DeckCard>>()))
                .Returns(Task.CompletedTask);

            var deckService = new DeckControllerService(mockReferenceService.Object, mockDeckService.Object);

            List<DeckCardDto> cardsToAdd = new List<DeckCardDto>();

            //Act
            await deckService.AddDeckCardBatch(cardsToAdd);

            //Assert
            //Nothing to assert, returns void
        }

        [TestMethod]
        public async Task DeckControllerService_UpdateDeckCard_Test()
        {
            //Arrange
            var mockReferenceService = new Mock<IDataReferenceService>(MockBehavior.Strict);

            var mockDeckService = new Mock<IDeckService>(MockBehavior.Strict);

            mockDeckService
                .Setup(p => p.UpdateDeckCard(It.IsNotNull<DeckCard>()))
                .Returns(Task.CompletedTask);

            var deckService = new DeckControllerService(mockReferenceService.Object, mockDeckService.Object);

            DeckCardDto card = new DeckCardDto();

            //Act
            await deckService.UpdateDeckCard(card);

            //Assert
            //Nothing to assert, returns void
        }

        [TestMethod]
        public async Task DeckControllerService_DeleteDeckCard_Test()
        {
            //Arrange
            int idToDelete = 1;

            var mockReferenceService = new Mock<IDataReferenceService>(MockBehavior.Strict);

            var mockDeckService = new Mock<IDeckService>(MockBehavior.Strict);

            mockDeckService
                .Setup(p => p.DeleteDeckCard(It.Is<int>(x => x == idToDelete)))
                .Returns(Task.CompletedTask);

            var deckService = new DeckControllerService(mockReferenceService.Object, mockDeckService.Object);

            //Act
            await deckService.DeleteDeckCard(idToDelete);

            //Assert
            //Nothing to assert, returns void
        }

    }
}
