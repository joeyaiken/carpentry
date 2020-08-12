using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using Carpentry.Logic.Models;
using Microsoft.AspNetCore.Mvc;
using Carpentry.Data.QueryParameters;
using Carpentry.Service.Interfaces;

namespace Carpentry.UI.Tests.UnitTests
{
    /// <summary>
    /// I initially created a single mock service & controller intance in the test constructor
    /// Instead, I want to arrange & mock only the service methods I expect to see called
    /// </summary>
    [TestClass]
    public class InventoryControllerTests
    {
        #region Status

        [TestMethod]
        public void Inventory_GetStatus_ReturnsAsyncOK_Test()
        {
            //arrange
            var mockService = new Mock<ICarpentryInventoryService>(MockBehavior.Strict);

            var inventoryController = new Controllers.InventoryController(mockService.Object);

            //act
            var response = inventoryController.GetStatus();

            //assert
            Assert.IsInstanceOfType(response, typeof(OkObjectResult));
            var typedResult = response as OkObjectResult;
            string resultValue = typedResult.Value as string;

            Assert.AreEqual("Online", resultValue);
        }

        #endregion

        #region Inventory Cards

        [TestMethod]
        public async Task Inventory_AddInventoryCard_ReturnsAsyncOK_Test()
        {
            //arrange
            int idToExpect = 1;

            var mockService = new Mock<ICarpentryInventoryService>(MockBehavior.Strict);

            mockService
                .Setup(p => p.AddInventoryCard(It.IsNotNull<InventoryCardDto>()))
                .ReturnsAsync(idToExpect);

            var inventoryController = new Controllers.InventoryController(mockService.Object);

            InventoryCardDto payload = new InventoryCardDto()
            {

            };

            //act
            var response = await inventoryController.AddInventoryCard(payload);

            //assert
            Assert.IsInstanceOfType(response.Result, typeof(OkObjectResult));
            var typedResult = response.Result as OkObjectResult;
            Assert.AreEqual(idToExpect, typedResult.Value);
        }

        [TestMethod]
        public async Task Inventory_AddInventoryCardBatch_ReturnsAsyncOK_Test()
        {
            //arrange
            var mockService = new Mock<ICarpentryInventoryService>(MockBehavior.Strict);

            mockService
                .Setup(p => p.AddInventoryCardBatch(It.IsNotNull<List<InventoryCardDto>>()))
                .Returns(Task.CompletedTask);

            var inventoryController = new Controllers.InventoryController(mockService.Object);

            List<InventoryCardDto> payload = new List<InventoryCardDto>()
            {
                new InventoryCardDto(){ },
                new InventoryCardDto(){ },
                new InventoryCardDto(){ },
                new InventoryCardDto(){ },
                new InventoryCardDto(){ },
            };

            //act
            var response = await inventoryController.AddInventoryCardBatch(payload);

            //assert
            Assert.IsInstanceOfType(response, typeof(OkResult));

        }

        [TestMethod]
        public async Task Inventory_UpdateInventoryCard_ReturnsAsyncOK_Test()
        {
            //arrange
            var mockService = new Mock<ICarpentryInventoryService>(MockBehavior.Strict);

            mockService
                .Setup(p => p.UpdateInventoryCard(It.IsNotNull<InventoryCardDto>()))
                .Returns(Task.CompletedTask);

            var inventoryController = new Controllers.InventoryController(mockService.Object);
            InventoryCardDto payload = new InventoryCardDto()
            {

            };

            //act
            var response = await inventoryController.UpdateInventoryCard(payload);

            //assert
            Assert.IsInstanceOfType(response, typeof(OkResult));

        }

        [TestMethod]
        public async Task Inventory_UpdateInventoryCardBatch_ReturnsAsyncOK_Test()
        {
            //arrange
            var mockService = new Mock<ICarpentryInventoryService>(MockBehavior.Strict);

            mockService
                .Setup(p => p.UpdateInventoryCardBatch(It.IsNotNull<IEnumerable<InventoryCardDto>>()))
                .Returns(Task.CompletedTask);

            var inventoryController = new Controllers.InventoryController(mockService.Object);
            var payload = new List<InventoryCardDto>()
            {
                new InventoryCardDto(){ },
                new InventoryCardDto(){ },
                new InventoryCardDto(){ },
            };

            //act
            var response = await inventoryController.UpdateInventoryCardBatch(payload);

            //assert
            Assert.IsInstanceOfType(response, typeof(OkResult));

        }

        [TestMethod]
        public async Task Inventory_DeleteInventoryCard_ReturnsAsyncOK_Test()
        {
            //arrange
            var idToExpect = 1;

            var mockService = new Mock<ICarpentryInventoryService>(MockBehavior.Strict);

            mockService
                .Setup(p => p.DeleteInventoryCard(It.Is<int>(i => i == idToExpect)))
                .Returns(Task.CompletedTask);

            var inventoryController = new Controllers.InventoryController(mockService.Object);

            //act
            var response = await inventoryController.DeleteInventoryCard(idToExpect);

            //assert
            Assert.IsInstanceOfType(response, typeof(OkResult));
        }

        [TestMethod]
        public async Task Inventory_DeleteInventoryCardBatch_ReturnsAsyncOK_Test()
        {
            //arrange
            var idsToSubmit = new List<int> { 1, 2, 3, 4, 5 };

            var mockService = new Mock<ICarpentryInventoryService>(MockBehavior.Strict);

            mockService
                .Setup(p => p.DeleteInventoryCardBatch(It.IsAny<List<int>>()))
                .Returns(Task.CompletedTask);

            var inventoryController = new Controllers.InventoryController(mockService.Object);

            //act
            var response = await inventoryController.DeleteInventoryCardBatch(idsToSubmit);

            //assert
            Assert.IsInstanceOfType(response, typeof(OkResult));
        }

        #endregion

        #region Search

        [TestMethod]
        public async Task Inventory_SearchCards_ReturnsAsyncOK_Test()
        {
            //arrange
            var mockService = new Mock<ICarpentryInventoryService>(MockBehavior.Strict);

            List<InventoryOverviewDto> searchResult = new List<InventoryOverviewDto>()
            {
                new InventoryOverviewDto() { },
                new InventoryOverviewDto() { },
                new InventoryOverviewDto() { },
                new InventoryOverviewDto() { },
                new InventoryOverviewDto() { },
            };

            mockService
                .Setup(p => p.GetInventoryOverviews(It.IsNotNull<InventoryQueryParameter>()))
                .ReturnsAsync(searchResult);

            var inventoryController = new Controllers.InventoryController(mockService.Object);

            InventoryQueryParameter param = new InventoryQueryParameter()
            {

            };

            //act
            var response = await inventoryController.SearchCards(param);

            //assert
            Assert.IsInstanceOfType(response.Result, typeof(OkObjectResult));
            var typedResult = response.Result as OkObjectResult;

            IEnumerable<InventoryOverviewDto> resultValue = typedResult.Value as IEnumerable<InventoryOverviewDto>;

            Assert.IsNotNull(resultValue);
            Assert.AreEqual(5, resultValue.Count());

        }

        [TestMethod]
        public async Task Inventory_GetInventoryDetail_ReturnsAsyncOK_Test()
        {
            //arrange
            var mockService = new Mock<ICarpentryInventoryService>(MockBehavior.Strict);

            int idToRequest = 1;

            InventoryDetailDto detailResult = new InventoryDetailDto()
            {
                Cards = new List<MagicCardDto>(),
                InventoryCards = new List<InventoryCardDto>(),
                Name = "Mock Card Detail",
            };

            mockService
                .Setup(p => p.GetInventoryDetail(It.Is<int>(i => i == idToRequest)))
                .ReturnsAsync(detailResult);

            var inventoryController = new Controllers.InventoryController(mockService.Object);

            //act
            var response = await inventoryController.GetInventoryDetail(idToRequest);

            //assert
            var typedResult = response.Result as OkObjectResult;

            InventoryDetailDto resultValue = typedResult.Value as InventoryDetailDto;
            Assert.IsNotNull(resultValue);
            //Assert.AreEqual(nameToRequest, resultValue.Name);

        }

        #endregion

        #region Collection Builder

        [TestMethod]
        public async Task Inventory_GetCollectionBuilderSuggestions_ReturnsAsyncOK_Test()
        {
            //arrange
            var mockService = new Mock<ICarpentryInventoryService>(MockBehavior.Strict);

            var serviceResult = new List<InventoryOverviewDto>()
            {
                new InventoryOverviewDto(),
                new InventoryOverviewDto(),
                new InventoryOverviewDto(),
            };

            mockService
                .Setup(p => p.GetCollectionBuilderSuggestions())
                .ReturnsAsync(serviceResult);

            var inventoryController = new Controllers.InventoryController(mockService.Object);
            
            //act
            var response = await inventoryController.GetCollectionBuilderSuggestions();

            //assert
            var typedResult = response.Result as OkObjectResult;

            var resultValue = typedResult.Value as List<InventoryOverviewDto>;
            Assert.IsNotNull(resultValue);
            Assert.AreEqual(serviceResult.Count, resultValue.Count);
        }

        [TestMethod]
        public async Task Inventory_HideCollectionBuilderSuggestion_ReturnsAsyncOK_Test()
        {
            //arrange
            var mockService = new Mock<ICarpentryInventoryService>(MockBehavior.Strict);

            mockService
                .Setup(p => p.HideCollectionBuilderSuggestion(It.IsNotNull<InventoryOverviewDto>()))
                .Returns(Task.CompletedTask);

            var inventoryController = new Controllers.InventoryController(mockService.Object);

            var payloadToSubmit = new InventoryOverviewDto() { };

            //act
            var response = await inventoryController.HideCollectionBuilderSuggestion(payloadToSubmit);

            //assert
            Assert.IsInstanceOfType(response, typeof(OkResult));
        }

        #endregion

        #region Trimming Tips

        [TestMethod]
        public async Task Inventory_GetTrimmingTips_ReturnsAsyncOK_Test()
        {
            //arrange
            var mockService = new Mock<ICarpentryInventoryService>(MockBehavior.Strict);

            var serviceResult = new List<InventoryOverviewDto>()
            {
                new InventoryOverviewDto(),
                new InventoryOverviewDto(),
                new InventoryOverviewDto(),
            };

            mockService
                .Setup(p => p.GetTrimmingTips())
                .ReturnsAsync(serviceResult);

            var inventoryController = new Controllers.InventoryController(mockService.Object);

            //act
            var response = await inventoryController.GetTrimmingTips();

            //assert
            var typedResult = response.Result as OkObjectResult;

            var resultValue = typedResult.Value as List<InventoryOverviewDto>;
            Assert.IsNotNull(resultValue);
            Assert.AreEqual(serviceResult.Count, resultValue.Count);
        }

        [TestMethod]
        public async Task Inventory_HideTrimmingTip_ReturnsAsyncOK_Test()
        {
            //arrange
            var mockService = new Mock<ICarpentryInventoryService>(MockBehavior.Strict);

            mockService
                .Setup(p => p.HideTrimmingTip(It.IsNotNull<InventoryOverviewDto>()))
                .Returns(Task.CompletedTask);

            var inventoryController = new Controllers.InventoryController(mockService.Object);

            var payloadToSubmit = new InventoryOverviewDto() { };

            //act
            var response = await inventoryController.HideTrimmingTip(payloadToSubmit);

            //assert
            Assert.IsInstanceOfType(response, typeof(OkResult));
        }

        #endregion

        #region Import

        [TestMethod]
        public async Task Inventory_ValidateCarpentryImport_ReturnsAsyncOK_Test()
        {
            //arrange
            var mockService = new Mock<ICarpentryInventoryService>(MockBehavior.Strict);

            var validatedImport = new ValidatedCarpentryImportDto()
            {

            };

            mockService
                .Setup(p => p.ValidateCarpentryImport(It.IsNotNull<CardImportDto>()))
                .ReturnsAsync(validatedImport);

            var inventoryController = new Controllers.InventoryController(mockService.Object);

            var importToSubmit = new CardImportDto()
            {

            };

            //act
            var response = await inventoryController.ValidateCarpentryImport(importToSubmit);

            //assert
            var typedResult = response.Result as OkObjectResult;

            ValidatedCarpentryImportDto resultValue = typedResult.Value as ValidatedCarpentryImportDto;
            Assert.IsNotNull(resultValue);
        }

        [TestMethod]
        public async Task Inventory_AddValidatedCarpentryImport_ReturnsAsyncOK_Test()
        {
            //arrange
            var mockService = new Mock<ICarpentryInventoryService>(MockBehavior.Strict);

            mockService
                .Setup(p => p.AddValidatedCarpentryImport(It.IsNotNull<ValidatedCarpentryImportDto>()))
                .Returns(Task.CompletedTask);

            var inventoryController = new Controllers.InventoryController(mockService.Object);

            var payloadToSubmit = new ValidatedCarpentryImportDto() { };

            //act
            var response = await inventoryController.AddValidatedCarpentryImport(payloadToSubmit);

            //assert
            Assert.IsInstanceOfType(response, typeof(OkResult));
        }
        
        #endregion

        #region Export

        [TestMethod]
        public async Task Inventory_ExportInventoryBackup_ReturnsAsyncOK_Test()
        {
            //arrange
            var mockService = new Mock<ICarpentryInventoryService>(MockBehavior.Strict);

            InventoryDetailDto detailResult = new InventoryDetailDto()
            {
                Cards = new List<MagicCardDto>(),
                InventoryCards = new List<InventoryCardDto>(),
                Name = "Mock Card Detail",
            };

            var mockResult = new byte[0];

            mockService
                .Setup(p => p.ExportInventoryBackup())
                .ReturnsAsync(mockResult);

            var inventoryController = new Controllers.InventoryController(mockService.Object);

            //act
            var response = await inventoryController.ExportInventoryBackup();

            //assert
            var typedResponse = response as FileContentResult;
            byte[] resultValue = typedResponse.FileContents;
            Assert.IsNotNull(resultValue);
        }

        #endregion
    }
}
