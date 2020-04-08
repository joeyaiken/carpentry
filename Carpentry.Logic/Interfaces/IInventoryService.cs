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
        Task<int> AddInventoryCard(InventoryCard dto);

        Task AddInventoryCardBatch(IEnumerable<InventoryCard> cards);

        Task UpdateInventoryCard(InventoryCard dto);

        Task DeleteInventoryCard(int id);

        Task<IEnumerable<InventoryOverview>> GetInventoryOverviews(InventoryQueryParameter param);

        Task<InventoryDetail> GetInventoryDetailByName(string name);
    }
}
