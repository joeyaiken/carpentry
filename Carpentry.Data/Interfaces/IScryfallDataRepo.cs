using Carpentry.Data.DataModels;
using Carpentry.Data.DataModels.QueryResults;
using Carpentry.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Carpentry.Data.Interfaces
{
    //TODO - Ensure methods of this interface don't return anything belonging to the Carpentry.DataModels namespace


    /// <summary>
    /// A scryfall repo should be a repository for data retrieved from scryfall
    /// It doesn't handle any manipulation of the data, just simple storage
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

        //Task DeleteSet(int setId);
    }
}
