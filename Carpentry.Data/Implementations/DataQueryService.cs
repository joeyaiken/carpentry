using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Carpentry.Data.DataModels;
using Carpentry.Data.Interfaces;
using Carpentry.Data.QueryParameters;

namespace Carpentry.Data.Implementations
{
    /// <summary>
    /// This class will preform all of the overview/detail queries on the DB
    /// It will NOT return DataModel classes
    /// It will return QueryResults classes
    /// It will accept single parameters, either default types, or something in the QueryParameters folder
    /// </summary>

    public class DataQueryService : IDataQueryService
    {
        
    }
}
