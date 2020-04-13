using Carpentry.Data.DataModels;
using Carpentry.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Carpentry.Data.Interfaces
{
    /// <summary>
    /// A scryfall repo should be a repository for data retrieved from scryfall
    /// It doesn't handle any manipulation of the data, just simple storage
    /// </summary>
    public interface IScryfallDataRepo
    {
        Task<DateTime?> GetSetDataLastUpdated(string setCode);

        Task AddOrUpdateSet(ScryfallSet setData);

        Task<ScryfallSet> GetSetByCode(string setCode);

        //Task DeleteSet(int setId);
    }
}
