using Carpentry.Logic.Interfaces;
using Carpentry.Logic.Models;
using Carpentry.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Carpentry.Service.Implementations
{
    public class CarpentryCoreService : ICarpentryCoreService
    {
        private readonly IDataUpdateService _dataUpdateService;
        private readonly IFilterService _filterService;

        public CarpentryCoreService(IDataUpdateService dataUpdateService, IFilterService filterService)
        {
            _dataUpdateService = dataUpdateService;
            _filterService = filterService;
        }

        #region Filter Options

        public async Task<AppFiltersDto> GetAppFilterValues()
        {
            var result = await _filterService.GetAppFilterValues();
            
            return result;
        }

        #endregion

        #region Tracked Sets

        public async Task<List<SetDetailDto>> GetTrackedSets(bool showUntracked, bool update)
        {
            var result = await _dataUpdateService.GetTrackedSets(showUntracked, update);

            return result;
        }

        public async Task AddTrackedSet(int setId)
        {
            await _dataUpdateService.AddTrackedSet(setId);
        }

        public async Task UpdateTrackedSet(int setId)
        {
            await _dataUpdateService.UpdateTrackedSet(setId);
        }

        public async Task RemoveTrackedSet(int setId)
        {
            await _dataUpdateService.RemoveTrackedSet(setId);
        }

        #endregion
    
    }
}
