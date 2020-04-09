using Carpentry.Data.QueryParameters;
using Carpentry.Logic.Interfaces;
using Carpentry.Logic.Models;
using Carpentry.UI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Carpentry.UI.Tests.UnitTests
{
    [TestClass]
    public class CardSearchControllerTests
    {
        private readonly Controllers.CardSearchController _cardSearchController;

        //readonly CarpentryFactory _factory;

        public CardSearchControllerTests()
        {
            //mock service
            var mockService = new Mock<ICardSearchService>();

            //search result payload
            IEnumerable<MagicCard> searchResults = new List<MagicCard>()
            {
                new MagicCard{ },
                new MagicCard{ },
                new MagicCard{ },
                new MagicCard{ },
                new MagicCard{ },
            }.AsEnumerable();

            //SearchWeb
            mockService
                .Setup(p => p.SearchCardsFromWeb(It.IsNotNull<NameSearchQueryParameter>()))
                .Returns(Task.FromResult(searchResults));

            //SearchSet
            mockService
                .Setup(p => p.SearchCardsFromSet(It.IsNotNull<CardSearchQueryParameter>()))
                .Returns(Task.FromResult(searchResults));

            //SearchInventory
            mockService
                .Setup(p => p.SearchCardsFromInventory(It.IsNotNull<InventoryQueryParameter>()))
                .Returns(Task.FromResult(searchResults));

            _cardSearchController = new Controllers.CardSearchController(mockService.Object);
        }


        [TestMethod]
        public async Task CardSearch_SearchWeb_ReturnsOK_Test()
        {
            //assemble
            NameSearchQueryParameter queryParam = new NameSearchQueryParameter()
            {

            };

            //act
            var response = await _cardSearchController.SearchWeb(queryParam);

            //assert
            Assert.IsInstanceOfType(response.Result, typeof(OkObjectResult));

            var typedResult = response.Result as OkObjectResult;

            IEnumerable<MagicCardDto> resultValue = typedResult.Value as IEnumerable<MagicCardDto>;

            Assert.IsNotNull(resultValue);
            Assert.AreEqual(5, resultValue.Count());
        }

        [TestMethod]
        public async Task CardSearch_SearchSet_ReturnsOK_Test()
        {
            //assemble
            CardSearchQueryParameter queryParam = new CardSearchQueryParameter()
            {

            };

            //act
            var response = await _cardSearchController.SearchSet(queryParam);

            //assert
            Assert.IsInstanceOfType(response.Result, typeof(OkObjectResult));

            var typedResult = response.Result as OkObjectResult;

            IEnumerable<MagicCardDto> resultValue = typedResult.Value as IEnumerable<MagicCardDto>;

            Assert.IsNotNull(resultValue);
            Assert.AreEqual(5, resultValue.Count());
        }

        [TestMethod]
        public async Task CardSearch_SearchInventory_ReturnsOK_Test()
        {
            //assemble
            InventoryQueryParameter queryParam = new InventoryQueryParameter()
            {

            };

            //act
            var response = await _cardSearchController.SearchInventory(queryParam);

            //assert
            Assert.IsInstanceOfType(response.Result, typeof(OkObjectResult));

            var typedResult = response.Result as OkObjectResult;

            IEnumerable<MagicCardDto> resultValue = typedResult.Value as IEnumerable<MagicCardDto>;

            Assert.IsNotNull(resultValue);
            Assert.AreEqual(5, resultValue.Count());
        }

    }
}
