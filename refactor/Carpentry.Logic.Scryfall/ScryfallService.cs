using Carpentry.Logic.Scryfall.Models;

namespace Carpentry.Logic.Scryfall;

public interface IScryfallService
{
    /// <summary>
    /// Get a list of all sets available from Scryfall
    /// Optionally excludes things not actually used by the app
    /// Stores results in a local database to avoid re-querying Scryfall for a given record type more than once a day
    /// </summary>
    /// <returns></returns>
    Task<List<ScryfallSetOverview>> GetSetOverviews(bool filter = true);
}

public class ScryfallService : IScryfallService
{
    public async Task<List<ScryfallSetOverview>> GetSetOverviews(bool filter = true)
    {
        throw new NotImplementedException();
    }
}