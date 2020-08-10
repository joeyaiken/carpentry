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
    public class DeckServiceImportExportTests
    {
        //Task<ValidatedDeckImportDto> ValidateDeckImport(DeckImportDto dto);
        [TestMethod]
        public async Task DeckService_ValidateDeckImport_Test()
        {
            //Arrange
 
            var mockRepo = new Mock<IDeckDataRepo>(MockBehavior.Strict);

            var mockLogger = new Mock<ILogger<DeckService>>(MockBehavior.Loose);

            var mockInventoryService = new Mock<IInventoryService>(MockBehavior.Strict);

            var mockCoreRepo = new Mock<ICoreDataRepo>(MockBehavior.Strict);

            var deckService = new DeckService(mockRepo.Object, mockInventoryService.Object, mockLogger.Object, mockCoreRepo.Object);

            var payload = new DeckImportDto() { };

            //Act
            var result = await deckService.ValidateDeckImport(payload);

            //Assert
            Assert.IsNotNull(result);

            Assert.Fail("Must decide how to validate the response object");
        }
        //Task AddValidatedDeckImport(ValidatedDeckImportDto validatedDto);
        [TestMethod]
        public async Task DeckService_AddValidatedDeckImport_Test()
        {
            //Arrange

            var mockRepo = new Mock<IDeckDataRepo>(MockBehavior.Strict);

            var mockLogger = new Mock<ILogger<DeckService>>(MockBehavior.Loose);

            var mockInventoryService = new Mock<IInventoryService>(MockBehavior.Strict);

            var mockCoreRepo = new Mock<ICoreDataRepo>(MockBehavior.Strict);

            var deckService = new DeckService(mockRepo.Object, mockInventoryService.Object, mockLogger.Object, mockCoreRepo.Object);

            var payload = new ValidatedDeckImportDto() { };

            //Act
            await deckService.AddValidatedDeckImport(payload);

            //Assert
            //Returns void, nothing to assert
        }
        //Task<string> ExportDeckList(int deckId);
        [TestMethod]
        public async Task DeckService_ExportDeckList_Test()
        {
            //Arrange

            var mockRepo = new Mock<IDeckDataRepo>(MockBehavior.Strict);

            var mockLogger = new Mock<ILogger<DeckService>>(MockBehavior.Loose);

            var mockInventoryService = new Mock<IInventoryService>(MockBehavior.Strict);

            var mockCoreRepo = new Mock<ICoreDataRepo>(MockBehavior.Strict);

            var deckService = new DeckService(mockRepo.Object, mockInventoryService.Object, mockLogger.Object, mockCoreRepo.Object);

            int deckIdToExport = 1;

            //Act
            var result = await deckService.ExportDeckList(deckIdToExport);

            //Assert
            Assert.IsNotNull(result);

            Assert.Fail("Must decide how to validate the response object");
        }
    }
}
