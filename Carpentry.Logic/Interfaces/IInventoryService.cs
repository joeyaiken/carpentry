using Carpentry.Data.QueryParameters;
using Carpentry.Logic.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Carpentry.Logic.Interfaces
{
    public interface IInventoryService
    {
        Task<int> AddInventoryCard(InventoryCardDto dto);

        Task AddInventoryCardBatch(IEnumerable<InventoryCardDto> cards);

        Task UpdateInventoryCard(InventoryCardDto dto);

        Task DeleteInventoryCard(int id);

        Task<IEnumerable<InventoryOverviewDto>> GetInventoryOverviews(InventoryQueryParameter param);

        Task<InventoryDetailDto> GetInventoryDetailByName(string name);
    }
}
