using Carpentry.Data.DataContext;
using Carpentry.Data.DataModels;
using Carpentry.Data.Interfaces;
using Carpentry.Data.QueryResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carpentry.Data.Implementations
{
    public class DataReferenceService : IDataReferenceService
    {
        //readonly ScryfallDataContext _scryContext;
        private readonly CarpentryDataContext _cardContext;
        //private readonly ILogger<DataReferenceService> _logger;

        public DataReferenceService(CarpentryDataContext cardContext
            //, ILogger<DataReferenceService> logger
            )
        {
            _cardContext = cardContext;
            //_logger = logger;
        }

        //public async Task<MagicFormat> GetFormatByName(string formatName)
        //{
        //    var matchingFormat = await _cardContext.MagicFormats.Where(x => x.Name.ToLower() == formatName.ToLower()).FirstOrDefaultAsync();
        //    return matchingFormat;
        //}
        //public async Task<MagicFormat> GetFormatById(int formatId)
        //{
        //    var matchingFormat = await _cardContext.MagicFormats.Where(x => x.Id == formatId).FirstOrDefaultAsync();
        //    return matchingFormat;
        //}

        
    }
}
