using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using Carpentry.UI.Legacy.Models;
using Microsoft.AspNetCore.Mvc;
using Carpentry.UI.Legacy.Util;
using Carpentry.UI.Legacy.Controllers;
using Carpentry.Logic.Interfaces;
using Carpentry.Logic.Models;
using Carpentry.Data.QueryParameters;

namespace Carpentry.UI.Tests.Legacy.UnitTests
{
    [TestClass]
    public class InventoryControllerTests
    {
        [TestMethod]
        public void Inventory_GetStatus_ReturnsAsyncOK_Test()
        {
            //arrange
            var mockService = new Mock<IInventoryService>(MockBehavior.Strict);

            var mapperService = new MapperService();

            var inventoryController = new InventoryController(mockService.Object, mapperService);

            //act
            var response = inventoryController.Get();

            //assert
            Assert.IsInstanceOfType(response, typeof(OkObjectResult));
            var typedResult = response as OkObjectResult;
            string resultValue = typedResult.Value as string;

            Assert.AreEqual("Online", resultValue);
        }

        [TestMethod]
        public async Task Inventory_Add_ReturnsAsyncOK_Test()
        {
            //arrange
            int idToExpect = 1;

            var mockService = new Mock<IInventoryService>(MockBehavior.Strict);

            mockService
                .Setup(p => p.AddInventoryCard(It.IsNotNull<InventoryCardDto>()))
                .ReturnsAsync(idToExpect);

            var mapperService = new MapperService();

            var inventoryController = new InventoryController(mockService.Object, mapperService);

            LegacyInventoryCardDto payload = new LegacyInventoryCardDto()
            {

            };

            //act
            var response = await inventoryController.Add(payload);

            //assert
            Assert.IsInstanceOfType(response.Result, typeof(OkObjectResult));
            var typedResult = response.Result as OkObjectResult;
            Assert.AreEqual(idToExpect, typedResult.Value);
        }

        [TestMethod]
        public async Task Inventory_AddBatch_ReturnsAsyncOK_Test()
        {
            //arrange
            var mockService = new Mock<IInventoryService>(MockBehavior.Strict);

            mockService
                .Setup(p => p.AddInventoryCardBatch(It.IsNotNull<List<InventoryCardDto>>()))
                .Returns(Task.CompletedTask);

            var mapperService = new MapperService();

            var inventoryController = new InventoryController(mockService.Object, mapperService);

            List<LegacyInventoryCardDto> payload = new List<LegacyInventoryCardDto>()
            {
                new LegacyInventoryCardDto(){ },
                new LegacyInventoryCardDto(){ },
                new LegacyInventoryCardDto(){ },
                new LegacyInventoryCardDto(){ },
                new LegacyInventoryCardDto(){ },
            };

            //act
            var response = await inventoryController.AddBatch(payload);

            //assert
            Assert.IsInstanceOfType(response.Result, typeof(OkResult));

        }

        [TestMethod]
        public async Task Inventory_Update_ReturnsAsyncOK_Test()
        {
            //arrange
            var mockService = new Mock<IInventoryService>(MockBehavior.Strict);

            mockService
                .Setup(p => p.UpdateInventoryCard(It.IsNotNull<InventoryCardDto>()))
                .Returns(Task.CompletedTask);

            var mapperService = new MapperService();

            var inventoryController = new InventoryController(mockService.Object, mapperService);

            LegacyInventoryCardDto payload = new LegacyInventoryCardDto()
            {

            };

            //act
            var response = await inventoryController.Update(payload);

            //assert
            Assert.IsInstanceOfType(response, typeof(OkResult));

        }

        [TestMethod]
        public async Task Inventory_Delete_ReturnsAsyncOK_Test()
        {
            //arrange
            var idToExpect = 1;

            var mockService = new Mock<IInventoryService>(MockBehavior.Strict);

            mockService
                .Setup(p => p.DeleteInventoryCard(It.Is<int>(i => i == idToExpect)))
                .Returns(Task.CompletedTask);

            var mapperService = new MapperService();

            var inventoryController = new InventoryController(mockService.Object, mapperService);

            //act
            var response = await inventoryController.Delete(idToExpect);

            //assert
            Assert.IsInstanceOfType(response, typeof(OkResult));
        }

        [TestMethod]
        public async Task Inventory_Search_ReturnsAsyncOK_Test()
        {
            //arrange
            var mockService = new Mock<IInventoryService>(MockBehavior.Strict);

            IEnumerable<InventoryOverviewDto> searchResult = new List<InventoryOverviewDto>()
            {
                new InventoryOverviewDto() { },
                new InventoryOverviewDto() { },
                new InventoryOverviewDto() { },
                new InventoryOverviewDto() { },
                new InventoryOverviewDto() { },
            }.AsEnumerable();

            mockService
                .Setup(p => p.GetInventoryOverviews(It.IsNotNull<InventoryQueryParameter>()))
                .ReturnsAsync(searchResult);

            var mapperService = new MapperService();

            var inventoryController = new InventoryController(mockService.Object, mapperService);

            InventoryQueryParameter param = new InventoryQueryParameter()
            {

            };

            //act
            var response = await inventoryController.Search(param);

            //assert
            Assert.IsInstanceOfType(response.Result, typeof(OkObjectResult));
            var typedResult = response.Result as OkObjectResult;

            IEnumerable<LegacyInventoryOverviewDto> resultValue = typedResult.Value as IEnumerable<LegacyInventoryOverviewDto>;

            Assert.IsNotNull(resultValue);
            Assert.AreEqual(5, resultValue.Count());

        }

        [TestMethod]
        public async Task Inventory_GetByName_ReturnsAsyncOK_Test()
        {
            //arrange
            var mockService = new Mock<IInventoryService>(MockBehavior.Strict);

            InventoryDetailDto detailResult = new InventoryDetailDto()
            {
                Cards = new List<MagicCardDto>(),
                InventoryCards = new List<InventoryCardDto>(),
                Name = "Mock Card Detail",
            };

            mockService
                .Setup(p => p.GetInventoryDetailByName(It.IsNotNull<string>()))
                .ReturnsAsync(detailResult);

            var mapperService = new MapperService();

            var inventoryController = new InventoryController(mockService.Object, mapperService);

            string nameToRequest = "Mock Card Detail";

            //act
            var response = await inventoryController.GetByName(nameToRequest);

            //assert
            var typedResult = response.Result as OkObjectResult;

            LegacyInventoryDetailDto resultValue = typedResult.Value as LegacyInventoryDetailDto;
            Assert.IsNotNull(resultValue);
            Assert.AreEqual(nameToRequest, resultValue.Name);

        }
    }
}
