//using Carpentry.Data.Models;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
//using Microsoft.Data.Sqlite;
//using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Carpentry.Logic.Interfaces;
using Moq;
using Carpentry.Logic.Models;
using Carpentry.Data.QueryParameters;
using Carpentry.UI.Legacy.Models;
using Microsoft.AspNetCore.Mvc;
using Carpentry.UI.Legacy.Util;
using Carpentry.UI.Legacy.Controllers;

namespace Carpentry.UI.Tests.UnitTests
{
    [TestClass]
    public class InventoryControllerTests
    {
        //readonly CarpentryFactory _factory;
        //readonly HttpClient _client;
        private readonly InventoryController _inventoryController;

        public InventoryControllerTests()
        {
            //_factory = new CarpentryFactory();
            //_client = _factory.CreateClient();

            var mockService = new Mock<IInventoryService>(MockBehavior.Strict);

            //Add
            mockService
                .Setup(p => p.AddInventoryCard(It.IsNotNull<InventoryCard>()))
                .ReturnsAsync(1);

            //AddBatch
            mockService
                .Setup(p => p.AddInventoryCardBatch(It.IsNotNull<List<InventoryCard>>()))
                .Returns(Task.CompletedTask);

            //Update
            mockService
                .Setup(p => p.UpdateInventoryCard(It.IsNotNull<InventoryCard>()))
                .Returns(Task.CompletedTask);

            //Delete
            mockService
                .Setup(p => p.DeleteInventoryCard(It.Is<int>(i => i > 0)))
                .Returns(Task.CompletedTask);

            //Search
            IEnumerable<InventoryOverview> searchResult = new List<InventoryOverview>()
            {
                new InventoryOverview() { },
                new InventoryOverview() { },
                new InventoryOverview() { },
                new InventoryOverview() { },
                new InventoryOverview() { },
            }.AsEnumerable();

            mockService
                .Setup(p => p.GetInventoryOverviews(It.IsNotNull<InventoryQueryParameter>()))
                .ReturnsAsync(searchResult);

            //GetByName
            InventoryDetail detailResult = new InventoryDetail()
            {
                Cards = new List<MagicCard>(),
                InventoryCards = new List<InventoryCard>(),
                Name = "Mock Card Detail",
            };

            mockService
                .Setup(p => p.GetInventoryDetailByName(It.IsNotNull<string>()))
                .ReturnsAsync(detailResult);

            //var mockMapper = new Mock<IMapperService>(MockBehavior.Strict);

            //var mockRefService = Carpentry.Data.Tests.MockClasses.MockDataServices.MockDataReferenceService();

            //var mapperService = new MapperService(mockRefService.Object);

            //_inventoryController = new InventoryController(mockService.Object, mapperService);
        }

        #region Tests - Controller methods all return Ok/Accepted

        [TestMethod]
        public void Inventory_GetStatus_ReturnsAsyncOK_Test()
        {
            //assemble


            //act
            var response = _inventoryController.Get();

            //assert
            Assert.IsInstanceOfType(response, typeof(OkObjectResult));
            var typedResult = response as OkObjectResult;
            string resultValue = typedResult.Value as string;

            Assert.AreEqual("Online", resultValue);
        }

        [TestMethod]
        public async Task Inventory_Add_ReturnsAsyncOK_Test()
        {
            //assemble
            LegacyInventoryCardDto payload = new LegacyInventoryCardDto()
            {

            };

            //act
            var response = await _inventoryController.Add(payload);

            //assert
            Assert.IsInstanceOfType(response.Result, typeof(OkObjectResult));
            var typedResult = response.Result as OkObjectResult;
            Assert.AreEqual(1, typedResult.Value);
        }

        [TestMethod]
        public async Task Inventory_AddBatch_ReturnsAsyncOK_Test()
        {
            //assemble
            List<LegacyInventoryCardDto> payload = new List<LegacyInventoryCardDto>()
            {
                new LegacyInventoryCardDto(){ },
                new LegacyInventoryCardDto(){ },
                new LegacyInventoryCardDto(){ },
                new LegacyInventoryCardDto(){ },
                new LegacyInventoryCardDto(){ },
            };

            //act
            var response = await _inventoryController.AddBatch(payload);

            //assert
            Assert.IsInstanceOfType(response.Result, typeof(OkResult));

        }

        [TestMethod]
        public async Task Inventory_Update_ReturnsAsyncOK_Test()
        {
            //assemble
            LegacyInventoryCardDto payload = new LegacyInventoryCardDto()
            {

            };

            //act
            var response = await _inventoryController.Update(payload);

            //assert
            Assert.IsInstanceOfType(response, typeof(OkResult));

        }
        
        [TestMethod]
        public async Task Inventory_Delete_ReturnsAsyncOK_Test()
        {
            //assemble
            int idToSubmit = 1;

            //act
            var response = await _inventoryController.Delete(idToSubmit);

            //assert
            Assert.IsInstanceOfType(response, typeof(OkResult));
        }
        
        [TestMethod]
        public async Task Inventory_Search_ReturnsAsyncOK_Test()
        {
            //assemble
            InventoryQueryParameter param = new InventoryQueryParameter()
            {

            };

            //act
            var response = await _inventoryController.Search(param);

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
            //assemble
            string nameToRequest = "Mock Card Detail";

            //act
            var response = await _inventoryController.GetByName(nameToRequest);

            //assert
            var typedResult = response.Result as OkObjectResult;

            LegacyInventoryDetailDto resultValue = typedResult.Value as LegacyInventoryDetailDto;
            Assert.IsNotNull(resultValue);
            Assert.AreEqual(nameToRequest, resultValue.Name);
            
        }

        #endregion
    }
}
