using Carpentry.Data.DataModels.QueryResults;
using Carpentry.Data.QueryResults;
using Carpentry.Logic.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Carpentry.Logic.Interfaces
{
    public interface IInventoryService
    {
        //Inventory Card add/update/delete
        Task<int> AddInventoryCard(InventoryCardDto dto);
        Task AddInventoryCardBatch(IEnumerable<InventoryCardDto> cards);
        Task UpdateInventoryCard(InventoryCardDto dto);
        Task UpdateInventoryCardBatch(IEnumerable<InventoryCardDto> batch);
        Task DeleteInventoryCard(int id);
        Task DeleteInventoryCardBatch(IEnumerable<int> batch);

        //Search
        //Task<List<InventoryOverviewDto>> GetInventoryOverviews(InventoryQueryParameter param);
        Task<InventoryDetailDto> GetInventoryDetail(int cardId);

        Task<List<TrimmingToolResult>> GetTrimmingToolCards(string setCode, int minCount, string filterBy, string searchGroup = null);
        Task TrimCards(List<TrimmedCardDto> cardsToTrim);

        Task<IEnumerable<InventoryTotalsByStatusResult>> GetCollectionTotals();
    }
}
