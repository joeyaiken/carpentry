using Carpentry.Logic.Models;
using Carpentry.Logic.Models.Scryfall;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Carpentry.Logic.Interfaces
{
    /// <summary>
    /// Provides a list of available sets from Scryfall, and all cards of a given set
    /// </summary>
    public interface IScryfallService
    {
        /// <summary>
        /// Gets a list of avaliable card sets
        /// </summary>
        /// <returns></returns>
        Task<List<ScryfallSetOverviewDto>> GetAvailableSets(bool filter = true);
        
        /// <summary>
        /// Gets all cards in a specific set
        /// Gets latest data from scryfall if necessary
        /// </summary>
        /// <param name="setCode"></param>
        /// <returns></returns>
        Task<ScryfallSetDataDto> GetSetCards(string setCode);
        
        /// <summary>
        /// Takes a list of set codes and ensures the sets are updated locally
        /// </summary>
        /// <returns></returns>
        Task EnsureSetsUpdated(List<string> setCodes);
        //Task EndureSets

        //Use cases are:
        //  Getting list of available sets (from app settings, or maybe from import)
        //  Getting all cards in a specific set (calling scryfall if necessary)
        //  Ensuring a list of sets are up to date: EnsureSetsUpdated | EnsureSets[ Updated | Cached | Stored | UpToDate ]



        //"ensure set is up to date" (string setCode);
        //"Ensure list of sets are up to date"
    }
}
