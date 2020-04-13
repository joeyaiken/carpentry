using Carpentry.Data.LegacyModels;
using Carpentry.Data.Models;
using System.Linq;
using System.Threading.Tasks;

namespace Carpentry.Data.Interfaces
{
    //TODO - Delete this when no longer referenced
    public interface ICardStringRepo
    {
        Task<IQueryable<ScryfallMagicCard>> QueryCardsBySet(string setCode);

        Task<IQueryable<ScryfallMagicCard>> QueryScryfallByName(string name, bool exclusive);

        Task<ScryfallMagicCard> GetCardById(int multiverseId);

        Task<int> GetCardMultiverseId(string name, string setCode);

        Task<bool> EnsureSetExistsLocally(string setCode);
    }
}
