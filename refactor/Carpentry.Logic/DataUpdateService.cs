using Carpentry.CarpentryData.Models;
using Carpentry.Data;
using Carpentry.Logic.Scryfall;
using Microsoft.EntityFrameworkCore;

namespace Carpentry.Logic;

/// <summary>
/// Represents the interface for the Data Update Service
/// </summary>
public interface IDataUpdateService
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="showUntracked"></param>
    /// <returns></returns>
    Task<List<TrackedSetDetail>> GetTrackedSets(bool showUntracked);
    
    /// <summary>
    /// Starts tracking the card definitions for a specific set.
    /// </summary>
    /// <param name="setId"></param>
    /// <returns></returns>
    Task AddTrackedSet(int setId);
    
    /// <summary>
    /// Updates the prices of all cards in the set, and updates definitions as necessary 
    /// </summary>
    /// <param name="setId"></param>
    /// <returns></returns>
    Task UpdateTrackedSet(int setId);
    
    /// <summary>
    /// Starts tracking the card definitions for a specific set, removing local definitions.
    /// Will fail if any inventory cards exist that reference cards in this set.
    /// </summary>
    /// <param name="setId"></param>
    /// <returns></returns>
    Task RemoveTrackedSet(int setId);
}

// TODO - decide if this should live in its own file or not
public class TrackedSetDetail
{
    public int SetId { get; set; }
    
    public string Code { get; set; }
    
    public string Name { get; set; }
    
    // public DateTime ReleaseDate { get; set; }
    
    public DateTime? DataLastUpdated { get; set; }
         
    // //Owned cards / card count / tracked count
    public bool IsTracked { get; set; }
    // public int? InventoryCount { get; set; }
    public int? CollectedCount { get; set; }
    // public int? TotalCount { get; set; }
}

/// <summary>
/// The Data Update Service is responsible for maintaining card definition data
/// This includes general card properties, as well as card prices
/// </summary>
public class DataUpdateService : IDataUpdateService
{
    private readonly CarpentryDataContext _carpentryContext;
    private readonly IScryfallService _scryfallService;

    public DataUpdateService(
        CarpentryDataContext carpentryContext,
        IScryfallService scryfallService
    )
    {
        _carpentryContext = carpentryContext;
        _scryfallService = scryfallService;
    }
    
    public async Task<List<TrackedSetDetail>> GetTrackedSets(bool showUntracked)
    {
        // Figure out how old the list of tracked sets is
        var availableSetsLastUpdated = await _carpentryContext.CoreDefinitionUpdateHistory
            .Where(d => d.Type == "AvailableSets") //TODO - no magic strings
            .OrderByDescending(d => d.UpdatedDate)
            .Select(d => d.UpdatedDate)
            .FirstOrDefaultAsync();

        // Update the list of available sets if data is > 24hrs old
        if (DateIsStale(availableSetsLastUpdated))
            await UpdateAvailableSets();
        
        // Get list of available sets, map, and return
        var result = _carpentryContext.
        
        
        throw new NotImplementedException();
    }

    public Task AddTrackedSet(int setId)
    {
        throw new NotImplementedException();
    }

    public Task UpdateTrackedSet(int setId)
    {
        
        // Only update card data if Last Updated < Release Date
        
        // Update all price data
        
        throw new NotImplementedException();
    }

    public Task RemoveTrackedSet(int setId)
    {
        // Check if any inventory cards reference any of these tracked cards
        
        // If none, remove card definitions, and mark set as untracked
        
        throw new NotImplementedException();
    }

    private static bool DateIsStale(DateTime? date)
    {
        if (date == null) return true;
        //How do I determine if a date is 'stale'?
        //  Old approach: if it was from the previous calendar day
        return DateTime.Now.Date > date.Value.Date;

        //  Possible approach: if it's < 24hrs old
        // return (DateTime.Now - date).Hours > 24;
    }

    private static bool DateIsVeryStale(DateTime date)
    {
        // A 'very stale' date is something over 1-2 months old
        // The idea is that I should really be updating card definition data on SOME regular interval
        return false;
    }

    private async Task UpdateAvailableSets()
    {
        // Get list of all sets from ScryService
        var scrySets = await _scryfallService.GetSetOverviews();

        // get list of db sets (=> map to dictionary)
        var existingCardSetsDict =
            (await _carpentryContext.Sets.ToListAsync()).ToDictionary(s => s.Code.ToLower(), s => s);

        foreach (var scrySet in scrySets)
        {
            // For each result, see if it exists in the DB
            if (!existingCardSetsDict.TryGetValue(scrySet.Code.ToLower(), out var existingSet))
            {
                // If it doesn't, create a new one that can be added to the db
                existingSet = new CardSetData()
                {
                    Code = scrySet.Code,
                    IsTracked = false,
                };
            }
            
            if (existingSet.Name != scrySet.Name) existingSet.Name = scrySet.Name;
            if (existingSet.ReleaseDate != scrySet.ReleasedAt) existingSet.ReleaseDate = scrySet.ReleasedAt;

            // Update will still add new if is untracked
            _carpentryContext.Sets.Update(existingSet);
            await _carpentryContext.SaveChangesAsync();
        }
    }
}