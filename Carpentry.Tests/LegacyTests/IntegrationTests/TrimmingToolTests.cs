using Carpentry.Data.QueryResults;
using Carpentry.Logic.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Carpentry.Tests.IntegrationTests
{
    [TestClass]
    public class TrimmingToolTests
    {
        readonly CarpentryFactory _factory;
        readonly HttpClient _client;

        public TrimmingToolTests()
        {
            _factory = new CarpentryFactory();
            _client = _factory.CreateClient();
        }


        [TestMethod]
        public async Task GetCardsToTrim_Works()
        {
            var request = new TrimmingToolRequest()
            {
                SetCode = "znr",
                MinCount = 11,
                SearchGroup = "Red",
            };

            //var result = await CallCardSearch(queryParamToRequest);
            string apiEndpoint = "api/Inventory/GetTrimmingToolCards";

            //var client = _factory.CreateClient();

            var queryParamStringBody = JsonConvert.SerializeObject(request, Formatting.None);

            var queryParamStringContent = new StringContent(queryParamStringBody, Encoding.UTF8, "application/json");

            var response = await _client.PostAsync(apiEndpoint, queryParamStringContent);

            var responseContent = await response.Content.ReadAsStringAsync();

            List<TrimmingToolResult> searchResult = JsonConvert.DeserializeObject<List<TrimmingToolResult>>(responseContent);

            Assert.IsNotNull(searchResult);
            Assert.IsTrue(searchResult.Count > 0);

            //assert every card is red
            foreach(var result in searchResult)
            {
                Assert.AreEqual("R",result.ColorIdentity);
                Assert.AreEqual(request.SetCode, result.SetCode);
                Assert.IsTrue(result.PrintTotalCount >= request.MinCount);
            }

        }
    }
}
