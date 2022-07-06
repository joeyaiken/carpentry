using Carpentry.Logic.Models.Scryfall;
using Microsoft.EntityFrameworkCore;

namespace Carpentry.ScryfallData;

public interface IScryfallDataService
{
    /// <summary>
    /// Get a list of all sets available from Scryfall
    /// Optionally excludes things not actually used by the app
    /// Stores results in a local database to avoid re-querying Scryfall for a given record type more than once a day
    /// </summary>
    /// <returns></returns>
    Task<List<ScryfallSetOverview>> GetSetOverviews(bool filter = true);
    
    /// <summary>
    /// Gets a detail object containing the set overview, and all cards contained in the set
    /// </summary>
    /// <param name="setCode"></param>
    /// <returns></returns>
    Task<ScryfallSetDetail> GetSetDetail(string setCode);
    
    //get set card prices

    Task GetSetCardPrices(string setCode);

}

public class ScryfallDataService : IScryfallDataService
{
    private readonly ScryfallApiService _apiService;
    private readonly ScryfallDataContext _dataContext;

    public ScryfallDataService(ScryfallApiService apiService, ScryfallDataContext dataContext)
    {
        _apiService = apiService;
        _dataContext = dataContext;
    }


    public async Task<List<ScryfallSetOverview>> GetSetOverviews(bool filter = true)
    {
        //check most recent date (if any)

        var cachedDataLastUpdated = await _dataContext.AllSetsCaches.Select(s => s.LastUpdated).FirstOrDefaultAsync();
        
        
        
        
        //decide if new data needs to be queried
        
        //if so: query for that new data
        //  then save
        
        //then, return overview list
        
        throw new NotImplementedException();
    }

    
    // When 'updating' a set, I won't always need to update the full card definitions
    //  I want to avoid a situation where I have a 7gb db file that should only really be 0.3gb
    
    public Task<ScryfallSetDetail> GetSetDetail(string setCode)
    {
        // Price Data is considered stale if...
        //  Prices are >24hrs old
        
        // Card Data is considered stale if...
        //  CardDataLastUpdated < Set Release Date && Card Data > 24hrs old
        
        
        // Get most recent record's date (if any)
        //   If the card data is 'stale', but were queried after the set was released, it can mostly be assumed that the data doesn't need to change
        
        
        //        //  If the record ends up being stale...it will need to be queried eventually so it can be updated \
        //  This assumption is wrong.  If the card data is 'stale'

        // It's recent
        //  Select payload, map card records
        //  Select prices to local model
        //  Return everything
        
        
        // It doesn't exist
        //   Query scryfall for most-recent
        //   Extract card price data from the result
        //   Create new Set and Card Price records, save to DB
        //   Return the records as a SetDetail
        
        // It is stale
        //  Query scryfall
        //  Extract card price data from the result
        //  Update the Set record
        //  Update price records (do I just delete & re-insert?
        
        throw new NotImplementedException();
    }

    public Task GetSetCardPrices(string setCode)
    {
        // Check if the set definition exists
        
        // Check if the prices are 'stale'
        
        // if fresh, select prices and return
        //  (There should never be a situation where this query updates card data)
        
        // if stale, get set definition, extract price records
        //  
        
        
        
        
        
        throw new NotImplementedException();
    }
}