using Carpentry.Data.QueryParameters;
using Carpentry.Logic.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Carpentry.Tools.QueryTestApp.Tests
{
    [TestClass]
    public class ControllerIntegrationTests
    {
        readonly QueryTestAppFactory _factory;

        public ControllerIntegrationTests()
        {
            _factory = new QueryTestAppFactory();
        }

        [TestMethod]
        public async Task Controller_IsOnline_Test()
        {
            var client = _factory.CreateClient();
            var response = await client.GetAsync("api/App/");
            var responseContent = await response.Content.ReadAsStringAsync();
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.AreEqual("Online", responseContent);
        }

        [TestMethod]
        public async Task Controller_CallTestQuery_Test()
        {
            InventoryQueryParameter queryParamToRequest = new InventoryQueryParameter()
            {
                Set = "thb",
                GroupBy = "unique",
                Sort = "price",
                SortDescending = true,
                Skip = 0,
                Take = 100,
            };

            string API_ENDPOINT_CallTestQuery = "api/App/CallTestQuery";

            var queryParamStringBody = JsonConvert.SerializeObject(queryParamToRequest, Formatting.None);

            var queryParamStringContent = new StringContent(queryParamStringBody, Encoding.UTF8, "application/json");

            using(var client = _factory.CreateClient())
            {
                var response = await client.PostAsync(API_ENDPOINT_CallTestQuery, queryParamStringContent);

                var responseContent = await response.Content.ReadAsStringAsync();

                List<InventoryOverviewDto> searchResult = JsonConvert.DeserializeObject<List<InventoryOverviewDto>>(responseContent);

                Assert.IsNotNull(searchResult);
            }
        }
    }
}
