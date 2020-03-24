using System;
using System.Linq;
using Carpentry.Data.DataContext;
using Carpentry.Data.MigrationTool.Models;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;

namespace Carpentry.Data.MigrationTool.Services
{
    public class DataBackupService
    {

        private readonly ILogger<DataBackupService> _logger;
        readonly SqliteDataContext _cardContext;
        readonly MigrationToolConfig _config;

        public DataBackupService(
            ILoggerFactory loggerFactory,
            SqliteDataContext cardContext,
            MigrationToolConfig config
            )
        {
            _logger = loggerFactory.CreateLogger<DataBackupService>();
            _cardContext = cardContext;
            _config = config;
        }

        public void SaveDb()
        {
            _logger.LogInformation("DataBackupService - SaveDb...");
            //query inventory cards
            var cardExports = _cardContext.InventoryCards.Select(x => new BackupInventoryCard
            {
                MultiverseId = x.MultiverseId,
                InventoryCardStatusId = x.InventoryCardStatusId,
                IsFoil = x.IsFoil,
                VariantName = x.VariantType.Name,
                DeckCards = x.DeckCards.Select(c => new BackupDeckCard
                {
                    DeckId = c.DeckId,
                    Category = c.CategoryId,
                }).ToList(),
            }).OrderBy(x => x.MultiverseId).ToList();

            //query decks
            var deckExports = _cardContext.Decks.Select(x => new BackupDeck
            {
                ExportId = x.Id,
                Name = x.Name,
                Format = x.Format.Name,
                Notes = x.Notes,
                BasicW = x.BasicW,
                BasicU = x.BasicU,
                BasicB = x.BasicB,
                BasicR = x.BasicR,
                BasicG = x.BasicG
            }).OrderBy(x => x.ExportId).ToList();

            //query set codes
            //var setCodes = _cardContext.Sets.Select(x => x.Code).OrderBy(x => x).ToList();
            var setCodes = _cardContext.InventoryCards.Select(x => x.Card.Set.Code).Distinct().OrderBy(x => x).ToList();

            BackupDataProps backupProps = new BackupDataProps()
            {
                SetCodes = setCodes,
                TimeStamp = DateTime.Now,
            };

            var deckBackupObj = JArray.FromObject(deckExports);
            var cardBackupObj = JArray.FromObject(cardExports);
            var propsBackupObj = JObject.FromObject(backupProps);

            System.IO.File.WriteAllText(_config.DeckBackupLocation, deckBackupObj.ToString());
            System.IO.File.WriteAllText(_config.CardBackupLocation, cardBackupObj.ToString());
            System.IO.File.WriteAllText(_config.PropsBackupLocation, propsBackupObj.ToString());

            _logger.LogInformation("DataBackupService - SaveDb...completed successfully");
        }

    }
}
