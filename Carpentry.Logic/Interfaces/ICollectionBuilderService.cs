using Carpentry.Logic.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Carpentry.Logic.Interfaces
{
    public interface ICollectionBuilderService
    {
        Task<List<InventoryOverviewDto>> GetCollectionBuilderSuggestions();
        Task HideCollectionBuilderSuggestion(InventoryOverviewDto dto);
    }
}
