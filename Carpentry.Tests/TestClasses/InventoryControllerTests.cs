using Carpentry.Data.LegacyModels;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Carpentry.Data.QueryParameters;

namespace Carpentry.Tests.TestClasses
{
    //[TestClass]
    public class InventoryControllerIntegrationTests
    {
        readonly CarpentryFactory _factory;
        readonly HttpClient _client;

        public InventoryControllerIntegrationTests()
        {
            _factory = new CarpentryFactory();
            _client = _factory.CreateClient();
        }

        //[TestInitialize]
        //public async Task TestInitialize()
        //{
        //    var client = _factory.WithWebHostBuilder(builder =>
        //    {
        //        builder.ConfigureServices(async services =>
        //        {
        //            var serviceProvider = services.BuildServiceProvider();

        //            using (var scope = serviceProvider.CreateScope())
        //            {
        //                var scopedServices = scope.ServiceProvider;
        //                var testService = scopedServices.GetRequiredService<TestDataService>();
        //                await testService.ResetDbAsync();
        //                //await testService.SeedDeckTestRecords();
        //            }
        //        });
        //    })
        //    .CreateClient(new WebApplicationFactoryClientOptions
        //    {
        //        AllowAutoRedirect = false
        //    });

        //}

        #region /Inventory/Add

        //Add
        //  Add buylist card
        [TestMethod]
        public void Inventory_Add_AddBuylistCard_Test()
        {
            Assert.Fail();
        }

        //  Add inventory card w/ a Card data obj
        [TestMethod]
        public void Inventory_Add_AddNewInventoryCardWithData_Test()
        {
            Assert.Fail();
        }

        //  Add inventory card w/o Card data obj
        [TestMethod]
        public void Inventory_Add_AdditionalInventoryCard_Test()
        {
            Assert.Fail();
        }

        [TestMethod]
        public void Inventory_Add_AdditionalInventoryCardWithoutData_Test()
        {
            Assert.Fail();
        }

        //  add invalid card?
        [TestMethod]
        public void Inventory_Add_ThrowsExpectedErrors_Test()
        {
            Assert.Fail();
        }

        //  Add sellList card?
        [TestMethod]
        public void Inventory_Add_AddSelllistCard_Test()
        {
            Assert.Fail();
        }

        #endregion

        #region /Inventory/Update
        //Update
        //  Move a buylist card to the inventory
        [TestMethod]
        public void Inventory_Update_MoveBuylistCardToInventory_Test()
        {
            Assert.Fail();
        }
        //  Move an inventory card to the sell list
        [TestMethod]
        public void Inventory_Update_MoveInventoryCardToSellList_Test()
        {
            Assert.Fail();
        }
        //  Invalid move?
        [TestMethod]
        public void Inventory_Update_ThrowsExpectedErrors_Test()
        {
            Assert.Fail();
        }

        //  Move a card, contained in a deck, to the sellList (incorporate into the above test?)
        [TestMethod]
        public void Inventory_Update_MoveDeckCardToSelllist_ThrowsAnError_Test()
        {
            Assert.Fail();
        }

        #endregion

        #region /Inventory/Delete
        //Delete
        //  Delete a card that exists in a deck
        [TestMethod]
        public void Inventory_Delete_CardExistsInADeck_Test()
        {
            Assert.Fail();
        }
        //  Delete a card that doesn't exist in a deck
        [TestMethod]
        public void Inventory_Delete_CardNotInADeck_Test()
        {
            Assert.Fail();
        }

        //Expected errors
        [TestMethod]
        public void Inventory_Delete_ThrowsExpectedErrors_Test()
        {
            Assert.Fail();
        }

        #endregion

        #region /Inventory/Search

        private async Task<List<InventoryQueryResult>> CallCardSearch(InventoryQueryParameter queryParameter)
        {
            string API_ENDPOINT_SearchInventory = "api/Inventory/Search";

            //var client = _factory.CreateClient();

            var queryParamStringBody = JsonConvert.SerializeObject(queryParameter, Formatting.None);

            var queryParamStringContent = new StringContent(queryParamStringBody, Encoding.UTF8, "application/json");

            var response = await _client.PostAsync(API_ENDPOINT_SearchInventory, queryParamStringContent);

            var responseContent = await response.Content.ReadAsStringAsync();

            List<InventoryQueryResult> searchResult = JsonConvert.DeserializeObject<List<InventoryQueryResult>>(responseContent);

            return searchResult;
        }

        //Get all unique names, order by count, 50
        //"What named cards do I have the most of? (top 50)"
        [TestMethod]
        public async Task InventorySearch_MostNamedCards_Test()
        {
            InventoryQueryParameter queryParameter = new InventoryQueryParameter()
            {
                //group by unique names
                GroupBy = "name",
                //sort by count DESC
                Sort = "count",
                Skip = 0,
                Take = 50,
            };

            var result = await CallCardSearch(queryParameter);

            Assert.IsTrue(result.Count == 50);

            //Assert.Fail();
        }


        //What Standard-Legal Knights do I own?
        [TestMethod]
        public async Task InventorySearch_StandardKnights_Test()
        {


            InventoryQueryParameter queryParameter = new InventoryQueryParameter()
            {
                Types = new List<string>() { "Knight" }, //Should this be a Type filter or Text filter?
                Format = "standard",
                GroupBy = "unique",
            };

            var result = await CallCardSearch(queryParameter);

            Assert.IsTrue(result.Count > 0);

            var totalCount = result.Select(x => x.Items.Count).Sum();

            result.ForEach(item =>
            {

                //Ensure none of the results aren't standard-legal

                //Ensure all of the results are knights


                //If there are multiple 'cards', they should be instances / variants of the same card?
                //Has to do with how items are grouped

                Assert.IsTrue(item.Cards.Count == 1);

                Assert.IsTrue(item.Items.Count > 0);

                var firstCard = item.Cards[0];

                Assert.IsTrue(firstCard.Type.ToLower().Contains("knight"));

                //TODO: Update MagicCardDto to include formats so I can validate all cards are standard
                //Assert.IsTrue(firstCard.)

                Assert.IsTrue(firstCard.Legalities.Contains("standard"));

            });

            //Assert.Fail();
        }

        //What unique set/name cards do I have the most of?
        [TestMethod]
        public async Task InventorySearch_MostUniqueSetCards_Test()
        {
            InventoryQueryParameter queryParameter = new InventoryQueryParameter()
            {
                //group by unique name / set combos
                GroupBy = "unique",
                //sort by count DESC
                Sort = "count_DESC",
                Skip = 0,
                Take = 50,
            };

            var result = await CallCardSearch(queryParameter);

            var totalCount = result.Select(x => x.Items.Count).Sum();

            Assert.IsTrue(result.Count == 50);

            //How do I verify the rest?
            Assert.Fail();

        }

        [TestMethod]
        public async Task InventorySearch_SnowCards_Test()
        {
            InventoryQueryParameter queryParameter = new InventoryQueryParameter()
            {
                GroupBy = "unique",
                Text = "snow"
            };

            var result = await CallCardSearch(queryParameter);

            Assert.IsTrue(result.Count > 0);

            result.ForEach(item =>
            {

                //Ensure all results contain 'snow' in one of name/type/text

                Assert.IsTrue(item.Cards.Count == 1);

                Assert.IsTrue(item.Items.Count > 0);

                var firstCard = item.Cards[0];

                Assert.IsTrue(
                    firstCard.Type.ToLower().Contains("snow")
                    ||
                    firstCard.Text.ToLower().Contains("snow")
                    ||
                    firstCard.Name.ToLower().Contains("snow")
                    );
            });
        }

        [TestMethod]
        public async Task SearchInventory_CanFilterByColors_Test()
        {

            Assert.Fail();

        }

        [TestMethod]
        public async Task SearchInventory_CanFilterByType_Test()
        {

            Assert.Fail();

        }

        [TestMethod]
        public async Task SearchInventory_CanFilterBySet_Test()
        {

            Assert.Fail();

        }

        [TestMethod]
        public async Task SearchInventory_CanFilterByText_Test()
        {

            Assert.Fail();

        }

        [TestMethod]
        public async Task SearchInventory_CanGroupResults_Test()
        {

            Assert.Fail();

        }

        [TestMethod]
        public async Task SearchInventory_EmptyFilter_ReturnsData_Test()
        {

            Assert.Fail();

        }

        [TestMethod]
        public async Task SearchInventory_NullFilter_ReturnsBadRequest_Test()
        {

            Assert.Fail();

            //InventoryQueryParameter queryParameter = null;
            InventoryQueryParameter queryParameter = new InventoryQueryParameter();
            string API_ENDPOINT_SearchInventory = "api/Inventory/Search";

            //List<FilterDescriptor> queryParameter = new List<FilterDescriptor>()
            //{
            //    new FilterDescriptor{ Name="Set", Value="MH1" },
            //    new FilterDescriptor{ Name="ColorIdentity", Value="R" },
            //    new FilterDescriptor{ Name="Rarity", Value="Uncommon" },
            //    new FilterDescriptor{ Name="Type", Value="Creature" }

            //};


            var queryParamStringBody = JsonConvert.SerializeObject(queryParameter, Formatting.None);
            var queryParamStringContent = new StringContent(queryParamStringBody, Encoding.UTF8, "application/json");

            var client = _factory.CreateClient();

            var response = await client.PostAsync(API_ENDPOINT_SearchInventory, queryParamStringContent);

            //var responseContent = response.Content.ReadAsStringAsync();

            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);

            //List<InventoryQueryResult> searchResult = JsonConvert.DeserializeObject<List<InventoryQueryResult>>(responseContent.Result);


        }

        [TestMethod]
        public async Task SearchInventory_PaginationWorks_Test()
        {

            Assert.Fail();

        }

        #endregion

    }
}
