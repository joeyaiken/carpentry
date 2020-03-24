using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CarpentryMigrationTool
{
    public interface IDataMigrationService
    {

        void MigrateScryfallData();

        void EnsureS9DbExists();

        void EnsureDefaultRecordsExist();

        void ClearDb();

        void MigrateSets();

        void MigrateCards();

        void MigrateDecks();

        void MigrateInventory(); // and deck cards

        void SaveDb();

        Task LoadDb();

        Task TryUpdateScryfallSet(string setCode);

        Task ForceUpdateScryfallSet(string setCode);

        Task UpdateScryfallData();

    }
}