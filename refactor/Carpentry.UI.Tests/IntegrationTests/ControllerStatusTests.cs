using System.Net;

namespace Carpentry.UI.Tests.IntegrationTests;

[TestClass]
public class ControllerStatusTests
{
    // Whenever I add a 2nd controller, I should swap this to TestDataMethod
    // Might also want to add a shared base class whenever I build out more test classes
    //  Although when I add angular tests, they should be as a 2nd test method
    [TestMethod]
    public async Task TrackedSetsController_IsOnline_Test()
    {
        const string controllerEndpoint = "api/TrackedSets/status";
        var factory = new CarpentryFactory<Startup>();
        var client = factory.CreateClient();
        var response = await client.GetAsync(controllerEndpoint);
        var responseContent = await response.Content.ReadAsStringAsync();
        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        Assert.AreEqual("Online", responseContent);
    }
}