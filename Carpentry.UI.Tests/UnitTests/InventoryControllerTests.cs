using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
//using Carpentry.Service.Interfaces;
using Moq;
using Carpentry.Logic.Models;
using Microsoft.AspNetCore.Mvc;
using Carpentry.Logic.Interfaces;
using Carpentry.Data.QueryParameters;
using System;

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
            var mockService = new Mock<IInventoryService>(MockBehavior.Strict);

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

            var mockService = new Mock<IInventoryService>(MockBehavior.Strict);

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
            var mockService = new Mock<IInventoryService>(MockBehavior.Strict);

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
            Assert.IsInstanceOfType(response.Result, typeof(OkResult));

        }

        [TestMethod]
        public async Task Inventory_UpdateInventoryCard_ReturnsAsyncOK_Test()
        {
            //arrange
            var mockService = new Mock<IInventoryService>(MockBehavior.Strict);

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
            Assert.Fail();
            ////arrange
            //var mockService = new Mock<IInventoryService>(MockBehavior.Strict);

            //mockService
            //    .Setup(p => p.UpdateInventoryCard(It.IsNotNull<InventoryCardDto>()))
            //    .Returns(Task.CompletedTask);

            //var inventoryController = new Controllers.InventoryController(mockService.Object);
            //InventoryCardDto payload = new InventoryCardDto()
            //{

            //};

            ////act
            //var response = await inventoryController.UpdateInventoryCard(payload);

            ////assert
            //Assert.IsInstanceOfType(response, typeof(OkResult));

        }

        [TestMethod]
        public async Task Inventory_DeleteInventoryCard_ReturnsAsyncOK_Test()
        {
            //arrange
            var idToExpect = 1;

            var mockService = new Mock<IInventoryService>(MockBehavior.Strict);

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
            Assert.Fail();
            ////arrange
            //var idToExpect = 1;

            //var mockService = new Mock<IInventoryService>(MockBehavior.Strict);

            //mockService
            //    .Setup(p => p.DeleteInventoryCard(It.Is<int>(i => i == idToExpect)))
            //    .Returns(Task.CompletedTask);

            //var inventoryController = new Controllers.InventoryController(mockService.Object);

            ////act
            //var response = await inventoryController.DeleteCard(idToExpect);

            ////assert
            //Assert.IsInstanceOfType(response, typeof(OkResult));
        }

        [HttpPost("[action]")]
        public async Task<ActionResult> DeleteInventoryCardBatch(List<int> batchIDs)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Search

        [TestMethod]
        public async Task Inventory_SearchCards_ReturnsAsyncOK_Test()
        {
            //arrange
            var mockService = new Mock<IInventoryService>(MockBehavior.Strict);

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
            var mockService = new Mock<IInventoryService>(MockBehavior.Strict);

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
            Assert.Fail();
            ////arrange
            //var mockService = new Mock<IInventoryService>(MockBehavior.Strict);

            //InventoryDetailDto detailResult = new InventoryDetailDto()
            //{
            //    Cards = new List<MagicCardDto>(),
            //    InventoryCards = new List<InventoryCardDto>(),
            //    Name = "Mock Card Detail",
            //};

            //mockService
            //    .Setup(p => p.GetInventoryDetailByName(It.IsNotNull<string>()))
            //    .ReturnsAsync(detailResult);

            //var inventoryController = new Controllers.InventoryController(mockService.Object);

            ////string nameToRequest = "Mock Card Detail";
            //int idToRequest = 1;

            ////act
            //var response = await inventoryController.GetInventoryDetail(idToRequest);

            ////assert
            //var typedResult = response.Result as OkObjectResult;

            //InventoryDetailDto resultValue = typedResult.Value as InventoryDetailDto;
            //Assert.IsNotNull(resultValue);
            ////Assert.AreEqual(nameToRequest, resultValue.Name);

        }
        
        [TestMethod]
        public async Task Inventory_HideCollectionBuilderSuggestion_ReturnsAsyncOK_Test()
        {
            Assert.Fail();
            ////arrange
            //var mockService = new Mock<IInventoryService>(MockBehavior.Strict);

            //InventoryDetailDto detailResult = new InventoryDetailDto()
            //{
            //    Cards = new List<MagicCardDto>(),
            //    InventoryCards = new List<InventoryCardDto>(),
            //    Name = "Mock Card Detail",
            //};

            //mockService
            //    .Setup(p => p.GetInventoryDetailByName(It.IsNotNull<string>()))
            //    .ReturnsAsync(detailResult);

            //var inventoryController = new Controllers.InventoryController(mockService.Object);

            ////string nameToRequest = "Mock Card Detail";
            //int idToRequest = 1;

            ////act
            //var response = await inventoryController.GetInventoryDetail(idToRequest);

            ////assert
            //var typedResult = response.Result as OkObjectResult;

            //InventoryDetailDto resultValue = typedResult.Value as InventoryDetailDto;
            //Assert.IsNotNull(resultValue);
            ////Assert.AreEqual(nameToRequest, resultValue.Name);

        }

        #endregion

        #region Trimming Tips

        [TestMethod]
        public async Task Inventory_GetTrimmingTips_ReturnsAsyncOK_Test()
        {
            Assert.Fail();
            ////arrange
            //var mockService = new Mock<IInventoryService>(MockBehavior.Strict);

            //InventoryDetailDto detailResult = new InventoryDetailDto()
            //{
            //    Cards = new List<MagicCardDto>(),
            //    InventoryCards = new List<InventoryCardDto>(),
            //    Name = "Mock Card Detail",
            //};

            //mockService
            //    .Setup(p => p.GetInventoryDetailByName(It.IsNotNull<string>()))
            //    .ReturnsAsync(detailResult);

            //var inventoryController = new Controllers.InventoryController(mockService.Object);

            ////string nameToRequest = "Mock Card Detail";
            //int idToRequest = 1;

            ////act
            //var response = await inventoryController.GetInventoryDetail(idToRequest);

            ////assert
            //var typedResult = response.Result as OkObjectResult;

            //InventoryDetailDto resultValue = typedResult.Value as InventoryDetailDto;
            //Assert.IsNotNull(resultValue);
            ////Assert.AreEqual(nameToRequest, resultValue.Name);

        }

        [TestMethod]
        public async Task Inventory_HideTrimmingTip_ReturnsAsyncOK_Test()
        {
            Assert.Fail();
            ////arrange
            //var mockService = new Mock<IInventoryService>(MockBehavior.Strict);

            //InventoryDetailDto detailResult = new InventoryDetailDto()
            //{
            //    Cards = new List<MagicCardDto>(),
            //    InventoryCards = new List<InventoryCardDto>(),
            //    Name = "Mock Card Detail",
            //};

            //mockService
            //    .Setup(p => p.GetInventoryDetailByName(It.IsNotNull<string>()))
            //    .ReturnsAsync(detailResult);

            //var inventoryController = new Controllers.InventoryController(mockService.Object);

            ////string nameToRequest = "Mock Card Detail";
            //int idToRequest = 1;

            ////act
            //var response = await inventoryController.GetInventoryDetail(idToRequest);

            ////assert
            //var typedResult = response.Result as OkObjectResult;

            //InventoryDetailDto resultValue = typedResult.Value as InventoryDetailDto;
            //Assert.IsNotNull(resultValue);
            ////Assert.AreEqual(nameToRequest, resultValue.Name);

        }

        #endregion

        #region Import

        [TestMethod]
        public async Task Inventory_ValidateCarpentryImport_ReturnsAsyncOK_Test()
        {
            Assert.Fail();
            ////arrange
            //var mockService = new Mock<IInventoryService>(MockBehavior.Strict);

            //InventoryDetailDto detailResult = new InventoryDetailDto()
            //{
            //    Cards = new List<MagicCardDto>(),
            //    InventoryCards = new List<InventoryCardDto>(),
            //    Name = "Mock Card Detail",
            //};

            //mockService
            //    .Setup(p => p.GetInventoryDetailByName(It.IsNotNull<string>()))
            //    .ReturnsAsync(detailResult);

            //var inventoryController = new Controllers.InventoryController(mockService.Object);

            ////string nameToRequest = "Mock Card Detail";
            //int idToRequest = 1;

            ////act
            //var response = await inventoryController.GetInventoryDetail(idToRequest);

            ////assert
            //var typedResult = response.Result as OkObjectResult;

            //InventoryDetailDto resultValue = typedResult.Value as InventoryDetailDto;
            //Assert.IsNotNull(resultValue);
            ////Assert.AreEqual(nameToRequest, resultValue.Name);

        }
  
        [TestMethod]
        public async Task Inventory_AddValidatedCarpentryImport_ReturnsAsyncOK_Test()
        {
            Assert.Fail();
            ////arrange
            //var mockService = new Mock<IInventoryService>(MockBehavior.Strict);

            //InventoryDetailDto detailResult = new InventoryDetailDto()
            //{
            //    Cards = new List<MagicCardDto>(),
            //    InventoryCards = new List<InventoryCardDto>(),
            //    Name = "Mock Card Detail",
            //};

            //mockService
            //    .Setup(p => p.GetInventoryDetailByName(It.IsNotNull<string>()))
            //    .ReturnsAsync(detailResult);

            //var inventoryController = new Controllers.InventoryController(mockService.Object);

            ////string nameToRequest = "Mock Card Detail";
            //int idToRequest = 1;

            ////act
            //var response = await inventoryController.GetInventoryDetail(idToRequest);

            ////assert
            //var typedResult = response.Result as OkObjectResult;

            //InventoryDetailDto resultValue = typedResult.Value as InventoryDetailDto;
            //Assert.IsNotNull(resultValue);
            ////Assert.AreEqual(nameToRequest, resultValue.Name);

        }
        
        #endregion

        #region Export

        [TestMethod]
        public async Task Inventory_ExportInventoryBackup_ReturnsAsyncOK_Test()
        {
            Assert.Fail();
            ////arrange
            //var mockService = new Mock<IInventoryService>(MockBehavior.Strict);

            //InventoryDetailDto detailResult = new InventoryDetailDto()
            //{
            //    Cards = new List<MagicCardDto>(),
            //    InventoryCards = new List<InventoryCardDto>(),
            //    Name = "Mock Card Detail",
            //};

            //mockService
            //    .Setup(p => p.GetInventoryDetailByName(It.IsNotNull<string>()))
            //    .ReturnsAsync(detailResult);

            //var inventoryController = new Controllers.InventoryController(mockService.Object);

            ////string nameToRequest = "Mock Card Detail";
            //int idToRequest = 1;

            ////act
            //var response = await inventoryController.GetInventoryDetail(idToRequest);

            ////assert
            //var typedResult = response.Result as OkObjectResult;

            //InventoryDetailDto resultValue = typedResult.Value as InventoryDetailDto;
            //Assert.IsNotNull(resultValue);
            ////Assert.AreEqual(nameToRequest, resultValue.Name);

        }
        
        #endregion
    }
}
