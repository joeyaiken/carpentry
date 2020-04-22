using Carpentry.Data.Interfaces;
using Carpentry.Data.Models;
using Carpentry.Data.QueryParameters;
using Carpentry.Logic.Implementations;
using Carpentry.Logic.Interfaces;
using Carpentry.Logic.Models;
using Carpentry.Logic.Models.Scryfall;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carpentry.Logic.Tests.UnitTests
{
    [TestClass]
    public class CardSearchServiceTests
    {
        private static CardDataDto CardInstance()
        {
            CardDataDto result = new CardDataDto()
            {
                Name = "Card",
                Variants = new List<CardVariantDto>()
                {
                    new CardVariantDto()
                    {
                        Name = "normal",
                    }
                }
            };
            return result;
        }

        [TestMethod]
        public async Task CardSearchService_SearchCardsFromInventory_Test()
        {
            //Assemble
            List<CardDataDto> expectedSearchResult = new List<CardDataDto>()
            {
                CardInstance(),
                CardInstance(),
                CardInstance(),
                CardInstance(),
                CardInstance(),
            };

            var mockQueryService = new Mock<IDataQueryService>(MockBehavior.Strict);

            mockQueryService
                .Setup(p => p.SearchInventoryCards(It.IsNotNull<InventoryQueryParameter>()))
                .ReturnsAsync(expectedSearchResult);

            var mockScryService = new Mock<IScryfallService>(MockBehavior.Strict);

            var cardSearchService = new CardSearchService(mockQueryService.Object, mockScryService.Object);

            InventoryQueryParameter filters = new InventoryQueryParameter()
            {

            };

            //Act
            IEnumerable<MagicCardDto> result = await cardSearchService.SearchCardsFromInventory(filters);

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedSearchResult.Count, result.Count());

        }

        [TestMethod]
        public async Task CardSearchService_SearchCardsFromSet_Test()
        {
            //Assemble
            List<CardDataDto> expectedSearchResult = new List<CardDataDto>()
            {
                CardInstance(),
                CardInstance(),
                CardInstance(),
                CardInstance(),
                CardInstance(),
            };

            var mockQueryService = new Mock<IDataQueryService>(MockBehavior.Strict);

            mockQueryService
                .Setup(p => p.SearchCardSet(It.IsNotNull<CardSearchQueryParameter>()))
                .ReturnsAsync(expectedSearchResult);

            var mockScryService = new Mock<IScryfallService>(MockBehavior.Strict);

            var cardSearchService = new CardSearchService(mockQueryService.Object, mockScryService.Object);

            CardSearchQueryParameter filters = new CardSearchQueryParameter()
            {

            };

            //Act
            IEnumerable<MagicCardDto> result = await cardSearchService.SearchCardsFromSet(filters);

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedSearchResult.Count, result.Count());

        }

        [TestMethod]
        public async Task CardSearchService_SearchCardsFromWeb_Test()
        {
            //Assemble
            List<ScryfallMagicCard> expectedSearchResult = new List<ScryfallMagicCard>()
            {
                new ScryfallMagicCard(){ Name = "Card" },
                new ScryfallMagicCard(){ Name = "Card" },
                new ScryfallMagicCard(){ Name = "Card" },
                new ScryfallMagicCard(){ Name = "Card" },
                new ScryfallMagicCard(){ Name = "Card" },
            };

            var mockQueryService = new Mock<IDataQueryService>(MockBehavior.Strict);

            var mockScryService = new Mock<IScryfallService>(MockBehavior.Strict);

            mockScryService
                .Setup(p => p.SearchScryfallByName(It.IsNotNull<string>(), It.IsAny<bool>()))
                .ReturnsAsync(expectedSearchResult);

            var cardSearchService = new CardSearchService(mockQueryService.Object, mockScryService.Object);

            NameSearchQueryParameter filters = new NameSearchQueryParameter()
            {
                Name = "",
                Exclusive = false,
            };

            //Act
            IEnumerable<MagicCardDto> result = await cardSearchService.SearchCardsFromWeb(filters);

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedSearchResult.Count, result.Count());
        }
    }
}
