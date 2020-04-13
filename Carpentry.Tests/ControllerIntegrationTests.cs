using Carpentry.Data.LegacyModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Carpentry.Tests
{
    //[TestClass]
    /// <summary>
    /// I was basically trying to just invoke different repo methods
    /// If I want to do that again, I should do it through the migration tool
    /// </summary>

    public class ControllerIntegrationTests
    {

        readonly CarpentryFactory _factory;

        public ControllerIntegrationTests()
        {
            _factory = new CarpentryFactory();
        }

        //[TestMethod]
        public async Task GetByPrice_Works_Test()
        {
            var client = _factory.CreateClient();

            var response = await client.GetAsync("api/cards/GetByPrice");

            List<PricedCardDto> parsedResponse = await response.Content.ReadAsAsync<List<PricedCardDto>>();

            Assert.AreEqual(25, parsedResponse.Count);
        }

        //[TestMethod]
        public async Task InventoryBackup_CalculateTotalPrice_Test()
        {
            var client = _factory.CreateClient();

            //var updatePricesResponse = await client.GetAsync("api/Cards/UpdatePrices");
            //Assert.AreEqual(HttpStatusCode.OK, updatePricesResponse.StatusCode);

            var calculatedPriceResponse = await client.GetAsync("api/Cards/TotalPrice");

            decimal calculatedPrice = await calculatedPriceResponse.Content.ReadAsAsync<decimal>();

            Assert.AreEqual(HttpStatusCode.OK, calculatedPriceResponse.StatusCode);

            Assert.IsTrue(calculatedPrice > 0);
        }
       
        //[TestMethod]
        public async Task ClearAndLoadDb_Test()
        {
            var client = _factory.CreateClient();

            var createDbResponse = await client.GetAsync("api/Core/LoadDb");

            Assert.AreEqual(HttpStatusCode.OK, createDbResponse.StatusCode);
        }

        //[TestMethod]
        public async Task SaveDb_Test()
        {
            var client = _factory.CreateClient();

            var createDbResponse = await client.GetAsync("api/Core/SaveDb");

            Assert.AreEqual(HttpStatusCode.OK, createDbResponse.StatusCode);
        }

        //[TestMethod]
        public void BasicTestPasses()
        {
            Assert.IsTrue(true);
        }

        private CardDto CreateCardDto(MagicCardDto card)
        {
            var dto = new CardDto
            {
                Card = new Data.LegacyModels.Card
                {
                    Id = 0,
                    MultiverseId = card.MultiverseId,
                    IsFoil = false,
                    DeckId = null
                },
                Data = card
            };

            return dto;
        }

    }
}
