using Carpentry.Logic.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Carpentry.Tests.IntegrationTests
{
    [TestClass]
    public class DeckControllerTests
    {

        readonly CarpentryFactory _factory;

        readonly HttpClient _client;

        public DeckControllerTests()
        {
            _factory = new CarpentryFactory();
            _client = _factory.CreateClient();
        }

        [TestMethod]
        public async Task Deck_GetOverviews_Works()
        {


            string apiEndpoint = "api/Decks/GetDeckOverviews";

            //var queryParamStringBody = JsonConvert.SerializeObject(queryParamToRequest, Formatting.None);

            //var queryParamStringContent = new StringContent(queryParamStringBody, Encoding.UTF8, "application/json");

            var response = await _client.GetAsync(apiEndpoint);

            var responseContent = await response.Content.ReadAsStringAsync();

            IEnumerable<DeckOverviewDto> searchResult = JsonConvert.DeserializeObject<IEnumerable<DeckOverviewDto>>(responseContent);

            Assert.IsNotNull(searchResult);
        }
    }
}
