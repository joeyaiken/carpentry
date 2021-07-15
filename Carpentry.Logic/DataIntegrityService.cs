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
            await _cardContext.EnsureDatabaseCreated();
            await _scryContext.Database.EnsureCreatedAsync();
        }
    }
}
