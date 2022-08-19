using Carpentry.Data.Scryfall.Models;
using Carpentry.Logic.Scryfall.Models;
using Carpentry.ScryfallData;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

// using Serilog;

namespace Carpentry.Logic.Scryfall;

/// <summary>
/// Interface for the ScryfallService
/// </summary>
public interface IScryfallService
{
    /// <summary>
    /// Get a list of all sets available from Scryfall
    /// Optionally excludes things not actually used by the app
    /// Stores results in a local database to avoid re-querying Scryfall for a given record type more than once a day
    /// </summary>
    /// <returns></returns>
    Task<List<ScryfallSetOverview>> GetSetOverviews(bool filter = true); //TODO - do I need 'filter' ?
    
    // /// <summary>
    // /// Gets a detail object containing the set overview, and all cards contained in the set
    // /// </summary>
    // /// <param name="setCode"></param>
    // /// <returns></returns>
    // Task<ScryfallSetDetail> GetSetDetail(string setCode);
    //
    // /// <summary>
    // /// Gets just the price data for all cards available in the set.
    // /// It's not always necessary to update all card definition data for a set, sometimes it's sufficient to just update price data
    // /// (saving & updating legalities will be handled in a future implementation) 
    // /// </summary>
    // /// <param name="setCode"></param>
    // /// <returns></returns>
    // Task GetSetCardPrices(string setCode);
}

/// <summary>
/// The ScryfallService acts as a source of scryfall data.
/// Saves results in a local table to avoid making duplicate calls to the Scryfall API.
/// </summary>
public class ScryfallService : IScryfallService
{
    private readonly ScryfallDataContext _scryContext;

    private readonly ScryfallApiService _apiService;
    // private readonly ILogger<ScryfallService> _logger;

    public ScryfallService(
        ScryfallDataContext scryContext,
        ScryfallApiService apiService//,
        // ILogger logger
    )
    {
        _scryContext = scryContext;
        _apiService = apiService;
        // _logger = logger;
    }
    
    
    public async Task<List<ScryfallSetOverview>> GetSetOverviews(bool filter = true)
    {
        // When checking if the audit data is stale, should I grab the entire record or just grab LastUpdated?
        // AKA: Would I rather run 2 db queries or sometimes grab more data than I need?
        // ... could I make that decision IN the query where-clause?
        // (I only want to select the cached data if it ISN'T stale)


        // Assumes data resets at midnight
        //var staleDate = DateTime.Today;
        
        // Assumes data isn't worth re-querying more than once every 24 hrs
        //  regardless of the Scryfall actual update time
        var staleDate = DateTime.Now.AddDays(-1);

        // Only want to actually grab card data when it isn't stale
        // The idea is maybe that's more efficient, only selecting that data when I'm actually going to use it
        var setData = await _scryContext.AllSetsCaches
            .Select(s => new AllSetsCachedData()
            {
                LastUpdated = s.LastUpdated,
                AllSetsCachedDataId = s.AllSetsCachedDataId,
                SetTokensString = s.LastUpdated < staleDate ? null : s.SetTokensString
            })
            .SingleOrDefaultAsync();
        
        JArray? setTokens;
        
        // SetTokensString will be null if the data is stale.  In that situation, get latest from Scryfall
        if (setData?.SetTokensString == null)
        {
            setTokens = await _apiService.GetAvailableSets();
            
            setData ??= new AllSetsCachedData();
            setData.SetTokensString = JsonConvert.SerializeObject(setTokens);
            setData.LastUpdated = DateTime.Now;

            _scryContext.Update(setData);
            await _scryContext.SaveChangesAsync();
        }
        else
        {
            setTokens = JsonConvert.DeserializeObject<JArray>(setData.SetTokensString);
        }
        
        if (setTokens == null)
        {
            // TODO - log an error, this shouldn't happen.  Also consider just throwing an exception
            return new List<ScryfallSetOverview>();
        }
        
        var result = setTokens.Select(token => new ScryfallSetOverview(token)).ToList();
        
        if (!filter) return result;
        
        // Filter some unwanted sets
        string[] excludedSetTypes = {
            
            "token", // I'll need to exclude this if I want to start tracking tokens 
            // "funny", // Need to include this to see mystery booster play-test cards
            // "memorabilia",// If I want to see the c21 display commanders I need to include this
            "promo",
            // "box", // This apparently omits secret lair
        };

        result = result
            .Where(s =>
                s.Digital == false
                &&
                !excludedSetTypes.Contains(s.SetType)
                &&
                !s.Name.Contains("Judge ")
            )
            .ToList();
    
        return result;
    }
}