using Carpentry.Logic.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Carpentry.Logic.Interfaces
{
    public interface IDataUpdateService
    {
        /// <summary>
        /// Updates all pricing and legality data for a card set
        /// </summary>
        /// <returns></returns>
        Task UpdateAllSets();

        /// <summary>
        /// Update the card and scry data for a particular set
        /// </summary>
        /// <param name="setCode"></param>
        /// <returns></returns>
        Task UpdateSetData(string setCode);

        Task EnsureCardDefinitionExists(int multiverseId);

        Task EnsureDatabasesCreated();

        Task EnsureDefaultRecordsExist();

        //
        //
        //

        //public async List<SetDetailDto> GetTrackedSets()
        Task<List<SetDetailDto>> GetTrackedSets();

        //public async void UpdateTrackedSetScryData(string setCode)
        Task UpdateTrackedSetScryData(string setCode);

        //public async void UpdateTrackedSetCardData(string setCode)
        Task UpdateTrackedSetCardData(string setCode);

        //public async List<SetDetailDto> GetAllAvailableSets()
        Task<List<SetDetailDto>> GetAllAvailableSets();

        //public async void AddTrackedSet(string setCode)
        Task AddTrackedSet(string setCode);

        //public async void RemoveTrackedSet(string setCode)
        Task RemoveTrackedSet(string setCode);

    }
}
