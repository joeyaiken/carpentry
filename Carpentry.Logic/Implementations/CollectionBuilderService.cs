using Carpentry.Logic.Interfaces;
using Carpentry.Logic.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Carpentry.Logic.Implementations
{
    class CollectionBuilderService : ICollectionBuilderService
    {
        public async Task<List<InventoryOverviewDto>> GetCollectionBuilderSuggestions()
        {
            throw new NotImplementedException();
        }

        public async Task HideCollectionBuilderSuggestion(InventoryOverviewDto dto)
        {
            throw new NotImplementedException();
        }
    }
}
