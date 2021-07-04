using Carpentry.ScryfallData.Models;
using Carpentry.ScryfallData.Models.QueryResults;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Carpentry.DataLogic.Interfaces
{
    /// <summary>
    /// A scryfall repo should be a repository for data retrieved from scryfall
    /// It stores both raw set data, as well as data mapped to relevant classes
    /// </summary>
    [Obsolete]
    public interface IScryfallDataRepo
    {
        [Obsolete]
        Task<DateTime?> GetSetDataLastUpdated(string setCode);
        [Obsolete]
        Task AddOrUpdateSet(ScryfallSetData setData, bool applyData);
        [Obsolete]
        Task<ScryfallSetData> GetSetByCode(string setCode, bool includeData);
        [Obsolete]
        Task<List<ScryfallSetOverview>> GetAvailableSetOverviews();
        [Obsolete]
        Task EnsureDatabaseExists();
        [Obsolete]
        Task<ScryfallAuditData> GetAuditData();
        [Obsolete]
        Task SetAuditData();
        //[Obsolete]
        //Task DeleteSet(int setId);
    }
}
