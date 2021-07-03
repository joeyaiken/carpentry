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
        Task<List<ScryfallSetOverviewDto>> GetAvailableSets();
        Task<ScryfallSetDataDto> GetSetCards(string setCode);
        
        //"ensure set is up to date" (string setCode);
        //"Ensure list of sets are up to date"
    }
}
