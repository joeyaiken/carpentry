using Carpentry.Data.DataModels;
using Carpentry.Data.DataModels.QueryResults;
using Carpentry.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Carpentry.Data.Interfaces
{
    /// <summary>
    /// A scryfall repo should be a repository for data retrieved from scryfall
    /// It stores both raw set data, as well as data mapped to relevant classes
    /// </summary>
    public interface IScryfallDataRepo
    {
        Task<DateTime?> GetSetDataLastUpdated(string setCode);
        Task AddOrUpdateSet(ScryfallSetData setData, bool applyData);
        Task<ScryfallSetData> GetSetByCode(string setCode, bool includeData);
        Task<List<ScryfallSetOverview>> GetAvailableSetOverviews(); 
        Task EnsureDatabaseExists();
        Task<ScryfallAuditData> GetAuditData();
        Task SetAuditData();
        Task DeleteSet(int setId);
    }
}
