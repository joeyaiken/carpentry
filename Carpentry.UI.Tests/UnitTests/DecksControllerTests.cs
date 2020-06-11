using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using Microsoft.AspNetCore.Mvc;
using Carpentry.Logic.Interfaces;
using Carpentry.Logic.Models;

namespace Carpentry.UI.Tests.UnitTests
{
    /// <summary>
    /// I initially created a single mock service & controller intance in the test constructor
    /// Instead, I want to arrange & mock only the service methods I expect to see called
    /// </summary>
    [TestClass]
    public class DecksControllerTests
    {
        [TestMethod]
        public void Decks_GetStatus_ReturnsAsyncOK_Test()
        {
            //Arrange            
            var mockDeckService = new Mock<IDeckService>(MockBehavior.Strict);
            var decksController = new Controllers.DecksController(mockDeckService.Object);

            //act
            var response = decksController.GetStatus();

            //assert
            Assert.IsInstanceOfType(response, typeof(OkObjectResult));
            var typedResult = response as OkObjectResult;
            string resultValue = typedResult.Value as string;

            Assert.AreEqual("Online", resultValue);
        }

        [TestMethod]
        public async Task Decks_AddDeck_ReturnsAsyncOK_Test()
        {
            //arrange
            var mockDeckService = new Mock<IDeckService>(MockBehavior.Strict);

            int expectedNewId = 1;

            mockDeckService
                .Setup(p => p.AddDeck(It.IsNotNull<DeckPropertiesDto>()))
                .ReturnsAsync(expectedNewId);

            var decksController = new Controllers.DecksController(mockDeckService.Object);
            DeckPropertiesDto mockDeck = new DeckPropertiesDto
            {
                Name = "A Mock Deck",
                Notes = "A mock deck for unit testing",
                Format = "modern",
            };

            //act
            ActionResult<int> response = await decksController.AddDeck(mockDeck);

            //assert
            Assert.IsInstanceOfType(response.Result, typeof(OkObjectResult));
            var typedResult = response.Result as OkObjectResult;
            Assert.AreEqual(expectedNewId, typedResult.Value);
        }
        
        [TestMethod]
        public async Task Decks_UpdateDeck_ReturnsAsyncOK_Test()
        {
            //arrange
            var mockDeckService = new Mock<IDeckService>(MockBehavior.Strict);

            mockDeckService
                .Setup(p => p.UpdateDeck(It.IsNotNull<DeckPropertiesDto>()))
                .Returns(Task.CompletedTask);

            var decksController = new Controllers.DecksController(mockDeckService.Object);

            DeckPropertiesDto mockDeck = new DeckPropertiesDto
            {
                Id = 3,
                Name = "An Existing Mock Deck",
                Notes = "A mock deck for unit testing",
                Format = "Modern",
            };

            //act
            var response = await decksController.UpdateDeck(mockDeck);

            //assert
            Assert.IsInstanceOfType(response, typeof(OkResult));
        }
        
        [TestMethod]
        public async Task Decks_DeleteDeck_ReturnsAsyncOK_Test()
        {
            //arrange
            var mockDeckService = new Mock<IDeckService>(MockBehavior.Strict);
            
            int idToSubmit = 1;

            mockDeckService
                .Setup(p => p.DeleteDeck(It.Is<int>(i => i == idToSubmit)))
                .Returns(Task.CompletedTask);

            var decksController = new Controllers.DecksController(mockDeckService.Object);
            
            //act
            var response = await decksController.DeleteDeck(idToSubmit);

            //assert
            Assert.IsInstanceOfType(response, typeof(OkResult));
        }

        [TestMethod]
        public async Task Decks_GetDeckOverviews_ReturnsAsyncOK_Test()
        {
            //arrange
            var mockDeckService = new Mock<IDeckService>(MockBehavior.Strict);

            IEnumerable<DeckOverviewDto> expectedSearchResults = new List<DeckOverviewDto>()
            {
                new DeckOverviewDto{ },
                new DeckOverviewDto{ },
                new DeckOverviewDto{ },
                new DeckOverviewDto{ },
                new DeckOverviewDto{ },
            }.AsEnumerable();

            mockDeckService
                .Setup(p => p.GetDeckOverviews())
                .ReturnsAsync(expectedSearchResults);

            var decksController = new Controllers.DecksController(mockDeckService.Object);
            
            //act
            var response = await decksController.GetDeckOverviews();

            //assert
            Assert.IsInstanceOfType(response.Result, typeof(OkObjectResult));
            var typedResult = response.Result as OkObjectResult;

            IEnumerable<DeckOverviewDto> resultValue = typedResult.Value as IEnumerable<DeckOverviewDto>;

            Assert.IsNotNull(resultValue);
            Assert.AreEqual(5, resultValue.Count());
        }
        
        [TestMethod]
        public async Task Decks_GetDeckDetail_ReturnsAsyncOK_Test()
        {
            //arrange
            int deckIdToRequest = 1;

            var mockDeckService = new Mock<IDeckService>(MockBehavior.Strict);

            mockDeckService
                .Setup(p => p.GetDeckDetail(It.Is<int>(i => i == deckIdToRequest)))
                .ReturnsAsync(new DeckDetailDto
                {
                    CardOverviews = new List<DeckCardOverview>(),
                    Cards = new List<DeckCard>(),
                    //CardDetails = new List<InventoryCardDto>(),
                    //CardOverviews = new List<InventoryOverviewDto>(),
                    Props = new DeckPropertiesDto(),
                    Stats = new DeckStatsDto(),
                });

            var decksController = new Controllers.DecksController(mockDeckService.Object);
            

            //act
            var response = await decksController.GetDeckDetail(deckIdToRequest);

            //assert
            Assert.IsInstanceOfType(response.Result, typeof(OkObjectResult));
            var typedResult = response.Result as OkObjectResult;

            DeckDetailDto resultValue = typedResult.Value as DeckDetailDto;
            Assert.IsNotNull(resultValue);
        }
        
        [TestMethod]
        public async Task Decks_AddDeckCard_ReturnsAsyncOK_Test()
        {
            //arrange
            var mockDeckService = new Mock<IDeckService>(MockBehavior.Strict);

            mockDeckService
                .Setup(p => p.AddDeckCard(It.Is<DeckCardDto>(c => c != null && c.Id == 0)))
                .Returns(Task.CompletedTask);

            var decksController = new Controllers.DecksController(mockDeckService.Object);

            DeckCardDto mockCard = new DeckCardDto
            {
                DeckId = 1,
                CategoryId = null,
                InventoryCard = new InventoryCardDto(),
            };

            //act
            var response = await decksController.AddDeckCard(mockCard);

            //assert
            Assert.IsInstanceOfType(response, typeof(OkResult));
        }
        
        [TestMethod]
        public async Task Decks_UpdateDeckCard_ReturnsAsyncOK_Test()
        {
            //arrange
            var mockDeckService = new Mock<IDeckService>(MockBehavior.Strict);

            mockDeckService
                .Setup(p => p.UpdateDeckCard(It.Is<DeckCardDto>(c => c != null && c.Id > 0)))
                .Returns(Task.CompletedTask);

            var decksController = new Controllers.DecksController(mockDeckService.Object);

            DeckCardDto mockCard = new DeckCardDto
            {
                Id = 5,
                DeckId = 1,
                CategoryId = null,
                InventoryCard = new InventoryCardDto(),
            };

            //act
            var response = await decksController.UpdateDeckCard(mockCard);

            //assert
            Assert.IsInstanceOfType(response, typeof(OkResult));
        }
        
        [TestMethod]
        public async Task Decks_RemoveDeckCard_ReturnsAsyncOK_Test()
        {
            //arrange
            var mockDeckService = new Mock<IDeckService>(MockBehavior.Strict);

            int idToSubmit = 3;

            mockDeckService
                .Setup(p => p.DeleteDeckCard(It.Is<int>(i => i == idToSubmit)))
                .Returns(Task.CompletedTask);

            var decksController = new Controllers.DecksController(mockDeckService.Object);

            //act
            var response = await decksController.RemoveDeckCard(idToSubmit);

            //assert
            Assert.IsInstanceOfType(response, typeof(OkResult));
        }

    }
}
