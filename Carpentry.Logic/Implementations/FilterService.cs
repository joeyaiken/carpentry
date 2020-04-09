using Carpentry.Logic.Interfaces;
using Carpentry.Logic.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Carpentry.Logic.Implementations
{
    public class FilterService : IFilterService
    {
        public async Task<List<FilterOption>> GetSetFilterOptions()
        {
            await Task.CompletedTask;
            throw new NotImplementedException();
        }
        public async Task<List<FilterOption>> GetTypeFilterOptions()
        {
            await Task.CompletedTask;
            throw new NotImplementedException();
        }
        public async Task<List<FilterOption>> GetFormatFilterOptions()
        {
            await Task.CompletedTask;
            throw new NotImplementedException();
        }
        public async Task<List<FilterOption>> GetManaColorFilterOptions()
        {
            await Task.CompletedTask;
            throw new NotImplementedException();
        }
        public async Task<List<FilterOption>> GetRarityFilterOptions()
        {
            await Task.CompletedTask;
            throw new NotImplementedException();
        }
        public async Task<List<FilterOption>> GetCardStatusFilterOptions()
        {
            await Task.CompletedTask;
            throw new NotImplementedException();
        }
    }
}
