using Carpentry.Service.Interfaces;
using Carpentry.Service.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Carpentry.Service.Implementations
{
    public class InventoryService : IInventoryService
    {
        public async Task<int> AddInventoryCard(InventoryCardDto dto)
        {
            throw new NotImplementedException();
        }

        public async Task AddInventoryCardBatch(IEnumerable<InventoryCardDto> cards)
        {
            throw new NotImplementedException();
        }

        public async Task UpdateInventoryCard(InventoryCardDto dto)
        {
            throw new NotImplementedException();
        }

        public async Task DeleteInventoryCard(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<InventoryOverviewDto>> GetInventoryOverviews(InventoryQueryParameter param)
        {
            throw new NotImplementedException();
        }

        public async Task<InventoryDetailDto> GetInventoryDetailByName(string name)
        {
            throw new NotImplementedException();
        }
    }
}
