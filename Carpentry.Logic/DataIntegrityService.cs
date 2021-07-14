using Carpentry.CarpentryData;
using Carpentry.ScryfallData;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Carpentry.Logic
{
    public interface IDataIntegrityService
    {
        Task EnsureDatabasesCreated();
    }

    public class DataIntegrityService : IDataIntegrityService
    {
        private readonly CarpentryDataContext _cardContext;
        private readonly ScryfallDataContext _scryContext;
        private readonly ILogger<DataIntegrityService> _logger;

        public DataIntegrityService(CarpentryDataContext cardContext, ScryfallDataContext scryContext, ILogger<DataIntegrityService> logger)
        {
            _cardContext = cardContext;
            _scryContext = scryContext;
            _logger = logger;
        }

        public async Task EnsureDatabasesCreated()
        {
            await _cardContext.Database.EnsureCreatedAsync();
            await _scryContext.Database.EnsureCreatedAsync();

            await ExecuteSqlScript("vwAllInventoryCards");
            await ExecuteSqlScript("vwCardTotals");
            await ExecuteSqlScript("vwInventoryCardsByName");
            await ExecuteSqlScript("vwInventoryCardsByPrint");
            await ExecuteSqlScript("vwInventoryCardsByUnique");
            await ExecuteSqlScript("vwInventoryTotalsByStatus");
            await ExecuteSqlScript("vwSetTotals");
            await ExecuteSqlScript("spGetInventoryTotals");
            //await ExecuteSqlScript("spGetTotalTrimCount");
            //await ExecuteSqlScript("spGetTrimmingTips");
        }

        private async Task ExecuteSqlScript(string scriptName)
        {
            try
            {
                await _cardContext.ExecuteSqlScript(scriptName);
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Error attempting to add DB object {scriptName}", ex);
            }
        }
    }
}
