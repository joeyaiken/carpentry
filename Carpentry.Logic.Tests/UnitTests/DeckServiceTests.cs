using Carpentry.Data.DataModels;
using Carpentry.Data.Interfaces;
using Carpentry.Logic.Implementations;
using Carpentry.Logic.Interfaces;
using Carpentry.Logic.Models;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Carpentry.Logic.Tests.UnitTests
{
    [TestClass]
    public class DeckServiceTests
    {
        private readonly DeckService _deckService;

        private readonly int _mockDeckId;
        public DeckServiceTests()
        {
            _mockDeckId = 1;

            //deckRepo
            var mockRepo = new Mock<IDeckDataRepo>(MockBehavior.Strict);

            mockRepo
                .Setup(p => p.AddDeck(It.IsNotNull<DeckData>()))
                .ReturnsAsync(_mockDeckId);

            mockRepo
                .Setup(p => p.GetDeckById(It.IsNotNull<int>()))
                .ReturnsAsync(new DeckData());

            mockRepo
                .Setup(p => p.UpdateDeck(It.IsNotNull<DeckData>()))
                .Returns(Task.CompletedTask);

            mockRepo
                .Setup(p => p.DeleteDeck(It.IsNotNull<int>()))
                .Returns(Task.CompletedTask);

            //mockRepo
            //    .Setup(p => p.(It.IsAny<>()))
            //    .Returns();

            //mockRepo
            //    .Setup(p => p.(It.IsAny<>()))
            //    .Returns();

            //mockRepo
            //    .Setup(p => p.(It.IsAny<>()))
            //    .Returns();

            //mockRepo
            //    .Setup(p => p.(It.IsAny<>()))
            //    .Returns();

            //mockRepo
            //    .Setup(p => p.(It.IsAny<>()))
            //    .Returns();

            mockRepo
                .Setup(p => p.GetDeckCardById(It.IsNotNull<int>()))
                .ReturnsAsync(new DeckCardData());

            mockRepo
                .Setup(p => p.UpdateDeckCard(It.IsNotNull<DeckCardData>()))
                .Returns(Task.CompletedTask);

            mockRepo
                .Setup(p => p.DeleteDeckCard(It.Is<int>(x => x > 0)))
                .Returns(Task.CompletedTask);


            //logger
            var mockLogger = new Mock<ILogger<DeckService>>(MockBehavior.Loose);

            var mockQueryService = new Mock<IDataQueryService>(MockBehavior.Strict);

            var mockInventoryService = new Mock<IInventoryService>(MockBehavior.Strict);

            _deckService = new DeckService(mockRepo.Object, mockQueryService.Object, mockInventoryService.Object, mockLogger.Object);

            
        }

        [TestMethod]
        public async Task DeckService_AddDeck_Test()
        {
            //Arrange
            DeckProperties props = new DeckProperties();

            //Act
            var result = await _deckService.AddDeck(props);

            //Assert
            Assert.AreEqual(_mockDeckId, result); 
        }

        [TestMethod]
        public async Task DeckService_UpdateDeck_Test()
        {
            //Arrange
            DeckProperties props = new DeckProperties();

            //Act
            await _deckService.UpdateDeck(props);

            //Assert
            //Returns void, just need to know it succeeded
        }

        [TestMethod]
        public async Task DeckService_DeleteDeck_Test()
        {
            //Arrange
            int idToDelete = 1;

            //Act
            await _deckService.DeleteDeck(idToDelete);

            //Assert
            //Returns void, just need to know it succeeded
        }

        [TestMethod]
        public async Task DeckService_GetDeckOverviews_Test()
        {
            //Arrange


            //Act
            var result = await _deckService.GetDeckOverviews();

            //Assert
            Assert.IsNotNull(result);
            
            Assert.Fail("want this to keep failing untill I have more conditions - like number of expected records"); //want this to keep failing
        }

        [TestMethod]
        public async Task DeckService_GetDeckDetail_Test()
        {
            //Arrange
            int idToRequest = 1;

            //Act
            var result = await _deckService.GetDeckDetail(idToRequest);

            //Assert
            Assert.IsNotNull(result);
            Assert.Fail("Must add more assert conditions");
        }

        [TestMethod]
        public async Task DeckService_AddDeckCard_Test()
        {
            //Arrange
            DeckCard cardToAdd = new DeckCard();

            //Act
            await _deckService.AddDeckCard(cardToAdd);

            //Assert
            //Nothing to assert, returns void
        }

        [TestMethod]
        public async Task DeckService_AddDeckCardBatch_Test()
        {
            //Arrange
            List<DeckCard> cardsToAdd = new List<DeckCard>();

            //Act
            await _deckService.AddDeckCardBatch(cardsToAdd);

            //Assert
            //Nothing to assert, returns void
        }

        [TestMethod]
        public async Task DeckService_UpdateDeckCard_Test()
        {
            //Arrange
            DeckCard card = new DeckCard();

            //Act
            await _deckService.UpdateDeckCard(card);

            //Assert
            //Nothing to assert, returns void
        }

        [TestMethod]
        public async Task DeckService_DeleteDeckCard_Test()
        {
            //Arrange
            int idToDelete = 1;

            //Act
            await _deckService.DeleteDeckCard(idToDelete);

            //Assert
            //Nothing to assert, returns void
        }

    }
}
