using Carpentry.CarpentryData.Models;
using Carpentry.Legacy.Models;
using Carpentry.Logic.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Carpentry.Logic;

namespace Carpentry.Tests
{
    [TestClass]
    public class DevTests
    {

        
        [TestMethod]
        public async Task GetTrackedSets_Query_Works()
        {
            var factory = new CarpentryFactory();

            var client = factory.CreateClient();

            var response = await client.GetAsync("api/core/GetTrackedSets?showUntracked=false");

            var responseContent = await response.Content.ReadAsStringAsync();

            var searchResult = JsonConvert.DeserializeObject<List<SetDetailDto>>(responseContent);

            Assert.IsNotNull(searchResult);
        }


        [TestMethod]
        public async Task GetCollectionTotals_Query_Works()
        {
            var factory = new CarpentryFactory();

            var client = factory.CreateClient();

            var response = await client.GetAsync("api/core/GetCollectionTotals");

            var responseContent = await response.Content.ReadAsStringAsync();

            var searchResult = JsonConvert.DeserializeObject<List<InventoryTotalsByStatusResult>>(responseContent);

            Assert.IsNotNull(searchResult);
        }

        [DataRow("name")]
        //[DataRow("print")]
        //[DataRow("unique")]
        [DataTestMethod]
        public async Task SearchInventoryCards_Queries_Work(string groupBy)
        {
            var queryParam = new InventoryQueryParameter()
            {
                GroupBy = groupBy,
                //Set = "kld"
            };

            var queryParamStringBody = JsonConvert.SerializeObject(queryParam, Formatting.None);

            var queryParamStringContent = new StringContent(queryParamStringBody, Encoding.UTF8, "application/json");

            var factory = new CarpentryFactory();

            var client = factory.CreateClient();
            
            var response = await client.PostAsync("api/Inventory/SearchCards", queryParamStringContent);
            
            var responseContent = await response.Content.ReadAsStringAsync();

            var searchResult = JsonConvert.DeserializeObject<List<InventoryOverviewDto>>(responseContent);

            Assert.IsNotNull(searchResult);

            //var byNameQueryParam = new InventoryQueryParameter()
            //{
            //    GroupBy = "name"
            //};

            //var byNameResult = 

            //var byPrintQueryParam = new InventoryQueryParameter()
            //{
            //    GroupBy = "unique"
            //};

            //var byUniqueQueryParam = new InventoryQueryParameter()
            //{
            //    GroupBy = "print"
            //};

        }
        
        //add tracked set 294
        [TestMethod]
        public async Task AddTrackedSet_SLD_Works()
        {
            var setId = 294;
            var factory = new CarpentryFactory();
            var client = factory.CreateClient();
            await client.GetAsync($"api/core/AddTrackedSet?setId={setId}");
        }

        [TestMethod]
        public async Task GetTrimmingToolOverview_Works()
        {
            var factory = new CarpentryFactory();
            var client = factory.CreateClient();
            var response = await client.GetAsync($"api/trimmingTool/GetTrimmingToolOverview");
            var responseContent = await response.Content.ReadAsStringAsync();

            var searchResult = JsonConvert.DeserializeObject<TrimmingToolOverview>(responseContent);

            Assert.IsNotNull(searchResult);
        }

        [TestMethod]
        public async Task GetDeckDetail_DinoEDH_Works()
        {
            const int deckId = 31;
            var factory = new CarpentryFactory();
            var client = factory.CreateClient();
            var response = await client.GetAsync($"api/decks/GetDeckDetail?deckId={deckId}");
            var responseContent = await response.Content.ReadAsStringAsync();

            var searchResult = JsonConvert.DeserializeObject<DeckDetailDto>(responseContent);

            Assert.IsNotNull(searchResult);
        }
    }
}
