using Carpentry.Data.Interfaces;
using Carpentry.Data.QueryParameters;
using Carpentry.Service.Implementations;
using Carpentry.Service.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carpentry.Service.Tests.UnitTests
{
    [TestClass]
    public class CardSearchControllerServiceTests
    {
        private readonly CardSearchControllerService _cardSearchService;
        public CardSearchControllerServiceTests()
        {
            //ICardRepo cardRepo
            var mockCardRepo = new Mock<ILegacyCardRepo>(MockBehavior.Strict);

            IQueryable<Data.LegacyDataContext.Card> inventoryQueryResult = new List<Data.LegacyDataContext.Card>()
            {
                new Data.LegacyDataContext.Card()
                {
                    Name = "Card1",
                },
                new Data.LegacyDataContext.Card()
                {
                    Name = "Card2",
                },
                new Data.LegacyDataContext.Card()
                {
                    Name = "Card3",
                },
                new Data.LegacyDataContext.Card()
                {
                    Name = "Card4",
                },
                new Data.LegacyDataContext.Card()
                {
                    Name = "Card5",
                },
                new Data.LegacyDataContext.Card()
                {
                    Name = "Card5",
                },

            }.AsQueryable();

            //mockCardRepo
            //    .Setup(p => p.QueryFilteredCards(It.IsNotNull<InventoryQueryParameter>()))
            //    .ReturnsAsync(inventoryQueryResult);
            //TODO re-add this
            //_cardSearchService = new CardSearchService(mockCardRepo.Object);
        }

        [TestMethod]
        public async Task CardSearchControllerService_SearchCardsFromInventory_Test()
        {
            ////Assemble
            //InventoryQueryParameter filters = new InventoryQueryParameter()
            //{

            //};

            ////Act
            //IEnumerable<MagicCard> result = await _cardSearchService.SearchCardsFromInventory(filters);

            ////Assert
            //Assert.IsNotNull(result);
            //Assert.AreEqual(5, result.Count());
            Assert.Fail();

        }

        [TestMethod]
        public async Task CardSearchControllerService_SearchCardsFromSet_Test()
        {
            //<IEnumerable<MagicCard>>
            //CardSearchQueryParameter filters
            await Task.CompletedTask;
            Assert.Fail();

            //IQueryable<ScryfallMagicCard> query = await _scryRepo.QueryCardsBySet(filters.SetCode);

            //if (!string.IsNullOrEmpty(filters.Type))
            //{
            //    query = query.Where(x => x.Type.Contains(filters.Type));
            //}

            //filters.ColorIdentity.ForEach(color =>
            //{
            //    query = query.Where(x => x.ColorIdentity.Contains(color));
            //});

            //if (filters.ExclusiveColorFilters)
            //{
            //    query = query.Where(x => x.ColorIdentity.Count() == filters.ColorIdentity.Count());
            //}

            //if (filters.MultiColorOnly)
            //{
            //    query = query.Where(x => x.ColorIdentity.Count() > 1);
            //}

            //query = query.Where(x => filters.Rarity.Contains(x.Rarity.ToLower()));

            //var result = query.OrderBy(x => x.Name).ToList();

            //return result;
        }

        [TestMethod]
        public async Task CardSearchControllerService_SearchCardsFromWeb_Test()
        {
            //<IEnumerable<MagicCard>>
            //NameSearchQueryParameter filters
            await Task.CompletedTask;
            Assert.Fail();

            //IQueryable<ScryfallMagicCard> query = await _scryRepo.QueryScryfallByName(filters.Name, filters.Exclusive);

            //List<ScryfallMagicCard> result = query.ToList();

            //return result;
        }


    }
}
