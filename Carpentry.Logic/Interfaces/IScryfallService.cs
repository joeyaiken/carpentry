using Carpentry.Logic.Models.Scryfall;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Carpentry.Logic.Interfaces
{
    /// <summary>
    /// Interface for a class that contains the logic required to query Scryfall for magic card info
    /// Basically just handles requests to the Scryfall API
    /// </summary>
    public interface IScryfallService
    {

        //Task<ScryfallSetDataDto> GetFullSet(string setCode);
        Task<ScryfallSetDataDto> GetFullMappedSet(string setCode);

        List<ScryfallMagicCard> MapScryfallDataToCards(List<JToken> cardSearchData);

        Task<ScryfallMagicCard> GetCardByMid(int multiverseId);

        Task<List<ScryfallMagicCard>> SearchScryfallByName(string name, bool exclusive);

        Task<List<ScryfallSetOverviewDto>> GetAllSets();

        //search by name
        //  GetCardsByName

        //get full set definition // search set
        //  GetCardsBySet
        //  GetScryfallSet

        //Get by MID
        //  GetCardByMid

        //Get by name AND SET (for getting the MID of just a name)
        //  GetCardByNameAndSet
    }
}
