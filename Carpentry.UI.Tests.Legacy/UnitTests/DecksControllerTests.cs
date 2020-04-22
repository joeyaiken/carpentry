using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Carpentry.UI.Legacy.Models;
using Moq;
using Carpentry.Logic.Interfaces;
using Carpentry.Logic.Models;
using Microsoft.AspNetCore.Mvc;
using Carpentry.UI.Legacy.Util;
using Carpentry.UI.Legacy.Controllers;

namespace Carpentry.UI.Tests.Legacy.UnitTests
{
    [TestClass]
    public class DecksControllerTests
    {
        [TestMethod]
        public void Decks_GetStatus_ReturnsAsyncOK_Test()
        {
            //Arrange            
            var mockDeckService = new Mock<IDeckService>(MockBehavior.Strict);
            var mapperService = new MapperService();
            var decksController = new DecksController(mockDeckService.Object, mapperService);

            //act
            var response = decksController.Get();

            //assert
            Assert.IsInstanceOfType(response, typeof(OkObjectResult));
            var typedResult = response as OkObjectResult;
            string resultValue = typedResult.Value as string;

            Assert.AreEqual("Online", resultValue);
        }

        [TestMethod]
        public async Task Decks_Add_ReturnsAsyncOK_Test()
        {
            //arrange
            var mockDeckService = new Mock<IDeckService>(MockBehavior.Strict);

            int expectedNewId = 1;

            mockDeckService
                .Setup(p => p.AddDeck(It.IsNotNull<DeckPropertiesDto>()))
                .ReturnsAsync(expectedNewId);

            var mapperService = new MapperService();

            var decksController = new DecksController(mockDeckService.Object, mapperService);

            LegacyDeckPropertiesDto mockDeck = new LegacyDeckPropertiesDto
            {
                Name = "A Mock Deck",
                Notes = "A mock deck for unit testing",
                Format = "modern",
            };

            //act
            ActionResult<int> response = await decksController.Add(mockDeck);

            //assert
            Assert.IsInstanceOfType(response.Result, typeof(OkObjectResult));
            var typedResult = response.Result as OkObjectResult;
            Assert.AreEqual(expectedNewId, typedResult.Value);
        }

        [TestMethod]
        public async Task Decks_Update_ReturnsAsyncOK_Test()
        {
            //arrange
            var mockDeckService = new Mock<IDeckService>(MockBehavior.Strict);

            mockDeckService
                .Setup(p => p.UpdateDeck(It.IsNotNull<DeckPropertiesDto>()))
                .Returns(Task.CompletedTask);

            var mapperService = new MapperService();

            var decksController = new DecksController(mockDeckService.Object, mapperService);

            LegacyDeckPropertiesDto mockDeck = new LegacyDeckPropertiesDto
            {
                Id = 3,
                Name = "An Existing Mock Deck",
                Notes = "A mock deck for unit testing",
                Format = "Modern",
            };

            //act
            var response = await decksController.Update(mockDeck);

            //assert
            Assert.IsInstanceOfType(response, typeof(OkResult));
        }

        [TestMethod]
        public async Task Decks_Delete_ReturnsAsyncOK_Test()
        {
            //arrange
            var mockDeckService = new Mock<IDeckService>(MockBehavior.Strict);

            int idToSubmit = 1;

            mockDeckService
                .Setup(p => p.DeleteDeck(It.Is<int>(i => i == idToSubmit)))
                .Returns(Task.CompletedTask);

            var mapperService = new MapperService();

            var decksController = new DecksController(mockDeckService.Object, mapperService);

            //act
            var response = await decksController.Delete(idToSubmit);

            //assert
            Assert.IsInstanceOfType(response, typeof(OkResult));
        }

        [TestMethod]
        public async Task Decks_Search_ReturnsAsyncOK_Test()
        {
            //arrange
            var mockDeckService = new Mock<IDeckService>(MockBehavior.Strict);

            IEnumerable<DeckPropertiesDto> expectedSearchResults = new List<DeckPropertiesDto>()
            {
                new DeckPropertiesDto{ },
                new DeckPropertiesDto{ },
                new DeckPropertiesDto{ },
                new DeckPropertiesDto{ },
                new DeckPropertiesDto{ },
            }.AsEnumerable();

            mockDeckService
                .Setup(p => p.GetDeckOverviews())
                .ReturnsAsync(expectedSearchResults);

            var mapperService = new MapperService();

            var decksController = new DecksController(mockDeckService.Object, mapperService);

            //act
            var response = await decksController.Search();

            //assert
            Assert.IsInstanceOfType(response.Result, typeof(OkObjectResult));
            var typedResult = response.Result as OkObjectResult;

            IEnumerable<LegacyDeckPropertiesDto> resultValue = typedResult.Value as IEnumerable<LegacyDeckPropertiesDto>;

            Assert.IsNotNull(resultValue);
            Assert.AreEqual(5, resultValue.Count());
        }

        [TestMethod]
        public async Task Decks_Get_ReturnsAsyncOK_Test()
        {
            //arrange
            int deckIdToRequest = 1;

            var mockDeckService = new Mock<IDeckService>(MockBehavior.Strict);

            mockDeckService
                .Setup(p => p.GetDeckDetail(It.Is<int>(i => i == deckIdToRequest)))
                .ReturnsAsync(new DeckDetailDto
                {
                    CardDetails = new List<InventoryCardDto>(),
                    CardOverviews = new List<InventoryOverviewDto>(),
                    Props = new DeckPropertiesDto(),
                    Stats = new DeckStatsDto(),
                });

            var mapperService = new MapperService();

            var decksController = new DecksController(mockDeckService.Object, mapperService);


            //act
            var response = await decksController.Get(deckIdToRequest);

            //assert
            Assert.IsInstanceOfType(response.Result, typeof(OkObjectResult));
            var typedResult = response.Result as OkObjectResult;

            LegacyDeckDetailDto resultValue = typedResult.Value as LegacyDeckDetailDto;
            Assert.IsNotNull(resultValue);
        }

        [TestMethod]
        public async Task Decks_AddCard_ReturnsAsyncOK_Test()
        {
            //arrange
            var mockDeckService = new Mock<IDeckService>(MockBehavior.Strict);

            mockDeckService
                .Setup(p => p.AddDeckCard(It.Is<DeckCardDto>(c => c != null && c.Id == 0)))
                .Returns(Task.CompletedTask);

            var mapperService = new MapperService();

            var decksController = new DecksController(mockDeckService.Object, mapperService);

            LegacyDeckCardDto mockCard = new LegacyDeckCardDto
            {
                DeckId = 1,
                CategoryId = null,
                InventoryCard = new LegacyInventoryCardDto(),
            };

            //act
            var response = await decksController.AddCard(mockCard);

            //assert
            Assert.IsInstanceOfType(response, typeof(OkResult));
        }

        [TestMethod]
        public async Task Decks_UpdateCard_ReturnsAsyncOK_Test()
        {
            //arrange
            var mockDeckService = new Mock<IDeckService>(MockBehavior.Strict);

            mockDeckService
                .Setup(p => p.UpdateDeckCard(It.Is<DeckCardDto>(c => c != null && c.Id > 0)))
                .Returns(Task.CompletedTask);

            var mapperService = new MapperService();

            var decksController = new DecksController(mockDeckService.Object, mapperService);

            LegacyDeckCardDto mockCard = new LegacyDeckCardDto
            {
                Id = 5,
                DeckId = 1,
                CategoryId = null,
                InventoryCard = new LegacyInventoryCardDto(),
            };

            //act
            var response = await decksController.UpdateCard(mockCard);

            //assert
            Assert.IsInstanceOfType(response, typeof(OkResult));
        }

        [TestMethod]
        public async Task Decks_RemoveCard_ReturnsAsyncOK_Test()
        {
            //arrange
            var mockDeckService = new Mock<IDeckService>(MockBehavior.Strict);

            int idToSubmit = 3;

            mockDeckService
                .Setup(p => p.DeleteDeckCard(It.Is<int>(i => i == idToSubmit)))
                .Returns(Task.CompletedTask);

            var mapperService = new MapperService();

            var decksController = new DecksController(mockDeckService.Object, mapperService);

            //act
            var response = await decksController.RemoveCard(idToSubmit);

            //assert
            Assert.IsInstanceOfType(response, typeof(OkResult));
        }

    }
}
