using Carpentry.Logic.Interfaces;
using Carpentry.Logic.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Carpentry.Logic.Implementations
{
    public class CollectionBuilderService : ICollectionBuilderService
    {
        public async Task<List<InventoryOverviewDto>> GetCollectionBuilderSuggestions()
        {
            await Task.CompletedTask;
            throw new NotImplementedException();
        }

        public async Task HideCollectionBuilderSuggestion(InventoryOverviewDto dto)
        {
            await Task.CompletedTask;
            throw new NotImplementedException();
        }
    }
}
