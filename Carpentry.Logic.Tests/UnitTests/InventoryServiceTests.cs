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

namespace Carpentry.Logic.Tests.UnitTests
{
    [TestClass]
    public class InventoryServiceTests
    {
        //Task<int> AddInventoryCard(InventoryCard dto);
        [TestMethod]
        public async Task InventoryServiceTests_AddInventoryCard_Test()
        {
            //Assemble
            int idToExpect = 1;

            var mockInventoryRepo = new Mock<IInventoryDataRepo>(MockBehavior.Strict);
            
            var mockDataUpdateService = new Mock<IDataUpdateService>(MockBehavior.Strict);

            mockDataUpdateService
                .Setup(p => p.EnsureCardDefinitionExists(It.Is<int>(i => i == idToExpect)))
                .Returns(Task.CompletedTask);

            var mockQueryService = new Mock<IDataQueryService>(MockBehavior.Strict);
            var mockDataReferenceService = new Mock<IDataReferenceService>(MockBehavior.Strict);
            var mockCardDatarepo = new Mock<ICardDataRepo>(MockBehavior.Strict);

            var inventoryService = new InventoryService(
                mockInventoryRepo.Object,
                mockDataUpdateService.Object,
                mockQueryService.Object,
                mockDataReferenceService.Object,
                mockCardDatarepo.Object);

            InventoryCardDto newCard = new InventoryCardDto()
            {
                MultiverseId = 1,
            };

            //Act
            var newId = await inventoryService.AddInventoryCard(newCard);

            //Assert
            Assert.AreEqual(idToExpect, newId);
        }
        //Task AddInventoryCardBatch(IEnumerable<InventoryCard> cards);
        [TestMethod]
        public void InventoryServiceTests_AddInventoryCardBatch_Test()
        {
            //Assemble


            //Act


            //Assert
            Assert.Fail();
        }
        //Task UpdateInventoryCard(InventoryCard dto);
        [TestMethod]
        public void InventoryServiceTests_UpdateInventoryCard_Test()
        {
            //Assemble


            //Act


            //Assert
            Assert.Fail();
        }
        //Task DeleteInventoryCard(int id);
        [TestMethod]
        public void InventoryServiceTests_DeleteInventoryCard_Test()
        {
            //Assemble


            //Act


            //Assert
            Assert.Fail();
        }
        //Task<IEnumerable<InventoryOverview>> GetInventoryOverviews(InventoryQueryParameter param);
        [TestMethod]
        public void InventoryServiceTests_GetInventoryOverviews_Test()
        {
            //Assemble


            //Act


            //Assert
            Assert.Fail();
        }
        //Task<InventoryDetail> GetInventoryDetailByName(string name);
        [TestMethod]
        public void InventoryServiceTests_GetInventoryDetailByName_Test()
        {
            //Assemble


            //Act


            //Assert
            Assert.Fail();
        }
    }
}
