using Carpentry.Data.QueryParameters;
using Carpentry.Logic.Models;
using Carpentry.Logic.Search;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Carpentry.Logic.Interfaces
{
    public interface ISearchService
    {
        Task<List<InventoryOverviewDto>> SearchInventoryCards(InventoryQueryParameter param);
        Task<List<CardSearchResultDto>> SearchCardDefinitions(CardSearchQueryParameter filters);
    }
}
