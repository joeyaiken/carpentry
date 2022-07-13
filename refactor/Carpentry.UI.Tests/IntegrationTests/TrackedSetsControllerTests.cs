using System.Net;
using Carpentry.UI.Models;
using Newtonsoft.Json;

namespace Carpentry.UI.Tests.IntegrationTests;

/// <summary>
/// Integration test class that calls various methods of the Tracked Sets controller
/// Simple series of tests used to develop against local data
/// </summary>
[TestClass]
public class TrackedSetsControllerTests
{
    // [TestMethod]
    // public async Task TrackedSetsController_IsOnline_Test(string controllerEndpoint = "api/trackedSets/status")
    // {
    //     var factory = new CarpentryFactory<Startup>();
    //     var client = factory.CreateClient();
    //     var response = await client.GetAsync(controllerEndpoint);
    //     var responseContent = await response.Content.ReadAsStringAsync();
    //     Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
    //     Assert.AreEqual("Online", responseContent);
    // }
    
    [TestMethod]
    public async Task GetTrackedSets_Query_Works()
    {
        const bool showUntracked = false;
        
        // var factory = new CarpentryFactory<Startup>();
        // var client = factory.CreateClient();
        //
        // var response = await client.GetAsync($"api/core/GetTrackedSets?showUntracked={showUntracked}");
        // var responseContent = await response.Content.ReadAsStringAsync();
        // var searchResult = JsonConvert.DeserializeObject<NormalizedList<TrackedSetDto>>(responseContent);

        var endpoint = $"api/TrackedSets?showUntracked={showUntracked}";

        var searchResult = await GetResult<NormalizedList<TrackedSetDto>>(endpoint);
        
        Assert.IsNotNull(searchResult);
    }




    private static async Task<T?> GetResult<T>(string apiEndpoint)
    {
        var factory = new CarpentryFactory<Startup>();
        var client = factory.CreateClient();
        var response = await client.GetAsync(apiEndpoint);

        if (response.StatusCode != HttpStatusCode.OK) return default;
        
        var responseContent = await response.Content.ReadAsStringAsync();
        var searchResult = JsonConvert.DeserializeObject<T>(responseContent);
        return searchResult;
    }
    
}