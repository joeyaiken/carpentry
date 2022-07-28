using System.Net;
using Carpentry.UI.Models;
using Newtonsoft.Json;

namespace Carpentry.UI.Tests.IntegrationTests;

/// <summary>
/// Integration test class that calls various methods of the Tracked Sets controller
/// Simple series of tests used to develop against local data
/// </summary>
[TestClass]
public class TrackedSetsControllerTests : CarpentryTestBase
{

    public const string ControllerEndpoint = "api/TrackedSets";

    // Just going to assume this can be disposed of as soon as we create the client
    private CarpentryFactory<Startup> _factory = null!;
    private HttpClient _client = null!;
    
    // [TestInitialize]
    // public void BeforeEach()
    // {
    //     _factory = new CarpentryFactory<Startup>();
    //     _client = _factory.CreateClient();
    // }

    // [TestCleanup]
    //  public void AfterEach()
    //  {
    //      // Not convinced this step helps with anything but w/e
    //      _client.Dispose();
    //      _factory.Dispose();
    //  }
    
    [TestMethod]
    public async Task GetTrackedSets_Query_Works()
    {
        const bool showUntracked = false;
        var endpoint = $"{ControllerEndpoint}?showUntracked={showUntracked}";
        var searchResult = await GetControllerResult<NormalizedList<TrackedSetDto>>(endpoint);
        Assert.IsNotNull(searchResult);
    }

    // private async Task<T?> GetControllerResult<T>(string apiEndpoint)
    // {
    //     var response = await _client.GetAsync(apiEndpoint);
    //     if (response.StatusCode != HttpStatusCode.OK) return default;
    //     var responseContent = await response.Content.ReadAsStringAsync();
    //     var searchResult = JsonConvert.DeserializeObject<T>(responseContent);
    //     return searchResult;
    // }
    
}