using Carpentry.Data.QueryParameters;
using Carpentry.Logic.Interfaces;
using Carpentry.Logic.Models;
using Carpentry.Service.Implementations;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Carpentry.Service.Tests.UnitTests
{
    [TestClass]
    public class CarpentryInventoryServiceTests
    {
        #region Inventory Card add/update/delete

        [TestMethod]
        public async Task InventoryServiceTests_AddInventoryCard_Test()
        {
            //Assemble
            var mockInventoryService = new Mock<IInventoryService>(MockBehavior.Strict);
            var mockDataBackupService = new Mock<IDataExportService>(MockBehavior.Strict);
            var mockCardImportService = new Mock<IDataImportService>(MockBehavior.Strict);
            var mockCollectionBuilderService = new Mock<ICollectionBuilderService>(MockBehavior.Strict);
            var mockTrimmingTipsService = new Mock<ITrimmingTipsService>(MockBehavior.Strict);
            var mockSearchService = new Mock<ISearchService>(MockBehavior.Strict);

            int idToExpect = 1;

            InventoryCardDto newCard = new InventoryCardDto()
            {
                CardId = 1,
                IsFoil = false,
                StatusId = 1,
            };

            mockInventoryService
                .Setup(p => p.AddInventoryCard(It.IsNotNull<InventoryCardDto>()))
                .ReturnsAsync(idToExpect);

            var inventoryService = new CarpentryInventoryService(
                mockInventoryService.Object,
                mockDataBackupService.Object,
                mockCardImportService.Object,
                mockCollectionBuilderService.Object,
                mockTrimmingTipsService.Object,
                mockSearchService.Object
                );

            //Act
            var newId = await inventoryService.AddInventoryCard(newCard);

            //Assert
            Assert.AreEqual(idToExpect, newId);
        }

        [TestMethod]
        public async Task InventoryServiceTests_AddInventoryCardBatch_Test()
        {
            //Assemble
            var mockInventoryService = new Mock<IInventoryService>(MockBehavior.Strict);
            var mockDataBackupService = new Mock<IDataExportService>(MockBehavior.Strict);
            var mockCardImportService = new Mock<IDataImportService>(MockBehavior.Strict);
            var mockCollectionBuilderService = new Mock<ICollectionBuilderService>(MockBehavior.Strict);
            var mockTrimmingTipsService = new Mock<ITrimmingTipsService>(MockBehavior.Strict);
            var mockSearchService = new Mock<ISearchService>(MockBehavior.Strict);

            List<InventoryCardDto> cardBatch = new List<InventoryCardDto>()
            {
                new InventoryCardDto() { },
                new InventoryCardDto() { },
                new InventoryCardDto() { },
                new InventoryCardDto() { },
            };

            mockInventoryService
                .Setup(p => p.AddInventoryCardBatch(It.IsNotNull<List<InventoryCardDto>>()))
                .Returns(Task.CompletedTask);

            var inventoryService = new CarpentryInventoryService(
                mockInventoryService.Object,
                mockDataBackupService.Object,
                mockCardImportService.Object,
                mockCollectionBuilderService.Object,
                mockTrimmingTipsService.Object,
                mockSearchService.Object
                );

            //Act
            await inventoryService.AddInventoryCardBatch(cardBatch);

            //Assert
            //Task returns void, nothing to assert
        }

        [TestMethod]
        public async Task InventoryServiceTests_UpdateInventoryCard_Test()
        {
            //Assemble
            var mockInventoryService = new Mock<IInventoryService>(MockBehavior.Strict);
            var mockDataBackupService = new Mock<IDataExportService>(MockBehavior.Strict);
            var mockCardImportService = new Mock<IDataImportService>(MockBehavior.Strict);
            var mockCollectionBuilderService = new Mock<ICollectionBuilderService>(MockBehavior.Strict);
            var mockTrimmingTipsService = new Mock<ITrimmingTipsService>(MockBehavior.Strict);
            var mockSearchService = new Mock<ISearchService>(MockBehavior.Strict);

            InventoryCardDto cardToUpdate = new InventoryCardDto()
            {
                Id = 1,
                CardId = 1,
                StatusId = 2,
            };

            mockInventoryService
                .Setup(p => p.UpdateInventoryCard(It.IsAny<InventoryCardDto>()))
                .Returns(Task.CompletedTask);

            var inventoryService = new CarpentryInventoryService(
                mockInventoryService.Object,
                mockDataBackupService.Object,
                mockCardImportService.Object,
                mockCollectionBuilderService.Object,
                mockTrimmingTipsService.Object,
                mockSearchService.Object
                );

            //Act
            await inventoryService.UpdateInventoryCard(cardToUpdate);

            //Assert
            //Task returns void, nothing to assert
        }

        [TestMethod]
        public async Task InventoryServiceTests_UpdateInventoryCardBatch_Test()
        {
            //Assemble
            var mockInventoryService = new Mock<IInventoryService>(MockBehavior.Strict);
            var mockDataBackupService = new Mock<IDataExportService>(MockBehavior.Strict);
            var mockCardImportService = new Mock<IDataImportService>(MockBehavior.Strict);
            var mockCollectionBuilderService = new Mock<ICollectionBuilderService>(MockBehavior.Strict);
            var mockTrimmingTipsService = new Mock<ITrimmingTipsService>(MockBehavior.Strict);
            var mockSearchService = new Mock<ISearchService>(MockBehavior.Strict);


            var cardsToUpdate = new List<InventoryCardDto>()
            {
                new InventoryCardDto()
                {
                    Id = 1,
                    StatusId = 2,
                },
                new InventoryCardDto()
                {
                    Id = 2,
                    StatusId = 3,
                },
                new InventoryCardDto()
                {
                    Id = 3,
                    StatusId = 1,
                },
            };

            mockInventoryService
                .Setup(p => p.UpdateInventoryCardBatch(It.IsNotNull<IEnumerable<InventoryCardDto>>()))
                .Returns(Task.CompletedTask);

            var inventoryService = new CarpentryInventoryService(
                mockInventoryService.Object,
                mockDataBackupService.Object,
                mockCardImportService.Object,
                mockCollectionBuilderService.Object,
                mockTrimmingTipsService.Object,
                mockSearchService.Object
                );

            //Act
            await inventoryService.UpdateInventoryCardBatch(cardsToUpdate);

            //Assert
            //Task returns void, nothing to assert
        }

        [TestMethod]
        public async Task InventoryServiceTests_DeleteInventoryCard_Test()
        {
            //Assemble
            var mockInventoryService = new Mock<IInventoryService>(MockBehavior.Strict);
            var mockDataBackupService = new Mock<IDataExportService>(MockBehavior.Strict);
            var mockCardImportService = new Mock<IDataImportService>(MockBehavior.Strict);
            var mockCollectionBuilderService = new Mock<ICollectionBuilderService>(MockBehavior.Strict);
            var mockTrimmingTipsService = new Mock<ITrimmingTipsService>(MockBehavior.Strict);
            var mockSearchService = new Mock<ISearchService>(MockBehavior.Strict);

            int idToDelete = 1;

            mockInventoryService
                .Setup(p => p.DeleteInventoryCard(It.Is<int>(i => i == idToDelete)))
                .Returns(Task.CompletedTask);

            var inventoryService = new CarpentryInventoryService(
                mockInventoryService.Object,
                mockDataBackupService.Object,
                mockCardImportService.Object,
                mockCollectionBuilderService.Object,
                mockTrimmingTipsService.Object,
                mockSearchService.Object
                );

            //Act
            await inventoryService.DeleteInventoryCard(idToDelete);

            //Assert
            //Task returns void, nothing to assert
        }

        [TestMethod]
        public async Task InventoryServiceTests_DeleteInventoryCardBatch_Test()
        {
            //Assemble
            var mockInventoryService = new Mock<IInventoryService>(MockBehavior.Strict);
            var mockDataBackupService = new Mock<IDataExportService>(MockBehavior.Strict);
            var mockCardImportService = new Mock<IDataImportService>(MockBehavior.Strict);
            var mockCollectionBuilderService = new Mock<ICollectionBuilderService>(MockBehavior.Strict);
            var mockTrimmingTipsService = new Mock<ITrimmingTipsService>(MockBehavior.Strict);
            var mockSearchService = new Mock<ISearchService>(MockBehavior.Strict);

            var idsToDelete = new List<int> { 1, 2, 3 };

            mockInventoryService
                .Setup(p => p.DeleteInventoryCardBatch(It.IsAny<IEnumerable<int>>()))
                .Returns(Task.CompletedTask);

            var inventoryService = new CarpentryInventoryService(
                mockInventoryService.Object,
                mockDataBackupService.Object,
                mockCardImportService.Object,
                mockCollectionBuilderService.Object,
                mockTrimmingTipsService.Object,
                mockSearchService.Object
                );

            //Act
            await inventoryService.DeleteInventoryCardBatch(idsToDelete);

            //Assert
            //Task returns void, nothing to assert
        }

        #endregion

        #region Search

        [TestMethod]
        public async Task InventoryServiceTests_GetInventoryOverviews_Test()
        {
            //Assemble
            var mockInventoryService = new Mock<IInventoryService>(MockBehavior.Strict);
            var mockDataBackupService = new Mock<IDataExportService>(MockBehavior.Strict);
            var mockCardImportService = new Mock<IDataImportService>(MockBehavior.Strict);
            var mockCollectionBuilderService = new Mock<ICollectionBuilderService>(MockBehavior.Strict);
            var mockTrimmingTipsService = new Mock<ITrimmingTipsService>(MockBehavior.Strict);
            var mockSearchService = new Mock<ISearchService>(MockBehavior.Strict);


            List<InventoryOverviewDto> expectedOverviewResult = new List<InventoryOverviewDto>()
            {
                new InventoryOverviewDto(),
                new InventoryOverviewDto(),
                new InventoryOverviewDto(),
            };

            mockSearchService
                .Setup(p => p.SearchInventory(It.IsNotNull<InventoryQueryParameter>()))
                .ReturnsAsync(expectedOverviewResult);

            var inventoryService = new CarpentryInventoryService(
                mockInventoryService.Object,
                mockDataBackupService.Object,
                mockCardImportService.Object,
                mockCollectionBuilderService.Object,
                mockTrimmingTipsService.Object,
                mockSearchService.Object
                );

            InventoryQueryParameter queryParamToRequest = new InventoryQueryParameter()
            {
                //GroupBy = "unique",
                //Sort = "price",
                //SortDescending = true,
                //Skip = 0,
                //Take = 100,
                //Rarity = new List<string>() { "common", "uncommon" },
            };

            //Act
            var overivews = await inventoryService.GetInventoryOverviews(queryParamToRequest);

            //Assert
            Assert.IsNotNull(overivews);
            Assert.AreEqual(expectedOverviewResult.Count, overivews.Count);
        }

        [TestMethod]
        public async Task InventoryServiceTests_GetInventoryDetail_Test()
        {
            //Assemble
            var mockInventoryService = new Mock<IInventoryService>(MockBehavior.Strict);
            var mockDataBackupService = new Mock<IDataExportService>(MockBehavior.Strict);
            var mockCardImportService = new Mock<IDataImportService>(MockBehavior.Strict);
            var mockCollectionBuilderService = new Mock<ICollectionBuilderService>(MockBehavior.Strict);
            var mockTrimmingTipsService = new Mock<ITrimmingTipsService>(MockBehavior.Strict);
            var mockSearchService = new Mock<ISearchService>(MockBehavior.Strict);

            var expectedResult = new InventoryDetailDto();

            mockInventoryService
                .Setup(p => p.GetInventoryDetail(It.Is<int>(i => i > 0)))
                .ReturnsAsync(expectedResult);

            var inventoryService = new CarpentryInventoryService(
                mockInventoryService.Object,
                mockDataBackupService.Object,
                mockCardImportService.Object,
                mockCollectionBuilderService.Object,
                mockTrimmingTipsService.Object,
                mockSearchService.Object
                );

            int idToRequest = 1;

            //Act
            var result = await inventoryService.GetInventoryDetail(idToRequest);

            //Assert
            Assert.IsNotNull(result);
        }

        #endregion

        #region Collection Builder

        [TestMethod]
        public async Task InventoryService_GetCollectionBuilderSuggestions_ThrowsNotImplemented_Test()
        {
            var mockInventoryService = new Mock<IInventoryService>(MockBehavior.Strict);
            var mockDataBackupService = new Mock<IDataExportService>(MockBehavior.Strict);
            var mockCardImportService = new Mock<IDataImportService>(MockBehavior.Strict);
            var mockCollectionBuilderService = new Mock<ICollectionBuilderService>(MockBehavior.Strict);
            var mockTrimmingTipsService = new Mock<ITrimmingTipsService>(MockBehavior.Strict);
            var mockSearchService = new Mock<ISearchService>(MockBehavior.Strict);

            var mockSuggestions = new List<InventoryOverviewDto>()
            {
                new InventoryOverviewDto(){ },
                new InventoryOverviewDto(){ },
                new InventoryOverviewDto(){ },
            };

            mockCollectionBuilderService
                .Setup(p => p.GetCollectionBuilderSuggestions())
                .ReturnsAsync(mockSuggestions);

            var inventoryService = new CarpentryInventoryService(
                mockInventoryService.Object,
                mockDataBackupService.Object,
                mockCardImportService.Object,
                mockCollectionBuilderService.Object,
                mockTrimmingTipsService.Object,
                mockSearchService.Object
                );

            //Act
            var result = await inventoryService.GetCollectionBuilderSuggestions();

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(mockSuggestions.Count, result.Count);

        }

        [TestMethod]
        public async Task InventoryService_HideCollectionBuilderSuggestion_ThrowsNotImplemented_Test()
        {
            var mockInventoryService = new Mock<IInventoryService>(MockBehavior.Strict);
            var mockDataBackupService = new Mock<IDataExportService>(MockBehavior.Strict);
            var mockCardImportService = new Mock<IDataImportService>(MockBehavior.Strict);
            var mockCollectionBuilderService = new Mock<ICollectionBuilderService>(MockBehavior.Strict);
            var mockTrimmingTipsService = new Mock<ITrimmingTipsService>(MockBehavior.Strict);
            var mockSearchService = new Mock<ISearchService>(MockBehavior.Strict);

            mockCollectionBuilderService
                .Setup(p => p.HideCollectionBuilderSuggestion(It.IsNotNull<InventoryOverviewDto>()))
                .Returns(Task.CompletedTask);

            var inventoryService = new CarpentryInventoryService(
                mockInventoryService.Object,
                mockDataBackupService.Object,
                mockCardImportService.Object,
                mockCollectionBuilderService.Object,
                mockTrimmingTipsService.Object,
                mockSearchService.Object
                );

            var payloadToSubmit = new InventoryOverviewDto() { };

            //Act
            await inventoryService.HideCollectionBuilderSuggestion(payloadToSubmit);

            //Assert
            //Returns void, nothing to assert
        }

        #endregion

        #region Trimming Tips

        [TestMethod]
        public async Task InventoryService_GetTrimmingTips_Test()
        {
            var mockInventoryService = new Mock<IInventoryService>(MockBehavior.Strict);
            var mockDataBackupService = new Mock<IDataExportService>(MockBehavior.Strict);
            var mockCardImportService = new Mock<IDataImportService>(MockBehavior.Strict);
            var mockCollectionBuilderService = new Mock<ICollectionBuilderService>(MockBehavior.Strict);
            var mockTrimmingTipsService = new Mock<ITrimmingTipsService>(MockBehavior.Strict);
            var mockSearchService = new Mock<ISearchService>(MockBehavior.Strict);

            var mockTrimmingTips = new List<InventoryOverviewDto>()
            {
                new InventoryOverviewDto(){ },
                new InventoryOverviewDto(){ },
                new InventoryOverviewDto(){ },
            };

            mockTrimmingTipsService
                .Setup(p => p.GetTrimmingTips())
                .ReturnsAsync(mockTrimmingTips);

            var inventoryService = new CarpentryInventoryService(
                mockInventoryService.Object,
                mockDataBackupService.Object,
                mockCardImportService.Object,
                mockCollectionBuilderService.Object,
                mockTrimmingTipsService.Object,
                mockSearchService.Object
                );

            //Act
            var result = await inventoryService.GetTrimmingTips();

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(mockTrimmingTips.Count, result.Count);

        }

        [TestMethod]
        public async Task InventoryService_HideTrimmingTip_Test()
        {
            var mockInventoryService = new Mock<IInventoryService>(MockBehavior.Strict);
            var mockDataBackupService = new Mock<IDataExportService>(MockBehavior.Strict);
            var mockCardImportService = new Mock<IDataImportService>(MockBehavior.Strict);
            var mockCollectionBuilderService = new Mock<ICollectionBuilderService>(MockBehavior.Strict);
            var mockTrimmingTipsService = new Mock<ITrimmingTipsService>(MockBehavior.Strict);
            var mockSearchService = new Mock<ISearchService>(MockBehavior.Strict);

            mockTrimmingTipsService
                .Setup(p => p.HideTrimmingTip(It.IsNotNull<InventoryOverviewDto>()))
                .Returns(Task.CompletedTask);

            var inventoryService = new CarpentryInventoryService(
                mockInventoryService.Object,
                mockDataBackupService.Object,
                mockCardImportService.Object,
                mockCollectionBuilderService.Object,
                mockTrimmingTipsService.Object,
                mockSearchService.Object
                );

            var payloadToSubmit = new InventoryOverviewDto() { };

            //Act
            await inventoryService.HideTrimmingTip(payloadToSubmit);

            //Assert
            //Returns void, nothing to assert
        }
        
        #endregion

        #region Import/Export

        [TestMethod]
        public async Task InventoryService_ValidateCarpentryImport_Test()
        {
            //Assemble
            var mockInventoryService = new Mock<IInventoryService>(MockBehavior.Strict);
            var mockDataBackupService = new Mock<IDataExportService>(MockBehavior.Strict);
            var mockCardImportService = new Mock<IDataImportService>(MockBehavior.Strict);
            var mockCollectionBuilderService = new Mock<ICollectionBuilderService>(MockBehavior.Strict);
            var mockTrimmingTipsService = new Mock<ITrimmingTipsService>(MockBehavior.Strict);
            var mockSearchService = new Mock<ISearchService>(MockBehavior.Strict);

            var expectedResult = new ValidatedCarpentryImportDto();

            mockCardImportService
                .Setup(p => p.ValidateCarpentryImport(It.IsNotNull<CardImportDto>()))
                .ReturnsAsync(expectedResult);

            var inventoryService = new CarpentryInventoryService(
                mockInventoryService.Object,
                mockDataBackupService.Object,
                mockCardImportService.Object,
                mockCollectionBuilderService.Object,
                mockTrimmingTipsService.Object,
                mockSearchService.Object
                );

            var payloadToSubmit = new CardImportDto() { };

            //Act
            var result = await inventoryService.ValidateCarpentryImport(payloadToSubmit);

            //Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public async Task InventoryService_AddValidatedCarpentryImport_Test()
        {
            //Assemble
            var mockInventoryService = new Mock<IInventoryService>(MockBehavior.Strict);
            var mockDataBackupService = new Mock<IDataExportService>(MockBehavior.Strict);
            var mockCardImportService = new Mock<IDataImportService>(MockBehavior.Strict);
            var mockCollectionBuilderService = new Mock<ICollectionBuilderService>(MockBehavior.Strict);
            var mockTrimmingTipsService = new Mock<ITrimmingTipsService>(MockBehavior.Strict);
            var mockSearchService = new Mock<ISearchService>(MockBehavior.Strict);

            mockCardImportService
                .Setup(p => p.AddValidatedCarpentryImport(It.IsNotNull<ValidatedCarpentryImportDto>()))
                .Returns(Task.CompletedTask);

            var inventoryService = new CarpentryInventoryService(
                mockInventoryService.Object,
                mockDataBackupService.Object,
                mockCardImportService.Object,
                mockCollectionBuilderService.Object,
                mockTrimmingTipsService.Object,
                mockSearchService.Object
                );

            var payloadToSubmit = new ValidatedCarpentryImportDto() { };

            //Act
            await inventoryService.AddValidatedCarpentryImport(payloadToSubmit);

            //Assert
            //Returns void, nothing to assert
        }

        [TestMethod]
        public async Task InventoryService_ExportInventoryBackup_Test()
        {
            //Assemble
            var mockInventoryService = new Mock<IInventoryService>(MockBehavior.Strict);
            var mockExportService = new Mock<IDataExportService>(MockBehavior.Strict);
            var mockCardImportService = new Mock<IDataImportService>(MockBehavior.Strict);
            var mockCollectionBuilderService = new Mock<ICollectionBuilderService>(MockBehavior.Strict);
            var mockTrimmingTipsService = new Mock<ITrimmingTipsService>(MockBehavior.Strict);
            var mockSearchService = new Mock<ISearchService>(MockBehavior.Strict);

            var expectedResult = new byte[] { };

            mockExportService
                .Setup(p => p.GenerateZipBackup())
                .ReturnsAsync(expectedResult);

            var inventoryService = new CarpentryInventoryService(
                mockInventoryService.Object,
                mockExportService.Object,
                mockCardImportService.Object,
                mockCollectionBuilderService.Object,
                mockTrimmingTipsService.Object,
                mockSearchService.Object
                );

            //Act
            var result = await inventoryService.ExportInventoryBackup();

            //Assert
            Assert.IsNotNull(result);
        }
        
        #endregion
    }
}
