using Carpentry.Logic.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Carpentry.Logic.Interfaces
{
    public interface IFilterService
    {
        Task<List<FilterOption>> GetSetFilterOptions();
        Task<List<FilterOption>> GetTypeFilterOptions();
        Task<List<FilterOption>> GetFormatFilterOptions();
        Task<List<FilterOption>> GetManaColorFilterOptions();
        Task<List<FilterOption>> GetRarityFilterOptions();
        Task<List<FilterOption>> GetCardStatusFilterOptions();
    }
}
