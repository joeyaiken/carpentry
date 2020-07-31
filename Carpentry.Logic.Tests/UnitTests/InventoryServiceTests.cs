using Carpentry.Data.DataModels;
using Carpentry.Data.Interfaces;
using Carpentry.Data.QueryParameters;
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
        public async Task TestMethodsVerifiedUpToDate()
        {
            Assert.Fail();
        }


        [TestMethod]
        public async Task InventoryServiceTests_AddInventoryCard_Test()
        {
            //Assemble
            int idToExpect = 1;

            InventoryCardDto newCard = new InventoryCardDto()
            {
                MultiverseId = 1,
                VariantName = "normal"
            };

            var mockInventoryRepo = new Mock<IInventoryDataRepo>(MockBehavior.Strict);

            mockInventoryRepo
                .Setup(p => p.AddInventoryCard(It.IsNotNull<InventoryCardData>()))
                .ReturnsAsync(idToExpect);

            var mockDataUpdateService = new Mock<IDataUpdateService>(MockBehavior.Strict);

            mockDataUpdateService
                .Setup(p => p.EnsureCardDefinitionExists(It.Is<int>(i => i == newCard.MultiverseId)))
                .Returns(Task.CompletedTask);

            var mockQueryService = new Mock<IDataQueryService>(MockBehavior.Strict);

            var mockDataReferenceService = new Mock<IDataReferenceService>(MockBehavior.Strict);

            DataReferenceValue<int> expectedVariantType = new DataReferenceValue<int>() {Id=1, Name="normal" };

            mockDataReferenceService
                .Setup(p => p.GetCardVariantTypeByName(It.IsNotNull<string>()))
                .ReturnsAsync(expectedVariantType);

            var mockCardDatarepo = new Mock<ICardDataRepo>(MockBehavior.Strict);

            var inventoryService = new InventoryService(
                mockInventoryRepo.Object,
                mockDataUpdateService.Object,
                mockQueryService.Object,
                mockDataReferenceService.Object,
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
                new InventoryCardDto() { MultiverseId = 1, VariantName = "normal" },
                new InventoryCardDto() { MultiverseId = 1, VariantName = "normal" },
                new InventoryCardDto() { MultiverseId = 1, VariantName = "normal" },
                new InventoryCardDto() { MultiverseId = 1, VariantName = "normal" },
            };

            var mockInventoryRepo = new Mock<IInventoryDataRepo>(MockBehavior.Strict);

            mockInventoryRepo
                .Setup(p => p.AddInventoryCardBatch(It.IsNotNull<List<InventoryCardData>>()))
                .Returns(Task.CompletedTask);

            var mockDataUpdateService = new Mock<IDataUpdateService>(MockBehavior.Strict);

            mockDataUpdateService
                .Setup(p => p.EnsureCardDefinitionExists(It.Is<int>(i => i > 0)))
                .Returns(Task.CompletedTask);

            var mockQueryService = new Mock<IDataQueryService>(MockBehavior.Strict);

            var mockDataReferenceService = new Mock<IDataReferenceService>(MockBehavior.Strict);

            List<DataReferenceValue<int>> variantTypes = new List<DataReferenceValue<int>>()
            {
                new DataReferenceValue<int>() { Id = 1, Name = "normal" },
                new DataReferenceValue<int>() { Id = 2, Name = "showcase" }
            };

            mockDataReferenceService
                .Setup(p => p.GetAllCardVariantTypes())
                .ReturnsAsync(variantTypes);

            var mockCardDatarepo = new Mock<ICardDataRepo>(MockBehavior.Strict);

            var inventoryService = new InventoryService(
                mockInventoryRepo.Object,
                mockDataUpdateService.Object,
                mockQueryService.Object,
                mockDataReferenceService.Object,
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
                MultiverseId = 1,
                VariantName = "normal"
            };

            var mockInventoryRepo = new Mock<IInventoryDataRepo>(MockBehavior.Strict);

            mockInventoryRepo
                .Setup(p => p.UpdateInventoryCard(It.IsNotNull<InventoryCardData>()))
                .Returns(Task.CompletedTask);

            InventoryCardData inventoryDbCard = new InventoryCardData() { Id = 1 };

            mockInventoryRepo
                .Setup(p => p.GetInventoryCardById(It.Is<int>(i => i == cardToUpdate.Id)))
                .ReturnsAsync(inventoryDbCard);

            var mockDataUpdateService = new Mock<IDataUpdateService>(MockBehavior.Strict);

            var mockQueryService = new Mock<IDataQueryService>(MockBehavior.Strict);

            var mockDataReferenceService = new Mock<IDataReferenceService>(MockBehavior.Strict);

            var mockCardDatarepo = new Mock<ICardDataRepo>(MockBehavior.Strict);

            var inventoryService = new InventoryService(
                mockInventoryRepo.Object,
                mockDataUpdateService.Object,
                mockQueryService.Object,
                mockDataReferenceService.Object,
                mockCardDatarepo.Object);

            //Act
            await inventoryService.UpdateInventoryCard(cardToUpdate);

            //Assert
            //Task returns void, nothing to assert
        }
        
        //Task DeleteInventoryCard(int id);
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

            var mockQueryService = new Mock<IDataQueryService>(MockBehavior.Strict);

            var mockDataReferenceService = new Mock<IDataReferenceService>(MockBehavior.Strict);

            var mockCardDatarepo = new Mock<ICardDataRepo>(MockBehavior.Strict);

            var inventoryService = new InventoryService(
                mockInventoryRepo.Object,
                mockDataUpdateService.Object,
                mockQueryService.Object,
                mockDataReferenceService.Object,
                mockCardDatarepo.Object);

            //Act
            await inventoryService.DeleteInventoryCard(idToDelete);

            //Assert
            //Task returns void, nothing to assert
        }

        //Task<IEnumerable<InventoryOverview>> GetInventoryOverviews(InventoryQueryParameter param);
        [TestMethod]
        public async Task InventoryServiceTests_GetInventoryOverviews_Test()
        {
            //Assemble
            InventoryQueryParameter queryParamToRequest = new InventoryQueryParameter()
            {
                //GroupBy = "unique",
                //Sort = "price",
                //SortDescending = true,
                //Skip = 0,
                //Take = 100,
                //Rarity = new List<string>() { "common", "uncommon" },
            };
            var mockInventoryRepo = new Mock<IInventoryDataRepo>(MockBehavior.Strict);

            var mockDataUpdateService = new Mock<IDataUpdateService>(MockBehavior.Strict);

            var mockQueryService = new Mock<IDataQueryService>(MockBehavior.Strict);

            List<CardOverviewResult> inventoryOverviewResult = new List<CardOverviewResult>()
            {
                
            };

            mockQueryService
                .Setup(p => p.GetInventoryOverviews(It.IsNotNull<InventoryQueryParameter>()))
                .ReturnsAsync(inventoryOverviewResult);

            var mockDataReferenceService = new Mock<IDataReferenceService>(MockBehavior.Strict);

            var mockCardDatarepo = new Mock<ICardDataRepo>(MockBehavior.Strict);

            var inventoryService = new InventoryService(
                mockInventoryRepo.Object,
                mockDataUpdateService.Object,
                mockQueryService.Object,
                mockDataReferenceService.Object,
                mockCardDatarepo.Object);

            //Act
            IEnumerable<InventoryOverviewDto> overivews = await inventoryService.GetInventoryOverviews(queryParamToRequest);

            //Assert
            Assert.IsNotNull(overivews);
        }
        
        //Task<InventoryDetail> GetInventoryDetailByName(string name);
        [TestMethod]
        public async Task InventoryServiceTests_GetInventoryDetail_Test()
        {
            //Assemble
            int idToRequest = 1;

            string nameToExpect = "Opt";

            var mockInventoryRepo = new Mock<IInventoryDataRepo>(MockBehavior.Strict);

            var mockDataUpdateService = new Mock<IDataUpdateService>(MockBehavior.Strict);

            var mockQueryService = new Mock<IDataQueryService>(MockBehavior.Strict);

            List<InventoryCardResult> inventoryQueryResult = new List<InventoryCardResult>()
            {
                new InventoryCardResult(),
                new InventoryCardResult(),
                new InventoryCardResult(),
                new InventoryCardResult(),
                //new InventoryCardResult { DeckCards = new List<DeckCardResult>(), },
                //new InventoryCardResult { DeckCards = new List<DeckCardResult>(), },
                //new InventoryCardResult { DeckCards = new List<DeckCardResult>(), },
                //new InventoryCardResult { DeckCards = new List<DeckCardResult>(), },
            };

            mockQueryService
                .Setup(p => p.GetInventoryCardsByName(It.Is<string>(s => s == nameToExpect)))
                .ReturnsAsync(inventoryQueryResult);

            var mockDataReferenceService = new Mock<IDataReferenceService>(MockBehavior.Strict);

            var mockCardDatarepo = new Mock<ICardDataRepo>(MockBehavior.Strict);

            CardData getCardDataResult = new CardData()
            {
                Name = nameToExpect
            };

            mockCardDatarepo
                .Setup(p => p.GetCardData(It.Is<int>(i => i == idToRequest)))
                .ReturnsAsync(getCardDataResult);
                
            List<CardData> cardDataResult = new List<CardData>()
            {
                new CardData()
                {
                    Legalities = new List<CardLegalityData>(),
                    Variants = new List<CardVariantData>(),
                    Rarity = new CardRarityData(),
                    Set = new CardSetData(),
                    CardColorIdentities = new List<CardColorIdentityData>(),
                    CardColors = new List<CardColorData>(),
                },
                new CardData()
                {
                    Legalities = new List<CardLegalityData>(),
                    Variants = new List<CardVariantData>(),
                    Rarity = new CardRarityData(),
                    Set = new CardSetData(),
                    CardColorIdentities = new List<CardColorIdentityData>(),
                    CardColors = new List<CardColorData>(),
                },
                new CardData()
                {
                    Legalities = new List<CardLegalityData>(),
                    Variants = new List<CardVariantData>(),
                    Rarity = new CardRarityData(),
                    Set = new CardSetData(),
                    CardColorIdentities = new List<CardColorIdentityData>(),
                    CardColors = new List<CardColorData>(),
                },
            };

            mockCardDatarepo
                .Setup(p => p.GetCardsByName(It.Is<string>(s => s == nameToExpect)))
                .ReturnsAsync(cardDataResult);

            var inventoryService = new InventoryService(
                mockInventoryRepo.Object,
                mockDataUpdateService.Object,
                mockQueryService.Object,
                mockDataReferenceService.Object,
                mockCardDatarepo.Object);

            //Act
            InventoryDetailDto result = await inventoryService.GetInventoryDetail(idToRequest);

            //Assert
            Assert.IsNotNull(result);
        }
    }
}
