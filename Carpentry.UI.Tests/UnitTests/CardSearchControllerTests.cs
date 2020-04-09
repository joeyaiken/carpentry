using Carpentry.Data.QueryParameters;
using Carpentry.Logic.Interfaces;
using Carpentry.Logic.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
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
            var mockService = new Mock<ICardSearchService>();
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

        }

        //[TestInitialize]
        //public void TestInitialize()
        //{
        //}

        #region Tests - Controller methods all return Ok/Accepted

        [TestMethod]
        public void CardSearch_SearchWeb_ReturnsOK_Test()
        {
            //assemble
            //var client = _factory.CreateClient();

            //act
            //var response = await client.GetAsync("api/CardSearch/SearchWeb");
            //var responseContent = await response.Content.ReadAsStringAsync();

            //assert
            //Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            //Assert.AreEqual("Online", responseContent);





            //Add
            Assert.Fail();
            //SearchWeb
            Assert.Fail();
        }

        [TestMethod]
        public void CardSearch_SearchSet_ReturnsOK_Test()
        {
            //assemble
            //var client = _factory.CreateClient();

            //act
            //var response = await client.GetAsync("api/CardSearch/");
            //var responseContent = await response.Content.ReadAsStringAsync();

            //assert
            //Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            //Assert.AreEqual("Online", responseContent);





            //Add
            Assert.Fail();
            //SearchSet
            Assert.Fail();
        }

        [TestMethod]
        public void CardSearch_SearchInventory_ReturnsOK_Test()
        {
            //assemble
            //var client = _factory.CreateClient();

            //act
            //var response = await client.GetAsync("api/CardSearch/");
            //var responseContent = await response.Content.ReadAsStringAsync();

            //assert
            //Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            //Assert.AreEqual("Online", responseContent);





            //Add
            Assert.Fail();
            //SearchInventory
            Assert.Fail();
        }

        #endregion

    }
}
