using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Carpentry.Tests.IntegrationTests
{
    /// <summary>
    /// This class will test Set Tracking integration tests
    /// 
    /// At the moment, I'll be using it to figure out errors adding Zendikar Rising
    /// I imagine I'll need to do the same thing for Mystery Booster
    /// </summary>
    [TestClass]
    public class TrackingIntegrationTests
    {
        readonly CarpentryFactory _factory;

        public TrackingIntegrationTests()
        {
            _factory = new CarpentryFactory();
        }

        [TestMethod]
        public async Task AddTrackedSet_AddZKR_Works()
        {
            //Get the tracked set for ZKR
            //ATM it's 214
            int setIdToRequest = 214;

            var apiEndpoint = $"api/Core/AddTrackedSet?setId={setIdToRequest}";
            var client = _factory.CreateClient();

            //var queryParamStringBody = JsonConvert.SerializeObject(queryParamToRequest, Formatting.None);

            //var queryParamStringContent = new StringContent(queryParamStringBody, Encoding.UTF8, "application/json");

            var response = await client.GetAsync(apiEndpoint);

            Assert.AreEqual(System.Net.HttpStatusCode.OK, response.StatusCode);
        }

        [TestMethod]
        public async Task UpdateSets_Works()
        {
            var apiEndpoint = $"api/Core/GetTrackedSets?showUntracked=true&update=true";
            var client = _factory.CreateClient();

            //var queryParamStringBody = JsonConvert.SerializeObject(queryParamToRequest, Formatting.None);

            //var queryParamStringContent = new StringContent(queryParamStringBody, Encoding.UTF8, "application/json");

            var response = await client.GetAsync(apiEndpoint);

            Assert.AreEqual(System.Net.HttpStatusCode.OK, response.StatusCode);
        }
    }
}
