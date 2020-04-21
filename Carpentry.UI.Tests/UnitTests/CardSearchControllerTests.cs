using Carpentry.Service.Interfaces;
using Carpentry.Service.Models;
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
        /// <summary>
        /// I initially created a single mock service & controller intance in the test constructor
        /// Instead, I want to arrange & mock only the service methods I expect to see called
        /// </summary>
        public CardSearchControllerTests()
        {
            ////mock service
            //var mockService = new Mock<ICardSearchService>(MockBehavior.Strict);

            ////search result payload
            //IEnumerable<MagicCardDto> searchResults = new List<MagicCardDto>()
            //{
            //    new MagicCardDto{ },
            //    new MagicCardDto{ },
            //    new MagicCardDto{ },
            //    new MagicCardDto{ },
            //    new MagicCardDto{ },
            //}.AsEnumerable();

            ////SearchWeb
            //mockService
            //    .Setup(p => p.SearchCardsFromWeb(It.IsNotNull<NameSearchQueryParameter>()))
            //    .ReturnsAsync(searchResults);

            ////SearchSet
            //mockService
            //    .Setup(p => p.SearchCardsFromSet(It.IsNotNull<CardSearchQueryParameter>()))
            //    .ReturnsAsync(searchResults);

            ////SearchInventory
            //mockService
            //    .Setup(p => p.SearchCardsFromInventory(It.IsNotNull<InventoryQueryParameter>()))
            //    .ReturnsAsync(searchResults);

            //_cardSearchController = new Controllers.CardSearchController(mockService.Object);
        }

        [TestMethod]
        public void CardSearch_GetStatus_ReturnsOK_Test()
        {
            //arrange
            var mockService = new Mock<ICardSearchControllerService>(MockBehavior.Strict);

            var cardSearchController = new Controllers.CardSearchController(mockService.Object);

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

            var cardSearchController = new Controllers.CardSearchController(mockService.Object);

            NameSearchQueryParameter queryParam = new NameSearchQueryParameter() { };

            //act
            var response = await cardSearchController.SearchWeb(queryParam);

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

            var cardSearchController = new Controllers.CardSearchController(mockService.Object);

            CardSearchQueryParameter queryParam = new CardSearchQueryParameter() { };

            //act
            var response = await cardSearchController.SearchSet(queryParam);

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

            var cardSearchController = new Controllers.CardSearchController(mockService.Object);
            
            InventoryQueryParameter queryParam = new InventoryQueryParameter()
            {

            };

            //act
            var response = await cardSearchController.SearchInventory(queryParam);

            //assert
            Assert.IsInstanceOfType(response.Result, typeof(OkObjectResult));

            var typedResult = response.Result as OkObjectResult;

            IEnumerable<MagicCardDto> resultValue = typedResult.Value as IEnumerable<MagicCardDto>;

            Assert.IsNotNull(resultValue);
            Assert.AreEqual(5, resultValue.Count());
        }

    }
}
