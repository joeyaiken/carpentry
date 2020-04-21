using Carpentry.Service.Models;
using Carpentry.Service.Interfaces;
using Carpentry.UI.Legacy.Controllers;
using Carpentry.UI.Legacy.Models;
using Carpentry.UI.Legacy.Util;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Carpentry.UI.Tests.Legacy.UnitTests
{
    [TestClass]
    public class CardSearchControllerTests
    {
        [TestMethod]
        public void CardSearch_GetStatus_ReturnsOK_Test()
        {
            //arrange
            var mockService = new Mock<ICardSearchControllerService>(MockBehavior.Strict);

            var mapperService = new MapperService();

            var cardSearchController = new CardSearchController(mockService.Object, mapperService);

            //act
            var response = cardSearchController.Get();

            //assert
            Assert.IsInstanceOfType(response, typeof(OkObjectResult));
            var typedResult = response as OkObjectResult;
            string resultValue = typedResult.Value as string;

            Assert.AreEqual("Online", resultValue);
        }

        [TestMethod]
        public async Task CardSearch_SearchWeb_ReturnsOK_Test()
        {
            //arrange
            IEnumerable<MagicCardDto> expectedSearchResults = new List<MagicCardDto>()
            {
                new MagicCardDto{ },
                new MagicCardDto{ },
                new MagicCardDto{ },
                new MagicCardDto{ },
                new MagicCardDto{ },
            }.AsEnumerable();

            var mockService = new Mock<ICardSearchControllerService>(MockBehavior.Strict);

            mockService
                .Setup(p => p.SearchCardsFromWeb(It.IsNotNull<NameSearchQueryParameter>()))
                .ReturnsAsync(expectedSearchResults);

            var mapperService = new MapperService();

            var cardSearchController = new CardSearchController(mockService.Object, mapperService);

            NameSearchQueryParameter queryParam = new NameSearchQueryParameter() { };

            //act
            var response = await cardSearchController.SearchWeb(queryParam);

            //assert
            Assert.IsInstanceOfType(response.Result, typeof(OkObjectResult));

            var typedResult = response.Result as OkObjectResult;

            IEnumerable<LegacyMagicCardDto> resultValue = typedResult.Value as IEnumerable<LegacyMagicCardDto>;

            Assert.IsNotNull(resultValue);
            Assert.AreEqual(expectedSearchResults.Count(), resultValue.Count());
        }

        [TestMethod]
        public async Task CardSearch_SearchSet_ReturnsOK_Test()
        {
            //arrange
            IEnumerable<MagicCardDto> expectedSearchResults = new List<MagicCardDto>()
            {
                new MagicCardDto{ },
                new MagicCardDto{ },
                new MagicCardDto{ },
                new MagicCardDto{ },
                new MagicCardDto{ },
            }.AsEnumerable();

            var mockService = new Mock<ICardSearchControllerService>(MockBehavior.Strict);

            mockService
                .Setup(p => p.SearchCardsFromSet(It.IsNotNull<CardSearchQueryParameter>()))
                .ReturnsAsync(expectedSearchResults);

            var mapperService = new MapperService();

            var cardSearchController = new CardSearchController(mockService.Object, mapperService);

            CardSearchQueryParameter queryParam = new CardSearchQueryParameter() { };

            //act
            var response = await cardSearchController.SearchSet(queryParam);

            //assert
            Assert.IsInstanceOfType(response.Result, typeof(OkObjectResult));

            var typedResult = response.Result as OkObjectResult;

            IEnumerable<LegacyMagicCardDto> resultValue = typedResult.Value as IEnumerable<LegacyMagicCardDto>;

            Assert.IsNotNull(resultValue);
            Assert.AreEqual(5, resultValue.Count());
        }

        [TestMethod]
        public async Task CardSearch_SearchInventory_ReturnsOK_Test()
        {
            //arrange
            IEnumerable<MagicCardDto> expectedSearchResults = new List<MagicCardDto>()
            {
                new MagicCardDto{ },
                new MagicCardDto{ },
                new MagicCardDto{ },
                new MagicCardDto{ },
                new MagicCardDto{ },
            }.AsEnumerable();

            var mockService = new Mock<ICardSearchControllerService>(MockBehavior.Strict);

            //SearchInventory
            mockService
                .Setup(p => p.SearchCardsFromInventory(It.IsNotNull<InventoryQueryParameter>()))
                .ReturnsAsync(expectedSearchResults);

            var mapperService = new MapperService();

            var cardSearchController = new CardSearchController(mockService.Object, mapperService);

            InventoryQueryParameter queryParam = new InventoryQueryParameter()
            {

            };

            //act
            var response = await cardSearchController.SearchInventory(queryParam);

            //assert
            Assert.IsInstanceOfType(response.Result, typeof(OkObjectResult));

            var typedResult = response.Result as OkObjectResult;

            IEnumerable<LegacyMagicCardDto> resultValue = typedResult.Value as IEnumerable<LegacyMagicCardDto>;

            Assert.IsNotNull(resultValue);
            Assert.AreEqual(expectedSearchResults.Count(), resultValue.Count());
        }
    }
}
