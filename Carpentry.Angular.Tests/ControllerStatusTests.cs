using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net;
using System.Threading.Tasks;

namespace Carpentry.Angular.Tests
{
    /// <summary>
    /// These tests all verify that all controllers can initialize correctly (and that they have all dependencies loaded)
    /// </summary>
    [TestClass]
    public class ControllerStatusTests
    {
        readonly CarpentryAngularFactory _factory;

        public ControllerStatusTests()
        {
            _factory = new CarpentryAngularFactory();
        }

        [TestMethod]
        public async Task CardSearchController_IsOnline_Test()
        {
            var client = _factory.CreateClient();
            var response = await client.GetAsync("api/CardSearch/");
            var responseContent = await response.Content.ReadAsStringAsync();
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.AreEqual("Online", responseContent);
        }

        [TestMethod]
        public async Task CoreController_IsOnline_Test()
        {
            var client = _factory.CreateClient();
            var response = await client.GetAsync("api/Core/");
            var responseContent = await response.Content.ReadAsStringAsync();
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.AreEqual("Online", responseContent);
        }

        [TestMethod]
        public async Task DecksController_IsOnline_Test()
        {
            var client = _factory.CreateClient();
            var response = await client.GetAsync("api/Decks/");
            var responseContent = await response.Content.ReadAsStringAsync();
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.AreEqual("Online", responseContent);
        }

        [TestMethod]
        public async Task InventoryController_IsOnline_Test()
        {
            var client = _factory.CreateClient();
            var response = await client.GetAsync("api/Inventory/");
            var responseContent = await response.Content.ReadAsStringAsync();
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.AreEqual("Online", responseContent);
        }
    }
}
