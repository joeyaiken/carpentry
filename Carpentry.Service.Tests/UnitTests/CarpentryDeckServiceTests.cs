using Carpentry.Logic.Interfaces;
using Carpentry.Logic.Models;
using Carpentry.Service.Implementations;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Carpentry.Service.Tests.UnitTests
{
    [TestClass]
    public class CarpentryDeckServiceTests
    {
        #region Deck Props

        [TestMethod]
        public async Task CarpentryDeckService_AddDeck_Test()
        {
            //Arrange
            var mockDeckService = new Mock<IDeckService>(MockBehavior.Strict);
            var mockImportService = new Mock<IDataImportService>(MockBehavior.Strict);
            var mockExportService = new Mock<IDataExportService>(MockBehavior.Strict);

            int idToReturn = 1;

            mockDeckService
                .Setup(p => p.AddDeck(It.IsNotNull<DeckPropertiesDto>()))
                .ReturnsAsync(idToReturn);

            var deckService = new CarpentryDeckService(
                mockDeckService.Object,
                mockImportService.Object,
                mockExportService.Object
                );

            var deckToSubmit = new DeckPropertiesDto();

            //Act
            var newDeckId = await deckService.AddDeck(deckToSubmit);

            //Assert
            Assert.AreEqual(idToReturn, newDeckId);

        }

        [TestMethod]
        public async Task CarpentryDeckService_UpdateDeck_Test()
        {
            //Arrange
            var mockDeckService = new Mock<IDeckService>(MockBehavior.Strict);
            var mockImportService = new Mock<IDataImportService>(MockBehavior.Strict);
            var mockExportService = new Mock<IDataExportService>(MockBehavior.Strict);

            mockDeckService
                .Setup(p => p.UpdateDeck(It.IsNotNull<DeckPropertiesDto>()))
                .Returns(Task.CompletedTask);

            var deckService = new CarpentryDeckService(
                mockDeckService.Object,
                mockImportService.Object,
                mockExportService.Object
                );

            var deckToSubmit = new DeckPropertiesDto();

            //Act
            await deckService.UpdateDeck(deckToSubmit);

            //Assert
            //returns void, nothing to assert
        }
        
        [TestMethod]
        public async Task CarpentryDeckService_DeleteDeck_Test()
        {
            //Arrange
            var mockDeckService = new Mock<IDeckService>(MockBehavior.Strict);
            var mockImportService = new Mock<IDataImportService>(MockBehavior.Strict);
            var mockExportService = new Mock<IDataExportService>(MockBehavior.Strict);

            mockDeckService
                .Setup(p => p.DeleteDeck(It.Is<int>(i => i > 0)))
                .Returns(Task.CompletedTask);

            var deckService = new CarpentryDeckService(
                mockDeckService.Object,
                mockImportService.Object,
                mockExportService.Object
                );

            var idToSubmit = 1;

            //Act
            await deckService.DeleteDeck(idToSubmit);

            //Assert
            //returns void, nothing to assert
        }

        #endregion Deck Props

        #region Deck Cards

        [TestMethod]
        public async Task CarpentryDeckService_AddDeckCard_Test()
        {
            //Arrange
            var mockDeckService = new Mock<IDeckService>(MockBehavior.Strict);
            var mockImportService = new Mock<IDataImportService>(MockBehavior.Strict);
            var mockExportService = new Mock<IDataExportService>(MockBehavior.Strict);

            mockDeckService
                .Setup(p => p.AddDeckCard(It.IsNotNull<DeckCardDto>()))
                .Returns(Task.CompletedTask);

            var deckService = new CarpentryDeckService(
                mockDeckService.Object,
                mockImportService.Object,
                mockExportService.Object
                );

            var deckCardToSubmit = new DeckCardDto();

            //Act
            await deckService.AddDeckCard(deckCardToSubmit);

            //Assert
            //returns void, nothing to assert
        }

        [TestMethod]
        public async Task CarpentryDeckService_AddDeckCardBatch_Test()
        {
            //Arrange
            var mockDeckService = new Mock<IDeckService>(MockBehavior.Strict);
            var mockImportService = new Mock<IDataImportService>(MockBehavior.Strict);
            var mockExportService = new Mock<IDataExportService>(MockBehavior.Strict);

            //int idToReturn = 1;

            mockDeckService
                .Setup(p => p.AddDeckCardBatch(It.IsNotNull<IEnumerable<DeckCardDto>>()))
                .Returns(Task.CompletedTask);

            var deckService = new CarpentryDeckService(
                mockDeckService.Object,
                mockImportService.Object,
                mockExportService.Object
                );

            //var deckCardToSubmit = new DeckCardDto();
            var batchToSubmit = new List<DeckCardDto>()
            {

            };

            //Act
            await deckService.AddDeckCardBatch(batchToSubmit);

            //Assert
            //returns void, nothing to assert
        }

        [TestMethod]
        public async Task CarpentryDeckService_UpdateDeckCard_Test()
        {
            //Arrange
            var mockDeckService = new Mock<IDeckService>(MockBehavior.Strict);
            var mockImportService = new Mock<IDataImportService>(MockBehavior.Strict);
            var mockExportService = new Mock<IDataExportService>(MockBehavior.Strict);

            mockDeckService
                .Setup(p => p.UpdateDeckCard(It.IsNotNull<DeckCardDto>()))
                .Returns(Task.CompletedTask);

            var deckService = new CarpentryDeckService(
                mockDeckService.Object,
                mockImportService.Object,
                mockExportService.Object
                );

            var deckCardToSubmit = new DeckCardDto();

            //Act
            await deckService.UpdateDeckCard(deckCardToSubmit);

            //Assert
            //returns void, nothing to assert
        }

        [TestMethod]
        public async Task CarpentryDeckService_DeleteDeckCard_Test()
        {
            //Arrange
            var mockDeckService = new Mock<IDeckService>(MockBehavior.Strict);
            var mockImportService = new Mock<IDataImportService>(MockBehavior.Strict);
            var mockExportService = new Mock<IDataExportService>(MockBehavior.Strict);

            mockDeckService
                .Setup(p => p.DeleteDeckCard(It.Is<int>(i => i > 0)))
                .Returns(Task.CompletedTask);

            var deckService = new CarpentryDeckService(
                mockDeckService.Object,
                mockImportService.Object,
                mockExportService.Object
                );

            int idToSubmit = 1;

            //Act
            await deckService.DeleteDeckCard(idToSubmit);

            //Assert
            //returns void, nothing to assert
        }

        #endregion Deck Cards

        #region Search

        [TestMethod]
        public async Task CarpentryDeckService_GetDeckOverviews_Test()
        {
            //Arrange
            var mockDeckService = new Mock<IDeckService>(MockBehavior.Strict);
            var mockImportService = new Mock<IDataImportService>(MockBehavior.Strict);
            var mockExportService = new Mock<IDataExportService>(MockBehavior.Strict);

            var deckOverviewsToReturn = new List<DeckOverviewDto>()
            {
                new DeckOverviewDto(){ },
                new DeckOverviewDto(){ },
                new DeckOverviewDto(){ },
            };

            mockDeckService
                .Setup(p => p.GetDeckOverviews())
                .ReturnsAsync(deckOverviewsToReturn);

            var deckService = new CarpentryDeckService(
                mockDeckService.Object,
                mockImportService.Object,
                mockExportService.Object
                );

            //Act
            var result = await deckService.GetDeckOverviews();

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(deckOverviewsToReturn.Count, result.Count);
        }
        
        [TestMethod]
        public async Task CarpentryDeckService_GetDeckDetail_Test()
        {
            //Arrange
            var mockDeckService = new Mock<IDeckService>(MockBehavior.Strict);
            var mockImportService = new Mock<IDataImportService>(MockBehavior.Strict);
            var mockExportService = new Mock<IDataExportService>(MockBehavior.Strict);

            var expectedResult = new DeckDetailDto();

            mockDeckService
                .Setup(p => p.GetDeckDetail(It.Is<int>(i => i > 0)))
                .ReturnsAsync(expectedResult);

            var deckService = new CarpentryDeckService(
                mockDeckService.Object,
                mockImportService.Object,
                mockExportService.Object
                );

            int idToSubmit = 1;

            //Act
            var result = await deckService.GetDeckDetail(idToSubmit);

            //Assert
            Assert.IsNotNull(result);
        }

        #endregion Search

        #region Import / Export

        [TestMethod]
        public async Task CarpentryDeckService_ValidateDeckImport_Test()
        {
            //Arrange
            var mockDeckService = new Mock<IDeckService>(MockBehavior.Strict);
            var mockImportService = new Mock<IDataImportService>(MockBehavior.Strict);
            var mockExportService = new Mock<IDataExportService>(MockBehavior.Strict);

            var expectedResult = new ValidatedDeckImportDto() { };

            mockImportService
                .Setup(p => p.ValidateDeckImport(It.IsNotNull<CardImportDto>()))
                .ReturnsAsync(expectedResult);

            var deckService = new CarpentryDeckService(
                mockDeckService.Object,
                mockImportService.Object,
                mockExportService.Object
                );

            var payloadToSubmit = new CardImportDto()
            {

            };

            //Act
            var result = await deckService.ValidateDeckImport(payloadToSubmit);

            //Assert
            Assert.IsNotNull(result);
        }
        
        [TestMethod]
        public async Task CarpentryDeckService_AddValidatedDeckImport_Test()
        {
            //Arrange
            var mockDeckService = new Mock<IDeckService>(MockBehavior.Strict);
            var mockImportService = new Mock<IDataImportService>(MockBehavior.Strict);
            var mockExportService = new Mock<IDataExportService>(MockBehavior.Strict);

            mockImportService
                .Setup(p => p.AddValidatedDeckImport(It.IsNotNull<ValidatedDeckImportDto>()))
                .Returns(Task.CompletedTask);

            var deckService = new CarpentryDeckService(
                mockDeckService.Object,
                mockImportService.Object,
                mockExportService.Object
                );

            var payloadToSubmit = new ValidatedDeckImportDto() { };

            //Act
            await deckService.AddValidatedDeckImport(payloadToSubmit);

            //Assert
            //returns void, nothing to assert
        }

        [TestMethod]
        public async Task CarpentryDeckService_ExportDeckList_Test()
        {
            //Arrange
            var mockDeckService = new Mock<IDeckService>(MockBehavior.Strict);
            var mockImportService = new Mock<IDataImportService>(MockBehavior.Strict);
            var mockExportService = new Mock<IDataExportService>(MockBehavior.Strict);

            var expectedResult = "A DECK LIST";

            mockExportService
                .Setup(p => p.GetDeckListExport(It.Is<int>(i => i > 0)))
                .ReturnsAsync(expectedResult);
            
            var deckService = new CarpentryDeckService(
                mockDeckService.Object,
                mockImportService.Object,
                mockExportService.Object
                );

            var idToSubmit = 1;

            //Act
            var result = await deckService.ExportDeckList(idToSubmit);

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedResult, result);
        }

        #endregion Import / Export
    }
}
