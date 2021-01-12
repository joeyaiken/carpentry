using Carpentry.Logic.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Carpentry.Logic.Interfaces
{
    public interface IDataUpdateService
    {
        Task ValidateDatabase();
        Task EnsureDatabasesCreated(); //Currently only used by QuickRestore, I don't know where these would otherwise get called (validateDB)
        Task EnsureDefaultRecordsExist();

        Task<List<SetDetailDto>> GetTrackedSets(bool showUntracked, bool update);
        Task AddTrackedSet(int setId);
        Task RemoveTrackedSet(int setId);
        Task UpdateTrackedSet(int setId);

        Task TryUpdateAvailableSets();

    }
}
