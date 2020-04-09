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
using Carpentry.UI.Models;
using Microsoft.AspNetCore.Mvc;

namespace Carpentry.UI.Tests.UnitTests
{
    [TestClass]
    public class InventoryControllerTests
    {
        //readonly CarpentryFactory _factory;
        //readonly HttpClient _client;
        private readonly Controllers.InventoryController _inventoryController;

        public InventoryControllerTests()
        {
            //_factory = new CarpentryFactory();
            //_client = _factory.CreateClient();

            var mockService = new Mock<IInventoryService>();

            //Add
            mockService
                .Setup(p => p.AddInventoryCard(It.IsNotNull<InventoryCard>()))
                .Returns(Task.FromResult(1));

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
                .Setup(p => p.DeleteInventoryCard(It.Is<int>(i => i < 0)))
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
                .Returns(Task.FromResult(searchResult));

            //GetByName
            InventoryDetail detailResult = new InventoryDetail()
            {
                Cards = new List<MagicCard>(),
                InventoryCards = new List<InventoryCard>(),
                Name = "Mock Card Detail",
            };

            mockService
                .Setup(p => p.GetInventoryDetailByName(It.IsNotNull<string>()))
                .Returns(Task.FromResult(detailResult));

            _inventoryController = new Controllers.InventoryController(mockService.Object);
        }

        #region Tests - Controller methods all return Ok/Accepted

        [TestMethod]
        public async Task Inventory_Add_ReturnsOK_Test()
        {
            //assemble
            InventoryCardDto payload = new InventoryCardDto()
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
        public async Task Inventory_AddBatch_ReturnsOK_Test()
        {
            //assemble
            List<InventoryCardDto> payload = new List<InventoryCardDto>()
            {
                new InventoryCardDto(){ },
                new InventoryCardDto(){ },
                new InventoryCardDto(){ },
                new InventoryCardDto(){ },
                new InventoryCardDto(){ },
            };

            //act
            var response = await _inventoryController.AddBatch(payload);

            //assert
            Assert.IsInstanceOfType(response.Result, typeof(OkResult));

        }

        [TestMethod]
        public async Task Inventory_Update_ReturnsOK_Test()
        {
            //assemble
            InventoryCardDto payload = new InventoryCardDto()
            {

            };

            //act
            var response = await _inventoryController.Update(payload);

            //assert
            Assert.IsInstanceOfType(response, typeof(OkResult));

        }
        
        [TestMethod]
        public async Task Inventory_Delete_ReturnsOK_Test()
        {
            //assemble
            int idToSubmit = 3;

            //act
            var response = await _inventoryController.Delete(idToSubmit);

            //assert
            Assert.IsInstanceOfType(response, typeof(OkResult));
        }
        
        [TestMethod]
        public async Task Inventory_Search_ReturnsOK_Test()
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

            IEnumerable<InventoryOverviewDto> resultValue = typedResult.Value as IEnumerable<InventoryOverviewDto>;

            Assert.IsNotNull(resultValue);
            Assert.AreEqual(5, resultValue.Count());

        }

        [TestMethod]
        public async Task Inventory_GetByName_ReturnsOK_Test()
        {
            //assemble
            string nameToRequest = "Mock Card Detail";

            //act
            var response = await _inventoryController.GetByName(nameToRequest);

            //assert
            var typedResult = response.Result as OkObjectResult;

            InventoryDetailDto resultValue = typedResult.Value as InventoryDetailDto;
            Assert.IsNotNull(resultValue);
            Assert.AreEqual(nameToRequest, resultValue.Name);
            
        }

        #endregion
    }
}
