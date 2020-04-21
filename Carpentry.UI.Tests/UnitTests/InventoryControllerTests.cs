using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Carpentry.Service.Interfaces;
using Moq;
using Carpentry.Service.Models;
using Microsoft.AspNetCore.Mvc;

namespace Carpentry.UI.Tests.UnitTests
{
    [TestClass]
    public class InventoryControllerTests
    {
        //readonly CarpentryFactory _factory;
        //readonly HttpClient _client;
        //private readonly Controllers.InventoryController _inventoryController;

        /// <summary>
        /// I initially created a single mock service & controller intance in the test constructor
        /// Instead, I want to arrange & mock only the service methods I expect to see called
        /// </summary>
        public InventoryControllerTests()
        {
            ////_factory = new CarpentryFactory();
            ////_client = _factory.CreateClient();

            //var mockService = new Mock<IInventoryService>(MockBehavior.Strict);

            ////Add
            //mockService
            //    .Setup(p => p.AddInventoryCard(It.IsNotNull<InventoryCard>()))
            //    .ReturnsAsync(1);

            ////AddBatch
            //mockService
            //    .Setup(p => p.AddInventoryCardBatch(It.IsNotNull<List<InventoryCard>>()))
            //    .Returns(Task.CompletedTask);

            ////Update
            //mockService
            //    .Setup(p => p.UpdateInventoryCard(It.IsNotNull<InventoryCard>()))
            //    .Returns(Task.CompletedTask);

            ////Delete
            //mockService
            //    .Setup(p => p.DeleteInventoryCard(It.Is<int>(i => i > 0)))
            //    .Returns(Task.CompletedTask);

            ////Search
            //IEnumerable<InventoryOverview> searchResult = new List<InventoryOverview>()
            //{
            //    new InventoryOverview() { },
            //    new InventoryOverview() { },
            //    new InventoryOverview() { },
            //    new InventoryOverview() { },
            //    new InventoryOverview() { },
            //}.AsEnumerable();

            //mockService
            //    .Setup(p => p.GetInventoryOverviews(It.IsNotNull<InventoryQueryParameter>()))
            //    .ReturnsAsync(searchResult);

            ////GetByName
            //InventoryDetail detailResult = new InventoryDetail()
            //{
            //    Cards = new List<MagicCard>(),
            //    InventoryCards = new List<InventoryCard>(),
            //    Name = "Mock Card Detail",
            //};

            //mockService
            //    .Setup(p => p.GetInventoryDetailByName(It.IsNotNull<string>()))
            //    .ReturnsAsync(detailResult);

            ////var mockMapper = new Mock<IMapperService>(MockBehavior.Strict);

            //var mockRefService = Carpentry.Data.Tests.MockClasses.MockDataServices.MockDataReferenceService();

            //var mapperService = new MapperService(mockRefService.Object);

            //_inventoryController = new Controllers.InventoryController(mockService.Object, mapperService);
        }

        [TestMethod]
        public void Inventory_GetStatus_ReturnsAsyncOK_Test()
        {
            //arrange
            var mockService = new Mock<IInventoryControllerService>(MockBehavior.Strict);

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

            var mockService = new Mock<IInventoryControllerService>(MockBehavior.Strict);

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
            var mockService = new Mock<IInventoryControllerService>(MockBehavior.Strict);

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
            var mockService = new Mock<IInventoryControllerService>(MockBehavior.Strict);

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

            var mockService = new Mock<IInventoryControllerService>(MockBehavior.Strict);

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
            var mockService = new Mock<IInventoryControllerService>(MockBehavior.Strict);

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
            var mockService = new Mock<IInventoryControllerService>(MockBehavior.Strict);

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
