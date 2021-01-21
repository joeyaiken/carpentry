using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Carpentry.Tests.IntegrationTests
{
    [TestClass]
    public class DeckExportTests
    {
        readonly CarpentryFactory _factory;

        //readonly HttpClient _client;

        public DeckExportTests()
        {
            _factory = new CarpentryFactory();
            //_client = _factory.CreateClient();
        }

        [TestMethod]
        public async Task ExportDeck_BySuggestions_Works()
        {
            int deckIdToRequest = 52;

            var apiEndpoint = $"api/Decks/ExportDeckList?deckId={deckIdToRequest}&exportType=suggestions";
            var client = _factory.CreateClient();

            var response = await client.GetAsync(apiEndpoint);

            Assert.AreEqual(System.Net.HttpStatusCode.OK, response.StatusCode);

            var responseContent = await response.Content.ReadAsStringAsync();

            Assert.IsFalse(string.IsNullOrEmpty(responseContent));
        }

        [TestMethod]
        public async Task ExportDeck_ByList_Works()
        {
            int deckIdToRequest = 40;

            var apiEndpoint = $"api/Decks/ExportDeckList?deckId={deckIdToRequest}";
            var client = _factory.CreateClient();

            var response = await client.GetAsync(apiEndpoint);

            Assert.AreEqual(System.Net.HttpStatusCode.OK, response.StatusCode);

            var responseContent = await response.Content.ReadAsStringAsync();

            Assert.IsFalse(string.IsNullOrEmpty(responseContent));
        }

        [TestMethod]
        public async Task ExportDeck_WithTags_Works()
        {
            int deckIdToRequest = 1;

            var apiEndpoint = $"api/Decks/ExportDeckList?deckId={deckIdToRequest}&exportType=detail";
            var client = _factory.CreateClient();

            var response = await client.GetAsync(apiEndpoint);

            Assert.AreEqual(System.Net.HttpStatusCode.OK, response.StatusCode);

            var responseContent = await response.Content.ReadAsStringAsync();

            Assert.IsFalse(string.IsNullOrEmpty(responseContent));
        }

    }
}
