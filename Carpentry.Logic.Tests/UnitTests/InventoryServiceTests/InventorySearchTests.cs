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

namespace Carpentry.Logic.Tests.UnitTests.InventoryServiceTests
{
    [TestClass]
    public class InventorySearchTests
    {
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

            var mockCoreRepo = new Mock<ICoreDataRepo>(MockBehavior.Strict);

            List<CardOverviewResult> inventoryOverviewResult = new List<CardOverviewResult>()
            {

            };

            mockInventoryRepo
                .Setup(p => p.GetInventoryOverviews(It.IsNotNull<InventoryQueryParameter>()))
                .ReturnsAsync(inventoryOverviewResult);

            var mockCardDatarepo = new Mock<ICardDataRepo>(MockBehavior.Strict);

            var inventoryService = new InventoryService(
                mockInventoryRepo.Object,
                mockDataUpdateService.Object,
                mockCoreRepo.Object,
                mockCardDatarepo.Object);

            //Act
            IEnumerable<InventoryOverviewDto> overivews = await inventoryService.GetInventoryOverviews(queryParamToRequest);

            //Assert
            Assert.IsNotNull(overivews);
        }

        [TestMethod]
        public async Task InventoryServiceTests_GetInventoryDetail_Test()
        {
            //Assemble
            int idToRequest = 1;

            string nameToExpect = "Opt";

            var mockInventoryRepo = new Mock<IInventoryDataRepo>(MockBehavior.Strict);

            var mockDataUpdateService = new Mock<IDataUpdateService>(MockBehavior.Strict);

            var mockCoreRepo = new Mock<ICoreDataRepo>(MockBehavior.Strict);

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

            mockInventoryRepo
                .Setup(p => p.GetInventoryCardsByName(It.Is<string>(s => s == nameToExpect)))
                .ReturnsAsync(inventoryQueryResult);

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
                mockCoreRepo.Object,
                mockCardDatarepo.Object);

            //Act
            InventoryDetailDto result = await inventoryService.GetInventoryDetail(idToRequest);

            //Assert
            Assert.IsNotNull(result);
        }
    }
}
