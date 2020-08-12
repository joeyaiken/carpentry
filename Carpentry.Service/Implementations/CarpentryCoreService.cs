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

        public CarpentryCoreService()
        {
            
        }

        public async Task<AppFiltersDto> GetAppFilterValues()
        {
            throw new NotImplementedException();
        }

        public async Task<List<SetDetailDto>> GetTrackedSets(bool showUntracked, bool update)
        {
            throw new NotImplementedException();
        }


        public async Task AddTrackedSet(int setId)
        {
            throw new NotImplementedException();
        }


        public async Task UpdateTrackedSet(int setId)
        {
            throw new NotImplementedException();
        }

        public async Task RemoveTrackedSet(int setId)
        {
            throw new NotImplementedException();
        }

    }
}
