using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using Carpentry.Service.Interfaces;
using Carpentry.Service.Models;
using Microsoft.AspNetCore.Mvc;

namespace Carpentry.UI.Tests.UnitTests
{
    [TestClass]
    public class DecksControllerTests
    {

        //private readonly Controllers.DecksController _decksController;

        /// <summary>
        /// I initially created a single mock service & controller intance in the test constructor
        /// Instead, I want to arrange & mock only the service methods I expect to see called
        /// </summary>
        public DecksControllerTests()
        {
            ////mock deck service
            //var mockDeckService = new Mock<IDeckService>(MockBehavior.Strict);

            ////Add
            //mockDeckService
            //    .Setup(p => p.AddDeck(It.IsNotNull<DeckPropertiesDto>()))
            //    .ReturnsAsync(1);

            ////Update
            //mockDeckService
            //    .Setup(p => p.UpdateDeck(It.IsNotNull<DeckPropertiesDto>()))
            //    .Returns(Task.CompletedTask);

            ////Delete
            //mockDeckService
            //    .Setup(p => p.DeleteDeck(It.Is<int>(i => i > 0)))
            //    .Returns(Task.CompletedTask);

            ////Search
            //IEnumerable<DeckPropertiesDto> searchResults = new List<DeckPropertiesDto>()
            //{
            //    new DeckPropertiesDto{ },
            //    new DeckPropertiesDto{ },
            //    new DeckPropertiesDto{ },
            //    new DeckPropertiesDto{ },
            //    new DeckPropertiesDto{ },
            //}.AsEnumerable();

            //mockDeckService
            //    .Setup(p => p.GetDeckOverviews())
            //    .ReturnsAsync(searchResults);

            ////Get
            //mockDeckService
            //    .Setup(p => p.GetDeckDetail(It.Is<int>(i => i > 0)))
            //    .ReturnsAsync(new DeckDetailDto
            //    { 
            //        CardDetails = new List<InventoryCardDto>(),
            //        CardOverviews = new List<InventoryOverviewDto>(),
            //        Props = new DeckPropertiesDto(),
            //        Stats = new DeckStatsDto(),
            //    });


            ////AddCard
            //mockDeckService
            //    //.Setup(p => p.AddDeckCard(It.IsNotNull<DeckCard>()))
            //    .Setup(p => p.AddDeckCard(It.Is<DeckCardDto>(c => c != null && c.Id == 0)))
            //    .Returns(Task.CompletedTask);


            ////.ReturnsAsync(1);

            ////UpdateCard
            //mockDeckService
            //    //.Setup(p => p.UpdateDeckCard(It.IsNotNull<DeckCard>()))
            //    .Setup(p => p.UpdateDeckCard(It.Is<DeckCardDto>(c => c != null && c.Id > 0)))
            //    .Returns(Task.CompletedTask);

            ////RemoveCard
            //mockDeckService
            //    .Setup(p => p.DeleteDeckCard(It.Is<int>(i => i > 0)))
            //    .Returns(Task.CompletedTask);

            ////create controller
            //_decksController = new Controllers.DecksController(mockDeckService.Object, mapperService);
        }

        [TestMethod]
        public void Decks_GetStatus_ReturnsAsyncOK_Test()
        {
            //Arrange            
            var mockDeckService = new Mock<IDeckService>(MockBehavior.Strict);
            var decksController = new Controllers.DecksController(mockDeckService.Object);

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

            var decksController = new Controllers.DecksController(mockDeckService.Object);
            DeckPropertiesDto mockDeck = new DeckPropertiesDto
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

            var decksController = new Controllers.DecksController(mockDeckService.Object);

            DeckPropertiesDto mockDeck = new DeckPropertiesDto
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

            var decksController = new Controllers.DecksController(mockDeckService.Object);
            
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

            var decksController = new Controllers.DecksController(mockDeckService.Object);
            
            //act
            var response = await decksController.Search();

            //assert
            Assert.IsInstanceOfType(response.Result, typeof(OkObjectResult));
            var typedResult = response.Result as OkObjectResult;

            IEnumerable<DeckPropertiesDto> resultValue = typedResult.Value as IEnumerable<DeckPropertiesDto>;

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

            var decksController = new Controllers.DecksController(mockDeckService.Object);
            

            //act
            var response = await decksController.Get(deckIdToRequest);

            //assert
            Assert.IsInstanceOfType(response.Result, typeof(OkObjectResult));
            var typedResult = response.Result as OkObjectResult;

            DeckDetailDto resultValue = typedResult.Value as DeckDetailDto;
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

            var decksController = new Controllers.DecksController(mockDeckService.Object);

            DeckCardDto mockCard = new DeckCardDto
            {
                DeckId = 1,
                CategoryId = null,
                InventoryCard = new InventoryCardDto(),
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

            var decksController = new Controllers.DecksController(mockDeckService.Object);

            DeckCardDto mockCard = new DeckCardDto
            {
                Id = 5,
                DeckId = 1,
                CategoryId = null,
                InventoryCard = new InventoryCardDto(),
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

            var decksController = new Controllers.DecksController(mockDeckService.Object);

            //act
            var response = await decksController.RemoveCard(idToSubmit);

            //assert
            Assert.IsInstanceOfType(response, typeof(OkResult));
        }

    }
}
