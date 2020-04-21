using Carpentry.Service.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Carpentry.Service.Interfaces
{
    public interface IInventoryControllerService
    {
        Task<int> AddInventoryCard(InventoryCardDto dto);

        Task AddInventoryCardBatch(IEnumerable<InventoryCardDto> cards);

        Task UpdateInventoryCard(InventoryCardDto dto);

        Task DeleteInventoryCard(int id);

        Task<IEnumerable<InventoryOverviewDto>> GetInventoryOverviews(InventoryQueryParameter param);

        Task<InventoryDetailDto> GetInventoryDetailByName(string name);
    }
}
