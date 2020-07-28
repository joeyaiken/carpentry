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
        Task<List<SetDetailDto>> GetTrackedSets(bool showUntracked, bool update);

        Task AddTrackedSet(int setId);

        Task AddTrackedSet(string setCode);

        Task RemoveTrackedSet(int setId);
        
        Task UpdateTrackedSet(int setId);



    }
}
