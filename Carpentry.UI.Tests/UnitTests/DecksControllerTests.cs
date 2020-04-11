using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Carpentry.UI.Models;
using Moq;
using Carpentry.Logic.Interfaces;
using Carpentry.Logic.Models;
using Microsoft.AspNetCore.Mvc;

namespace Carpentry.UI.Tests.UnitTests
{
    [TestClass]
    public class DecksControllerTests
    {

        private readonly Controllers.DecksController _decksController;

        public DecksControllerTests()
        {
            //mock deck service
            var mockDeckService = new Mock<IDeckService>();

            //Add
            mockDeckService
                .Setup(p => p.AddDeck(It.IsNotNull<DeckProperties>()))
                .ReturnsAsync(1);

            //Update
            mockDeckService
                .Setup(p => p.UpdateDeck(It.IsNotNull<DeckProperties>()));
            //.ReturnsAsync(Task.CompletedTask);

            //Delete
            mockDeckService
                .Setup(p => p.DeleteDeck(It.Is<int>(i => i > 0)));
                //.ReturnsAsync(Task.CompletedTask);

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
                .Setup(p => p.SearchDecks())
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
                .Setup(p => p.AddDeckCard(It.Is<DeckCard>(c => c != null && c.Id == 0)));
            //.ReturnsAsync(1);

            //UpdateCard
            mockDeckService
                //.Setup(p => p.UpdateDeckCard(It.IsNotNull<DeckCard>()))
                .Setup(p => p.UpdateDeckCard(It.Is<DeckCard>(c => c != null && c.Id > 0)));
                //.ReturnsAsync(Task.CompletedTask);

            //RemoveCard
            mockDeckService
                .Setup(p => p.DeleteDeckCard(It.Is<int>(i => i > 0)));
                //.ReturnsAsync(Task.CompletedTask);

            //create controller
            _decksController = new Controllers.DecksController(mockDeckService.Object);
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
            DeckPropertiesDto mockDeck = new DeckPropertiesDto
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
            DeckPropertiesDto mockDeck = new DeckPropertiesDto
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

            IEnumerable<DeckPropertiesDto> resultValue = typedResult.Value as IEnumerable<DeckPropertiesDto>;

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

            DeckDetailDto resultValue = typedResult.Value as DeckDetailDto;
            Assert.IsNotNull(resultValue);
        }
        
        [TestMethod]
        public async Task Decks_AddCard_ReturnsAsyncOK_Test()
        {
            //assemble
            DeckCardDto mockCard = new DeckCardDto
            {
                DeckId = 1,
                CategoryId = null,
                InventoryCard = new InventoryCardDto
                {
                    
                }
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
            DeckCardDto mockCard = new DeckCardDto
            {
                Id = 5,
                DeckId = 1,
                CategoryId = null,
                InventoryCard = new InventoryCardDto
                {

                }
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
