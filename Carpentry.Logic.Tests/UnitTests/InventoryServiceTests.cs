using Carpentry.Data.DataModels;
using Carpentry.Data.Interfaces;
using Carpentry.Data.QueryResults;
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

        [TestMethod]
        public async Task InventoryServiceTests_AddInventoryCard_Test()
        {
            //Assemble
            int idToExpect = 1;

            InventoryCardDto newCard = new InventoryCardDto()
            {
                //MultiverseId = 1,
                //VariantName = "normal"
            };

            var mockInventoryRepo = new Mock<IInventoryDataRepo>(MockBehavior.Strict);
            var mockCardDatarepo = new Mock<ICardDataRepo>(MockBehavior.Strict);
            //var mockSearchService

            mockInventoryRepo
                .Setup(p => p.AddInventoryCard(It.IsNotNull<InventoryCardData>()))
                .ReturnsAsync(idToExpect);

            var cardDataResponse = new CardData();

            mockCardDatarepo
                .Setup(p => p.GetCardData(It.IsAny<string>(), It.IsAny<int>())) //set/collectionNumber
                .ReturnsAsync(cardDataResponse);

            //var mockDataUpdateService = new Mock<IDataUpdateService>(MockBehavior.Strict);

            //mockDataUpdateService
            //    .Setup(p => p.EnsureCardDefinitionExists(It.Is<int>(i => i == newCard.MultiverseId)))
            //    .Returns(Task.CompletedTask);

            //var mockCoreRepo = new Mock<ICoreDataRepo>(MockBehavior.Strict);

            //DataReferenceValue<int> expectedVariantType = new DataReferenceValue<int>() { Id = 1, Name = "normal" };

            //mockCoreRepo
            //    .Setup(p => p.GetCardVariantTypeByName(It.IsNotNull<string>()))
            //    .ReturnsAsync(expectedVariantType);

            

            var inventoryService = new InventoryService(
                mockInventoryRepo.Object,
                //mockDataUpdateService.Object,
                //mockCoreRepo.Object,
                mockCardDatarepo.Object);

            //Act
            var newId = await inventoryService.AddInventoryCard(newCard);

            //Assert
            Assert.AreEqual(idToExpect, newId);
        }

        [TestMethod]
        public async Task InventoryServiceTests_AddInventoryCardBatch_Test()
        {
            //Assemble
            List<InventoryCardDto> cardBatch = new List<InventoryCardDto>()
            {
                //new InventoryCardDto() { MultiverseId = 1, VariantName = "normal" },
                //new InventoryCardDto() { MultiverseId = 1, VariantName = "normal" },
                //new InventoryCardDto() { MultiverseId = 1, VariantName = "normal" },
                //new InventoryCardDto() { MultiverseId = 1, VariantName = "normal" },
            };

            var mockInventoryRepo = new Mock<IInventoryDataRepo>(MockBehavior.Strict);

            mockInventoryRepo
                .Setup(p => p.AddInventoryCardBatch(It.IsNotNull<List<InventoryCardData>>()))
                .Returns(Task.CompletedTask);

            //var mockDataUpdateService = new Mock<IDataUpdateService>(MockBehavior.Strict);

            //mockDataUpdateService
            //    .Setup(p => p.EnsureCardDefinitionExists(It.Is<int>(i => i > 0)))
            //    .Returns(Task.CompletedTask);

            //var mockCoreRepo = new Mock<ICoreDataRepo>(MockBehavior.Strict);

            //List<DataReferenceValue<int>> variantTypes = new List<DataReferenceValue<int>>()
            //{
            //    new DataReferenceValue<int>() { Id = 1, Name = "normal" },
            //    new DataReferenceValue<int>() { Id = 2, Name = "showcase" }
            //};

            //mockCoreRepo
            //    .Setup(p => p.GetAllCardVariantTypes())
            //    .ReturnsAsync(variantTypes);

            var mockCardDatarepo = new Mock<ICardDataRepo>(MockBehavior.Strict);

            var inventoryService = new InventoryService(
                mockInventoryRepo.Object,
                //mockDataUpdateService.Object,
                //mockCoreRepo.Object,
                mockCardDatarepo.Object);

            //Act
            await inventoryService.AddInventoryCardBatch(cardBatch);

            //Assert
            //Task returns void, nothing to assert
        }

        [TestMethod]
        public async Task InventoryServiceTests_UpdateInventoryCard_Test()
        {
            //Assemble
            InventoryCardDto cardToUpdate = new InventoryCardDto()
            {
                Id = 1,
                //MultiverseId = 1,
                //VariantName = "normal"
            };

            var mockInventoryRepo = new Mock<IInventoryDataRepo>(MockBehavior.Strict);

            mockInventoryRepo
                .Setup(p => p.UpdateInventoryCard(It.IsNotNull<InventoryCardData>()))
                .Returns(Task.CompletedTask);

            InventoryCardData inventoryDbCard = new InventoryCardData() { Id = 1 };

            mockInventoryRepo
                .Setup(p => p.GetInventoryCard(It.Is<int>(i => i == cardToUpdate.Id)))
                .ReturnsAsync(inventoryDbCard);

            var mockDataUpdateService = new Mock<IDataUpdateService>(MockBehavior.Strict);

            var mockCoreRepo = new Mock<ICoreDataRepo>(MockBehavior.Strict);

            var mockCardDatarepo = new Mock<ICardDataRepo>(MockBehavior.Strict);

            var inventoryService = new InventoryService(
                mockInventoryRepo.Object,
                //mockDataUpdateService.Object,
                //mockCoreRepo.Object,
                mockCardDatarepo.Object);

            //Act
            await inventoryService.UpdateInventoryCard(cardToUpdate);

            //Assert
            //Task returns void, nothing to assert
        }

        [TestMethod]
        public async Task InventoryServiceTests_UpdateInventoryCardBatch_Test()
        {
            //Assemble
            var cardsToUpdate = new List<InventoryCardDto>()
            {
                //new InventoryCardDto()
                //{
                //    Id = 1,
                //    MultiverseId = 1,
                //    VariantName = "normal"
                //},
                //new InventoryCardDto()
                //{
                //    Id = 2,
                //    MultiverseId = 2,
                //    VariantName = "normal"
                //},
                //new InventoryCardDto()
                //{
                //    Id = 3,
                //    MultiverseId = 3,
                //    VariantName = "normal"
                //},
            };

            var mockInventoryRepo = new Mock<IInventoryDataRepo>(MockBehavior.Strict);

            mockInventoryRepo
                .Setup(p => p.UpdateInventoryCard(It.IsNotNull<InventoryCardData>()))
                .Returns(Task.CompletedTask);

            InventoryCardData inventoryDbCard = new InventoryCardData() { Id = 1 };

            mockInventoryRepo
                .Setup(p => p.GetInventoryCard(It.IsAny<int>()))
                .ReturnsAsync(inventoryDbCard);

            var mockDataUpdateService = new Mock<IDataUpdateService>(MockBehavior.Strict);

            var mockCoreRepo = new Mock<ICoreDataRepo>(MockBehavior.Strict);

            var mockCardDatarepo = new Mock<ICardDataRepo>(MockBehavior.Strict);

            var inventoryService = new InventoryService(
                mockInventoryRepo.Object,
                //mockDataUpdateService.Object,
                //mockCoreRepo.Object,
                mockCardDatarepo.Object);

            //Act
            await inventoryService.UpdateInventoryCardBatch(cardsToUpdate);

            //Assert
            //Task returns void, nothing to assert
        }

        [TestMethod]
        public async Task InventoryServiceTests_DeleteInventoryCard_Test()
        {
            //Assemble
            int idToDelete = 1;

            var mockInventoryRepo = new Mock<IInventoryDataRepo>(MockBehavior.Strict);

            mockInventoryRepo
                .Setup(p => p.DeleteInventoryCard(It.Is<int>(i => i == idToDelete)))
                .Returns(Task.CompletedTask);

            var mockDataUpdateService = new Mock<IDataUpdateService>(MockBehavior.Strict);

            var mockCoreRepo = new Mock<ICoreDataRepo>(MockBehavior.Strict);

            var mockCardDatarepo = new Mock<ICardDataRepo>(MockBehavior.Strict);

            var inventoryService = new InventoryService(
                mockInventoryRepo.Object,
                //mockDataUpdateService.Object,
                //mockCoreRepo.Object,
                mockCardDatarepo.Object);

            //Act
            await inventoryService.DeleteInventoryCard(idToDelete);

            //Assert
            //Task returns void, nothing to assert
        }

        [TestMethod]
        public async Task InventoryServiceTests_DeleteInventoryCardBatch_Test()
        {
            //Assemble
            var idsToDelete = new List<int> { 1, 2, 3 };

            var mockInventoryRepo = new Mock<IInventoryDataRepo>(MockBehavior.Strict);

            mockInventoryRepo
                .Setup(p => p.DeleteInventoryCard(It.IsAny<int>()))
                .Returns(Task.CompletedTask);

            var mockDataUpdateService = new Mock<IDataUpdateService>(MockBehavior.Strict);

            var mockCoreRepo = new Mock<ICoreDataRepo>(MockBehavior.Strict);

            var mockCardDatarepo = new Mock<ICardDataRepo>(MockBehavior.Strict);

            var inventoryService = new InventoryService(
                mockInventoryRepo.Object,
                //mockDataUpdateService.Object,
                //mockCoreRepo.Object,
                mockCardDatarepo.Object);

            //Act
            await inventoryService.DeleteInventoryCardBatch(idsToDelete);

            //Assert
            //Task returns void, nothing to assert
        }


    }
}
