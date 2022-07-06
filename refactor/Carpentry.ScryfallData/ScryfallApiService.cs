using Carpentry.ScryfallData.Models;
using Newtonsoft.Json.Linq;

namespace Carpentry.ScryfallData;

public interface IScryfallApiService
{
    Task<JArray?> GetAvailableSets();

    /// <summary>
    /// Gets a full set of unmapped Card Tokens from Scryfall
    /// </summary>
    /// <param name="setCode"></param>
    /// <returns></returns>
    Task<SetApiResult> GetFullSet(string setCode);
}

public class ScryfallApiService : IScryfallApiService
{
    private readonly HttpClient _client;
    private readonly int _scryfallApiDelay;
    
    public ScryfallApiService(HttpClient client)
    {
        _client = client;
        //TODO - load this from a config
        _scryfallApiDelay = 100; //1000 = 1 second, scryfall requests 50-100
    }
    
    public async Task<JArray?> GetAvailableSets()
    {
        const string endpoint = $"https://api.scryfall.com/sets";
        var responseString = await _client.GetStringAsync(endpoint);

        var setResponseObject = JObject.Parse(responseString);

        var responseData = setResponseObject.Value<JArray>("data");

        return responseData;
    }

    public async Task<SetApiResult> GetFullSet(string setCode)
    {
        var result = new SetApiResult();

        var setEndpoint = $"https://api.scryfall.com/sets/{setCode}";
        var setResponseString = await _client.GetStringAsync(setEndpoint);

        result.SetToken = JToken.Parse(setResponseString);
        
        var searchHasMore = true;

        var cardSearchUri = result.SetToken.Value<string>("search_uri");

        while (searchHasMore)
        {
            await Task.Delay(_scryfallApiDelay);
            var cardSearchResponse = await _client.GetStringAsync(cardSearchUri);
            var cardSearchJObject = JObject.Parse(cardSearchResponse);

            var dataBatch = cardSearchJObject.Value<JArray>("data");

            if (dataBatch != null) result.CardTokens.AddRange(dataBatch);
            searchHasMore = cardSearchJObject.Value<bool>("has_more");
            if (searchHasMore)
            {
                cardSearchUri = cardSearchJObject.Value<string>("next_page");
            }
        }

        return result;
    }

}