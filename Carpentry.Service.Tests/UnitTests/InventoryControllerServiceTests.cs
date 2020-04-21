using Carpentry.Service.Implementations;
using Carpentry.Service.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Carpentry.Service.Tests.UnitTests
{
    [TestClass]
    public class InventoryControllerServiceTests
    {
        //Task<int> AddInventoryCard(InventoryCard dto);
        [TestMethod]
        public async Task InventoryControllerService_AddInventoryCard_Test()
        {
            //Assemble
            InventoryControllerService inventoryController = new InventoryControllerService();

            InventoryCardDto payload = new InventoryCardDto()
            {

            };

            //Act
            int result = await inventoryController.AddInventoryCard(payload);

            //Assert
            Assert.Fail();
        }

        //Task AddInventoryCardBatch(IEnumerable<InventoryCard> cards);
        [TestMethod]
        public async Task InventoryControllerService_AddInventoryCardBatch_Test()
        {
            ////Assemble
            //InventoryControllerService inventoryController = new InventoryControllerService();

            //List<InventoryCardDto> payload = new List<InventoryCardDto>()
            //{

            //};

            ////Act
            //int result = await inventoryController.AddInventoryCardBatch(payload);

            //Assert
            Assert.Fail();
        }

        //Task UpdateInventoryCard(InventoryCard dto);
        [TestMethod]
        public void InventoryControllerService_UpdateInventoryCard_Test()
        {
            Assert.Fail();
        }

        //Task DeleteInventoryCard(int id);
        [TestMethod]
        public void InventoryControllerService_DeleteInventoryCard_Test()
        {
            Assert.Fail();
        }

        //Task<IEnumerable<InventoryOverview>> GetInventoryOverviews(InventoryQueryParameter param);
        [TestMethod]
        public void InventoryControllerService_GetInventoryOverviews_Test()
        {
            Assert.Fail();
        }

        //Task<InventoryDetail> GetInventoryDetailByName(string name);
        [TestMethod]
        public void InventoryControllerService_GetInventoryDetailByName_Test()
        {
            Assert.Fail();
        }
    }
}
