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
    }
}
