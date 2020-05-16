using Carpentry.Data.LegacyDataContext;
using Carpentry.Logic.Interfaces;
using Carpentry.Logic.Models;
using Carpentry.Logic.Models.Backups;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Carpentry.Logic.Implementations
{
    /// <summary>
    /// This class contains the logic for saving relevant DB contents to a text file
    /// Save location is pulled from the IDataBackupConfig
    /// </summary>
    public class DataBackupService : IDataBackupService
    {
        private readonly ILogger<DataBackupService> _logger;
        private readonly SqliteDataContext _cardContext;
        private readonly IDataBackupConfig _config;
        
        public DataBackupService(
            ILogger<DataBackupService> logger,
            SqliteDataContext cardContext,
            IDataBackupConfig config
            )
        {
            _logger = logger;
            _cardContext = cardContext;
            _config = config;
        }

        public async Task<BackupDetailDto> GetBackupDetail(string directory)
        {
            throw new NotImplementedException();
        }
        public async Task BackupDatabase(string directory)
        {
            _logger.LogInformation("DataBackupService - SaveDb...");

            string backupDirectory = directory != null ? directory : _config.BackupDirectory;
            
            string deckBackupLocation = $"{backupDirectory}{_config.DeckBackupFilename}";
            string cardBackupLocation = $"{backupDirectory}{_config.CardBackupFilename}";
            string propsBackupLocation = $"{backupDirectory}{_config.PropsBackupFilename}";

            await BackupDatabaseToTextFiles(deckBackupLocation, cardBackupLocation, propsBackupLocation);

            _logger.LogInformation("DataBackupService - SaveDb...completed successfully");
        }

        //TODO - This should take a sinle folder directory, where all 3 files will be dropped.  Not [this weird design where they COULD be in different folders]
        private async Task BackupDatabaseToTextFiles(string deckBackupFilepath, string cardBackupFilepath, string propsBackupFilepath)
        {
            List<BackupInventoryCard> cardExports;
            List<BackupDeck> deckExports;
            BackupDataProps backupProps;

            //try
            //{
            //query inventory cards
            cardExports = await _cardContext.InventoryCards.Select(x => new BackupInventoryCard
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
            }).OrderBy(x => x.MultiverseId).ToListAsync();

            //query decks
            deckExports = await _cardContext.Decks.Select(x => new BackupDeck
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
            }).OrderBy(x => x.ExportId).ToListAsync();

            //query set codes
            //var setCodes = _cardContext.Sets.Select(x => x.Code).OrderBy(x => x).ToList();
            var setCodes = await _cardContext.InventoryCards.Select(x => x.Card.Set.Code).Distinct().OrderBy(x => x).ToListAsync();

            backupProps = new BackupDataProps()
            {
                SetCodes = setCodes,
                TimeStamp = DateTime.Now,
            };

            //catch// (Exception ex)
            //{
            //    _logger.LogWarning("DataBackupService - SaveDb - Exception encountered generating backups, skipping this step");
            //    return;
            //}

            var deckBackupObj = JArray.FromObject(deckExports);
            var cardBackupObj = JArray.FromObject(cardExports);
            var propsBackupObj = JObject.FromObject(backupProps);

            await System.IO.File.WriteAllTextAsync(deckBackupFilepath, deckBackupObj.ToString());
            await System.IO.File.WriteAllTextAsync(cardBackupFilepath, cardBackupObj.ToString());
            await System.IO.File.WriteAllTextAsync(propsBackupFilepath, propsBackupObj.ToString());

            
        }
    }
}
