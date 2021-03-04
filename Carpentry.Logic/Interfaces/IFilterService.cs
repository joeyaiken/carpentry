using Carpentry.Logic.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Carpentry.Logic.Interfaces
{
    public interface IFilterService
    {        
        Task<AppFiltersDto> GetAppCoreData();
        Task<List<FilterOption>> GetFormatFilters();
        Task<List<FilterOption>> GetRarityFilters();
        Task<List<FilterOption>> GetCardSetFilters();
        Task<List<FilterOption>> GetStatusFilters();
        List<FilterOption> GetCardTypeFilters();
        List<FilterOption> GetManaTypeFilters();
        List<FilterOption> GetCardGroupFilters();
        List<FilterOption> GetInventoryGroupOptions();
        List<FilterOption> GetInventorySortOptions();
    }
}
