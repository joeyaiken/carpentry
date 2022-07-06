using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net;
using System.Threading.Tasks;

namespace Carpentry.Tests.ControllerStatusTests
{
    // [TestClass]
    // public class CarpentryReactLegacyControllerTests
    // {
    //     readonly CarpentryFactory<Legacy.Startup> _factory;
    //     public CarpentryReactLegacyControllerTests()
    //     {
    //         _factory = new CarpentryFactory<Legacy.Startup>();
    //     }
    //
    //     [TestMethod]
    //     public async Task CardSearchController_IsOnline_Test()
    //     {
    //         var client = _factory.CreateClient();
    //         var response = await client.GetAsync("api/CardSearch/");
    //         var responseContent = await response.Content.ReadAsStringAsync();
    //         Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
    //         Assert.AreEqual("Online", responseContent);
    //     }
    //
    //     [TestMethod]
    //     public async Task CoreController_IsOnline_Test()
    //     {
    //         var client = _factory.CreateClient();
    //         var response = await client.GetAsync("api/Core/");
    //         var responseContent = await response.Content.ReadAsStringAsync();
    //         Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
    //         Assert.AreEqual("Online", responseContent);
    //     }
    //
    //     [TestMethod]
    //     public async Task DecksController_IsOnline_Test()
    //     {
    //         var client = _factory.CreateClient();
    //         var response = await client.GetAsync("api/Decks/");
    //         var responseContent = await response.Content.ReadAsStringAsync();
    //         Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
    //         Assert.AreEqual("Online", responseContent);
    //     }
    //
    //     [TestMethod]
    //     public async Task InventoryController_IsOnline_Test()
    //     {
    //         var client = _factory.CreateClient();
    //         var response = await client.GetAsync("api/Inventory/");
    //         var responseContent = await response.Content.ReadAsStringAsync();
    //         Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
    //         Assert.AreEqual("Online", responseContent);
    //     }
    // }
}
