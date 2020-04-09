using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Carpentry.UI.Tests.IntegrationTests
{
    //[TestClass]
    public class CardSearchControllerTests
    {
        readonly CarpentryFactory _factory;

        public CardSearchControllerTests()
        {
            _factory = new CarpentryFactory();
        }

        //[TestInitialize]
        //public void TestInitialize()
        //{
        //}

        #region Tests - Controller methods all return Ok/Accepted

        [TestMethod]
        public async void CardSearch_SearchWeb_ReturnsOK_Test()
        {
            //assemble
            var client = _factory.CreateClient();

            //act
            var response = await client.GetAsync("api/CardSearch/SearchWeb");
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
            var client = _factory.CreateClient();

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
            var client = _factory.CreateClient();

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
