using Carpentry.Data.Models;
using Carpentry.Data.QueryParameters;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Carpentry.LiveDataTests
{
    /// <summary>
    /// Some specific integration tests I'm interested related to the Inventory Search
    /// These are LIVE DATA tests and should be moved to the other project
    /// </summary>
    [TestClass]
    public class SearchInventoryIntegrationTests
    {
        readonly LiveDataTestFactory _factory;

        readonly HttpClient _client;

        public SearchInventoryIntegrationTests()
        {
            _factory = new LiveDataTestFactory();
            _client = _factory.CreateClient();
        }

        //[ClassInitialize]
        //public static void InitializeClient()
        //{
        //    _client = _factory.CreateClient();
        //}

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
                GroupBy = "quantity",
                //sort by count DESC
                //Sort = "count_DESC",
                Skip = 0,
                Take = 50,
            };

            var result = await CallCardSearch(queryParameter);

            Assert.IsTrue(result.Count == 50);

        }

        [TestMethod]
        public async Task Inventory_CardDetail_GolgariGuildgate_Test()
        {
            string cardName = "Golgari Guildgate"; //Luminous Bonds
            string API_ENDPOINT_SearchInventory = $"api/Inventory/GetByName?name={cardName}";

            //var client = _factory.CreateClient();

            //var queryParamStringBody = JsonConvert.SerializeObject(queryParameter, Formatting.None);

            //var queryParamStringContent = new StringContent(queryParamStringBody, Encoding.UTF8, "application/json");

            var response = await _client.GetAsync(API_ENDPOINT_SearchInventory);

            var responseContent = await response.Content.ReadAsStringAsync();

            InventoryDetailDto searchResult = JsonConvert.DeserializeObject<InventoryDetailDto>(responseContent);

            Assert.IsTrue(searchResult.Name == cardName);
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
        






    }
}
