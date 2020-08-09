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

namespace Carpentry.Logic.Tests.UnitTests.DeckServiceTests
{
    [TestClass]
    public class DeckServiceDeckTests
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

            var mockInventoryService = new Mock<IInventoryService>(MockBehavior.Strict);

            var mockCoreRepo = new Mock<ICoreDataRepo>(MockBehavior.Strict);

            DataReferenceValue<int> formatResult = new DataReferenceValue<int>() { Id = 1 };

            mockCoreRepo
                .Setup(p => p.GetMagicFormat(It.IsAny<string>()))
                .ReturnsAsync(formatResult);

            var deckService = new DeckService(mockRepo.Object, mockInventoryService.Object, mockLogger.Object, mockCoreRepo.Object);

            DeckPropertiesDto props = new DeckPropertiesDto();

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

            var mockInventoryService = new Mock<IInventoryService>(MockBehavior.Strict);

            var mockCoreRepo = new Mock<ICoreDataRepo>(MockBehavior.Strict);

            DataReferenceValue<int> formatResult = new DataReferenceValue<int>() { Id = 1 };

            mockCoreRepo
                .Setup(p => p.GetMagicFormat(It.IsAny<string>()))
                .ReturnsAsync(formatResult);

            var deckService = new DeckService(mockRepo.Object, mockInventoryService.Object, mockLogger.Object, mockCoreRepo.Object);

            DeckPropertiesDto props = new DeckPropertiesDto();

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

            var mockInventoryService = new Mock<IInventoryService>(MockBehavior.Strict);

            var mockCoreRepo = new Mock<ICoreDataRepo>(MockBehavior.Strict);

            var deckService = new DeckService(mockRepo.Object, mockInventoryService.Object, mockLogger.Object, mockCoreRepo.Object);

            int idToDelete = 1;

            //Act
            await deckService.DeleteDeck(idToDelete);

            //Assert
            //Returns void, just need to know it succeeded
        }
    }

}
