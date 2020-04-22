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

namespace Carpentry.UI.Tests.UnitTests
{
    /// <summary>
    /// I initially created a single mock service & controller intance in the test constructor
    /// Instead, I want to arrange & mock only the service methods I expect to see called
    /// </summary>
    [TestClass]
    public class InventoryControllerTests
    {
        [TestMethod]
        public void Inventory_GetStatus_ReturnsAsyncOK_Test()
        {
            //arrange
            var mockService = new Mock<IInventoryService>(MockBehavior.Strict);

            var inventoryController = new Controllers.InventoryController(mockService.Object);

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

            var inventoryController = new Controllers.InventoryController(mockService.Object);

            InventoryCardDto payload = new InventoryCardDto()
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

            var inventoryController = new Controllers.InventoryController(mockService.Object);
            InventoryCardDto payload = new InventoryCardDto()
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

            var inventoryController = new Controllers.InventoryController(mockService.Object);

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

            var inventoryController = new Controllers.InventoryController(mockService.Object);

            InventoryQueryParameter param = new InventoryQueryParameter()
            {

            };

            //act
            var response = await inventoryController.Search(param);

            //assert
            Assert.IsInstanceOfType(response.Result, typeof(OkObjectResult));
            var typedResult = response.Result as OkObjectResult;

            IEnumerable<InventoryOverviewDto> resultValue = typedResult.Value as IEnumerable<InventoryOverviewDto>;

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

            var inventoryController = new Controllers.InventoryController(mockService.Object);
            
            string nameToRequest = "Mock Card Detail";

            //act
            var response = await inventoryController.GetByName(nameToRequest);

            //assert
            var typedResult = response.Result as OkObjectResult;

            InventoryDetailDto resultValue = typedResult.Value as InventoryDetailDto;
            Assert.IsNotNull(resultValue);
            Assert.AreEqual(nameToRequest, resultValue.Name);
            
        }
    }
}
