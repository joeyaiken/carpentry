using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Carpentry.ScryfallData;
using Carpentry.ScryfallData.Models;
using Carpentry.ScryfallData.Models.QueryResults;

namespace Carpentry.DataLogic
{
    /// <summary>
    /// A scryfall repo should be a repository for data retrieved from scryfall
    /// It stores both raw set data, as well as data mapped to relevant classes
    /// </summary>
    [Obsolete]
    public interface IScryfallDataRepo
    {
        [Obsolete]
        Task EnsureDatabaseExists();   
    }


    [Obsolete]
    public class ScryfallRepo : IScryfallDataRepo
    {
        private readonly ScryfallDataContext _scryContext;

        public ScryfallRepo(
            ScryfallDataContext scryContext
            )
        {
            _scryContext = scryContext;
        }

        public async Task EnsureDatabaseExists()
        {
            await _scryContext.Database.EnsureCreatedAsync();
        }
    }
}
