using System.Net;
using Newtonsoft.Json;

namespace Carpentry.UI.Tests.IntegrationTests;

public abstract class CarpentryTestBase
{
    private CarpentryFactory<Startup> _factory = null!;
    private HttpClient _client = null!;
    
    [TestInitialize]
    public void BeforeEach()
    {
        _factory = new CarpentryFactory<Startup>();
        _client = _factory.CreateClient();
    }
    
    [TestCleanup]
    public void AfterEach()
    {
        // Not convinced this step helps with anything but w/e
        _client.Dispose();
        _factory.Dispose();
    }
    
    protected async Task<T?> GetControllerResult<T>(string apiEndpoint)
    {
        var responseContent = await GetControllerStringResult(apiEndpoint);
        if (responseContent == null) return default;
        
        var searchResult = JsonConvert.DeserializeObject<T>(responseContent);
        return searchResult;
    }

    protected async Task<string?> GetControllerStringResult(string apiEndpoint)
    {
        var response = await _client.GetAsync(apiEndpoint);
        if (response.StatusCode != HttpStatusCode.OK) return default;
        
        return await response.Content.ReadAsStringAsync();
    }
    
}