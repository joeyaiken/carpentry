using Carpentry.CarpentryData.Models;
using Carpentry.Logic.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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


    }
}
