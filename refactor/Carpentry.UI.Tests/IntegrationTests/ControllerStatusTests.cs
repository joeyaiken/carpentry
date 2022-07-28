using System.Net;

namespace Carpentry.UI.Tests.IntegrationTests;

/// <summary>
/// This test class simply checks if all of the controllers can be successfully started.
/// It catches DI errors & other things.
/// </summary>
[TestClass]
public class ControllerStatusTests : CarpentryTestBase
{
    [DataRow(TrackedSetsControllerTests.ControllerEndpoint)]
    [DataTestMethod]
    public async Task TrackedSetsController_IsOnline_Test(string apiEndpoint)
    {
        var controllerEndpoint = $"{apiEndpoint}/status";
        var responseContent = await GetControllerStringResult(controllerEndpoint);
        Assert.AreEqual("Online", responseContent);
    }
}