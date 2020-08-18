using Carpentry.Data.QueryParameters;
using Carpentry.Logic.Models;
using Carpentry.Logic.Search;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Carpentry.Service.Interfaces
{
    public interface ICarpentryCardSearchService
    {
        Task<List<CardSearchResultDto>> SearchInventory(CardSearchQueryParameter filters);

        Task<List<MagicCardDto>> SearchWeb(NameSearchQueryParameter filters);
    }
}
