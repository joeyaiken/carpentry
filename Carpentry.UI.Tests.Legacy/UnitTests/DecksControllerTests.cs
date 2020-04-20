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

namespace Carpentry.UI.Tests.UnitTests
{
    [TestClass]
    public class DecksControllerTests
    {

        private readonly DecksController _decksController;

        public DecksControllerTests()
        {
            //mock deck service
            var mockDeckService = new Mock<IDeckService>(MockBehavior.Strict);

            //Add
            mockDeckService
                .Setup(p => p.AddDeck(It.IsNotNull<DeckProperties>()))
                .ReturnsAsync(1);

            //Update
            mockDeckService
                .Setup(p => p.UpdateDeck(It.IsNotNull<DeckProperties>()))
                .Returns(Task.CompletedTask);

            //Delete
            mockDeckService
                .Setup(p => p.DeleteDeck(It.Is<int>(i => i > 0)))
                .Returns(Task.CompletedTask);

            //Search
            IEnumerable<DeckProperties> searchResults = new List<DeckProperties>()
            {
                new DeckProperties{ },
                new DeckProperties{ },
                new DeckProperties{ },
                new DeckProperties{ },
                new DeckProperties{ },
            }.AsEnumerable();

            mockDeckService
                .Setup(p => p.GetDeckOverviews())
                .ReturnsAsync(searchResults);

            //Get
            mockDeckService
                .Setup(p => p.GetDeckDetail(It.Is<int>(i => i > 0)))
                .ReturnsAsync(new DeckDetail 
                { 
                    CardDetails = new List<InventoryCard>(),
                    CardOverviews = new List<InventoryOverview>(),
                    Props = new DeckProperties(),
                    Stats = new DeckStats(),
                });


            //AddCard
            mockDeckService
                //.Setup(p => p.AddDeckCard(It.IsNotNull<DeckCard>()))
                .Setup(p => p.AddDeckCard(It.Is<DeckCard>(c => c != null && c.Id == 0)))
                .Returns(Task.CompletedTask);


            //.ReturnsAsync(1);

            //UpdateCard
            mockDeckService
                //.Setup(p => p.UpdateDeckCard(It.IsNotNull<DeckCard>()))
                .Setup(p => p.UpdateDeckCard(It.Is<DeckCard>(c => c != null && c.Id > 0)))
                .Returns(Task.CompletedTask);

            //RemoveCard
            mockDeckService
                .Setup(p => p.DeleteDeckCard(It.Is<int>(i => i > 0)))
                .Returns(Task.CompletedTask);

            //var mockMapper = new Mock<IMapperService>(MockBehavior.Strict);

            //var mockRefService = Carpentry.Data.Tests.MockClasses.MockDataServices.MockDataReferenceService();

            //var mapperService = new MapperService(mockRefService.Object);

            ////create controller
            //_decksController = new DecksController(mockDeckService.Object, mapperService);
        }

        [TestMethod]
        public void Decks_GetStatus_ReturnsAsyncOK_Test()
        {
            //assemble


            //act
            var response = _decksController.Get();

            //assert
            Assert.IsInstanceOfType(response, typeof(OkObjectResult));
            var typedResult = response as OkObjectResult;
            string resultValue = typedResult.Value as string;

            Assert.AreEqual("Online", resultValue);
        }

        [TestMethod]
        public async Task Decks_Add_ReturnsAsyncOK_Test()
        {
            //assemble
            LegacyDeckPropertiesDto mockDeck = new LegacyDeckPropertiesDto
            {
                Name = "A Mock Deck",
                Notes = "A mock deck for unit testing",
                Format = "Modern", //CAN a format be null?? I hope so, otherwise I need to query a format
            };

            //act
            ActionResult<int> response = await _decksController.Add(mockDeck);

            //assert
            Assert.IsInstanceOfType(response.Result, typeof(OkObjectResult));
            var typedResult = response.Result as OkObjectResult;
            Assert.AreEqual(1, typedResult.Value);
        }
        
        [TestMethod]
        public async Task Decks_Update_ReturnsAsyncOK_Test()
        {
            //assemble
            LegacyDeckPropertiesDto mockDeck = new LegacyDeckPropertiesDto
            {
                Id = 3,
                Name = "An Existing Mock Deck",
                Notes = "A mock deck for unit testing",
                Format = "Modern",
            };

            //act
            var response = await _decksController.Update(mockDeck);

            //assert
            Assert.IsInstanceOfType(response, typeof(OkResult));
        }
        
        [TestMethod]
        public async Task Decks_Delete_ReturnsAsyncOK_Test()
        {
            //assemble
            int idToSubmit = 3;

            //act
            var response = await _decksController.Delete(idToSubmit);

            //assert
            Assert.IsInstanceOfType(response, typeof(OkResult));
        }

        [TestMethod]
        public async Task Decks_Search_ReturnsAsyncOK_Test()
        {
            //assemble

            //act
            var response = await _decksController.Search();

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
            //assemble
            int deckIdToRequest = 1;

            //act
            var response = await _decksController.Get(deckIdToRequest);

            //assert
            Assert.IsInstanceOfType(response.Result, typeof(OkObjectResult));
            var typedResult = response.Result as OkObjectResult;

            LegacyDeckDetailDto resultValue = typedResult.Value as LegacyDeckDetailDto;
            Assert.IsNotNull(resultValue);
        }
        
        [TestMethod]
        public async Task Decks_AddCard_ReturnsAsyncOK_Test()
        {
            //assemble
            LegacyDeckCardDto mockCard = new LegacyDeckCardDto
            {
                DeckId = 1,
                CategoryId = null,
                InventoryCard = new LegacyInventoryCardDto(),
            };

            //act
            var response = await _decksController.AddCard(mockCard);

            //assert
            Assert.IsInstanceOfType(response, typeof(OkResult));
        }
        
        [TestMethod]
        public async Task Decks_UpdateCard_ReturnsAsyncOK_Test()
        {
            //assemble
            LegacyDeckCardDto mockCard = new LegacyDeckCardDto
            {
                Id = 5,
                DeckId = 1,
                CategoryId = null,
                InventoryCard = new LegacyInventoryCardDto(),
            };

            //act
            var response = await _decksController.UpdateCard(mockCard);

            //assert
            Assert.IsInstanceOfType(response, typeof(OkResult));
        }
        
        [TestMethod]
        public async Task Decks_RemoveCard_ReturnsAsyncOK_Test()
        {
            //assemble
            int idToSubmit = 3;

            //act
            var response = await _decksController.RemoveCard(idToSubmit);

            //assert
            Assert.IsInstanceOfType(response, typeof(OkResult));
        }

    }
}
