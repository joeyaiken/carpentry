using Carpentry.Data.Models;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.Sqlite;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http;
using System.Text;


namespace Carpentry.Tests.TestClasses
{
    [TestClass]
    public class DecksControllerIntegrationTests
    {
        readonly CarpentryFactory _factory;

        public DecksControllerIntegrationTests()
        {
            _factory = new CarpentryFactory();
        }

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            Console.WriteLine("ClassInitialize");
        }

        [TestInitialize]
        public async Task TestInitialize()
        {
            var client = _factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(async services =>
                {
                    var serviceProvider = services.BuildServiceProvider();

                    using (var scope = serviceProvider.CreateScope())
                    {
                        var scopedServices = scope.ServiceProvider;
                        //var testService = scopedServices.GetRequiredService<TestDataService>();
                        //await testService.ResetDbAsync();
                        //await testService.SeedDeckTestRecords();
                    }
                });
            })
            .CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });

        }

        //[TestMethod]
        //public void TestsCanIn()
        //{
        //    Assert.IsTrue(true);
        //}

        #region /Decks/Add

        //Decks/Add
        //  Add a new deck (w/ or w/o basic lands)?
        [TestMethod]
        public async Task Decks_Add_CanAddSuccessfully_Test()
        {
            Assert.Fail();
        }

        //  Invalid add?
        [TestMethod]
        public async Task Decks_Add_NamesMustBeUnique_Test()
        {
            Assert.Fail();
        }

        [TestMethod]
        public async Task Decks_Add_MustHaveRequiredFields_Test()
        {
            Assert.Fail();
        }

        #endregion

        #region /Decks/Update

        //Decks/Update
        //  Update the properties of a deck
        [TestMethod]
        public async Task Decks_Update_ProperlySavesChanges_Test()
        {
            Assert.Fail();
        }

        //  Invalid update?
        [TestMethod]
        public async Task Decks_Update_ThrowsExpectedErrors_Test()
        {
            Assert.Fail();
        }

        #endregion

        #region /Decks/Delete

        //Decks/Delete
        //  Delete the blue deck
        [TestMethod]
        public async Task Decks_Delete_CanDeleteBlueDeck_Test()
        {
            Assert.Fail();
        }

        [TestMethod]
        public async Task Decks_Delete_ThrowsExpectedErrors_Test()
        {
            Assert.Fail();
        }

        #endregion

        #region /Decks/Search

        //Decks/Search
        //  Search deck properties
        [TestMethod]
        public async Task Decks_Search_ReturnsSampleDecks_Test()
        {
            //Assemble
            string API_ENDPOINT = "api/Decks/Search";
            List<DeckProperties> searchResults;

            //Act
            using (var client = _factory.CreateClient())
            {


                //var response = await client.PostAsync(API_ENDPOINT, queryParamStringContent);
                var response = await client.PostAsync(API_ENDPOINT, null);

                var responseContent = await response.Content.ReadAsStringAsync();

                //List<DeckProperties> searchResult = JsonConvert.DeserializeObject<List<DeckProperties>>(responseContent);

                searchResults = JsonConvert.DeserializeObject<List<DeckProperties>>(responseContent);
            }

            //Assert

            //Expecting 5 mock decks
            Assert.IsTrue(searchResults.Count == 5);

            //TODO: Add more assertions
        }

        #endregion

        #region /Decks/Get

        //Get

        //  Get the details of a deck
        [TestMethod]
        public async Task Decks_Get_SelectRedDeck_Test()
        {
            Assert.Fail();

            //Assemble
            string API_ENDPOINT = "api/Decks/Get";
            List<DeckProperties> searchResults;

            //Act
            using (var client = _factory.CreateClient())
            {


                //var response = await client.PostAsync(API_ENDPOINT, queryParamStringContent);
                var response = await client.PostAsync(API_ENDPOINT, null);

                var responseContent = await response.Content.ReadAsStringAsync();

                //List<DeckProperties> searchResult = JsonConvert.DeserializeObject<List<DeckProperties>>(responseContent);

                searchResults = JsonConvert.DeserializeObject<List<DeckProperties>>(responseContent);
            }

            //Assert

            //Expecting 5 mock decks
            Assert.IsTrue(searchResults.Count == 5);

            //TODO: Add more assertions
        }

        //  Invalid deck?
        [TestMethod]
        public async Task Decks_Get_InvalidIdReturnsError_Test()
        {

            //Assemble
            string API_ENDPOINT = $"api/Decks/Get?{6}";
            HttpStatusCode responseCode;

            //Act
            using (var client = _factory.CreateClient())
            {
                //There shouldn't be a 6th deck
                var response = await client.GetAsync(API_ENDPOINT);

                responseCode = response.StatusCode;
            }

            //Assert

            //Expecting 5 mock decks
            Assert.AreEqual(HttpStatusCode.BadRequest, responseCode);
        }

        #endregion

        #region /Decks/AddCard


        //AddCard
        //  Add a card & buylist card
        [TestMethod]
        public async Task Decks_AddCard_AddsNewBuylistCard_Test()
        {
            Assert.Fail();
        }

        //  Add deck card and NEW inventory card
        [TestMethod]
        public async Task Decks_AddCard_AddsNewInventoryCard_Test()
        {
            Assert.Fail();
        }

        //  Add existing inventory card to deck
        [TestMethod]
        public async Task Decks_AddCard_AddInventoryCardToDeck_Test()
        {
            Assert.Fail();
        }

        //  Invalid add? Maybe a bad Inventory record?
        [TestMethod]
        public async Task Decks_AddCard_ThrowsExpectedErrors_Test()
        {
            Assert.Fail();
        }

        #endregion

        #region /Decks/RemoveCard

        //RemoveCard
        //  Remove a card from a deck
        [TestMethod]
        public async Task Decks_RemoveCard_LeavesInventoryCardIntact_Test()
        {
            Assert.Fail();
        }

        //  Remove a deck card AND inventory card?
        [TestMethod]
        public async Task Decks_RemoveCard_AlsoDeletesInventoryCard_Test()
        {
            Assert.Fail();
        }

        //  Remove a deck card AND move inventory card to the buylist?
        [TestMethod]
        public async Task Decks_RemoveCard_MovesInventoryCardToBuylist_Test()
        {
            Assert.Fail();
        }

        #endregion

    }
}
