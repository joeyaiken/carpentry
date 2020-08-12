using Carpentry.Data.Interfaces;
using Carpentry.Logic.Implementations;
using Carpentry.Logic.Interfaces;
using Carpentry.Logic.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Carpentry.Logic.Tests.UnitTests.InventoryServiceTests
{
    [TestClass]
    public class ImportExportTests
    {
        //Task<ValidatedCarpentryImportDto> ValidateCarpentryImport(CardImportDto cardImportDto);
        [TestMethod]
        public async Task InventoryService_ValidateCarpentryImport_Test()
        {
            //Assemble

            var mockInventoryRepo = new Mock<IInventoryDataRepo>(MockBehavior.Strict);

            var mockDataUpdateService = new Mock<IDataUpdateService>(MockBehavior.Strict);

            var mockCoreRepo = new Mock<ICoreDataRepo>(MockBehavior.Strict);

            var mockCardDatarepo = new Mock<ICardDataRepo>(MockBehavior.Strict);

            var inventoryService = new InventoryService(
                mockInventoryRepo.Object,
                mockDataUpdateService.Object,
                mockCoreRepo.Object,
                mockCardDatarepo.Object);

            var payloadToSubmit = new CardImportDto() { };

            //List<CardDataDto> expectedSearchResult = new List<CardDataDto>()
            //{
            //    CardInstance(),
            //    CardInstance(),
            //    CardInstance(),
            //    CardInstance(),
            //    CardInstance(),
            //};

            ////var mockQueryService = new Mock<IDataQueryService>(MockBehavior.Strict);

            //var mockInventoryRepo = new Mock<IInventoryDataRepo>(MockBehavior.Strict);

            //mockInventoryRepo
            //    .Setup(p => p.SearchInventoryCards(It.IsNotNull<InventoryQueryParameter>()))
            //    .ReturnsAsync(expectedSearchResult);

            //var mockScryService = new Mock<IScryfallService>(MockBehavior.Strict);

            //var cardSearchService = new CardSearchService(mockInventoryRepo.Object, mockScryService.Object);

            //InventoryQueryParameter filters = new InventoryQueryParameter()
            //{

            //};

            //Act
            var result = await inventoryService.ValidateCarpentryImport(payloadToSubmit);

            Assert.IsNotNull(result);


            //IEnumerable<MagicCardDto> result = await cardSearchService.SearchCardsFromInventory(filters);

            ////Assert
            //Assert.IsNotNull(result);
            //Assert.AreEqual(expectedSearchResult.Count, result.Count());
            Assert.Fail("Must confirm this is working as expected");
        }

        //Task AddValidatedCarpentryImport(ValidatedCarpentryImportDto dto);
        [TestMethod]
        public async Task InventoryService_AddValidatedCarpentryImport_Test()
        {

            //Assemble

            var mockInventoryRepo = new Mock<IInventoryDataRepo>(MockBehavior.Strict);

            var mockDataUpdateService = new Mock<IDataUpdateService>(MockBehavior.Strict);

            var mockCoreRepo = new Mock<ICoreDataRepo>(MockBehavior.Strict);

            var mockCardDatarepo = new Mock<ICardDataRepo>(MockBehavior.Strict);

            var inventoryService = new InventoryService(
                mockInventoryRepo.Object,
                mockDataUpdateService.Object,
                mockCoreRepo.Object,
                mockCardDatarepo.Object);

            var payloadToSubmit = new ValidatedCarpentryImportDto() { };

            //List<CardDataDto> expectedSearchResult = new List<CardDataDto>()
            //{
            //    CardInstance(),
            //    CardInstance(),
            //    CardInstance(),
            //    CardInstance(),
            //    CardInstance(),
            //};

            ////var mockQueryService = new Mock<IDataQueryService>(MockBehavior.Strict);

            //var mockInventoryRepo = new Mock<IInventoryDataRepo>(MockBehavior.Strict);

            //mockInventoryRepo
            //    .Setup(p => p.SearchInventoryCards(It.IsNotNull<InventoryQueryParameter>()))
            //    .ReturnsAsync(expectedSearchResult);

            //var mockScryService = new Mock<IScryfallService>(MockBehavior.Strict);

            //var cardSearchService = new CardSearchService(mockInventoryRepo.Object, mockScryService.Object);

            //InventoryQueryParameter filters = new InventoryQueryParameter()
            //{

            //};

            //Act
            await inventoryService.AddValidatedCarpentryImport(payloadToSubmit);

            //Assert
            //Returns void, nothing to assert
            //Assert.IsNotNull(result);
        }

        //Task<byte[]> ExportInventoryBackup();
        [TestMethod]
        public async Task InventoryService_ExportInventoryBackup_Test()
        {
            //Assemble

            var mockInventoryRepo = new Mock<IInventoryDataRepo>(MockBehavior.Strict);

            var mockDataUpdateService = new Mock<IDataUpdateService>(MockBehavior.Strict);

            var mockCoreRepo = new Mock<ICoreDataRepo>(MockBehavior.Strict);

            var mockCardDatarepo = new Mock<ICardDataRepo>(MockBehavior.Strict);

            var inventoryService = new InventoryService(
                mockInventoryRepo.Object,
                mockDataUpdateService.Object,
                mockCoreRepo.Object,
                mockCardDatarepo.Object);

            //List<CardDataDto> expectedSearchResult = new List<CardDataDto>()
            //{
            //    CardInstance(),
            //    CardInstance(),
            //    CardInstance(),
            //    CardInstance(),
            //    CardInstance(),
            //};

            ////var mockQueryService = new Mock<IDataQueryService>(MockBehavior.Strict);

            //var mockInventoryRepo = new Mock<IInventoryDataRepo>(MockBehavior.Strict);

            //mockInventoryRepo
            //    .Setup(p => p.SearchInventoryCards(It.IsNotNull<InventoryQueryParameter>()))
            //    .ReturnsAsync(expectedSearchResult);

            //var mockScryService = new Mock<IScryfallService>(MockBehavior.Strict);

            //var cardSearchService = new CardSearchService(mockInventoryRepo.Object, mockScryService.Object);

            //InventoryQueryParameter filters = new InventoryQueryParameter()
            //{

            //};

            //Act
            var result = await inventoryService.ExportInventoryBackup();

            //Assert
            Assert.IsNotNull(result);
            
            Assert.Fail("Must decide how to validate the response object");
        }
    }
}
