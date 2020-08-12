using Carpentry.Logic.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Carpentry.Service.Interfaces
{
    public interface ICarpentryCoreService
    {
        Task<AppFiltersDto> GetAppFilterValues();

        Task<List<SetDetailDto>> GetTrackedSets(bool showUntracked, bool update);
        Task AddTrackedSet(int setId);
        Task UpdateTrackedSet(int setId);
        Task RemoveTrackedSet(int setId);
    }
}
