using Carpentry.Logic.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Carpentry.Data.DataContext;
using Carpentry.Logic.Models.Backups;
using Newtonsoft.Json.Linq;
using Carpentry.Data.DataModels;
using System.Linq;
using Carpentry.Data.Interfaces;
using Carpentry.Data.QueryResults;

namespace Carpentry.Logic.Implementations
{
    public class DataRestoreService : IDataRestoreService
    {
        private readonly ILogger<DataRestoreService> _logger;
        private readonly IDataUpdateService _dataUpdateService;
        private readonly IDataBackupConfig _config;
        private readonly IDataReferenceService _dataReferenceService;
        private readonly ICardDataRepo _cardDataRepo;
        private readonly IDeckDataRepo _deckDataRepo;
        private readonly IInventoryDataRepo _inventoryDataRepo;

        public DataRestoreService(
            ILogger<DataRestoreService> logger,
            IDataUpdateService dataUpdateService,
            IDataReferenceService dataReferenceService,
            IDataBackupConfig config,
            ICardDataRepo cardDataRepo, 
            IDeckDataRepo deckDataRepo,
            IInventoryDataRepo inventoryDataRepo
            )
        {
            _logger = logger;
            _dataUpdateService = dataUpdateService;
            _dataReferenceService = dataReferenceService;
            _config = config;
            _cardDataRepo = cardDataRepo;
            _deckDataRepo = deckDataRepo;
            _inventoryDataRepo = inventoryDataRepo;
        }

        public async Task RestoreDatabase()
        {
            _logger.LogInformation("DataRestoreService - RestoreDb...");

            //Stary by ensuring a DB actually exists
            await _dataUpdateService.EnsureDatabasesCreated();

            ////ensure default records exist
            //await _dataUpdateService.EnsureDefaultRecordsExist();

            ////load all decks
            //await LoadDeckBackups();

            ////try to add sets, adding only dummy info
            //await LoadSetTempData();

            ////run "refresh DB"
            //await _dataUpdateService.UpdateAllSets();

            ////add inventory & deck cards
            //await LoadCardBackups();

            _logger.LogWarning("DataRestoreService - RestoreDb...Completed!");
        }

        public async Task LoadDeckBackups()
        {
            //int existingDeckCount = _cardContext.Decks.Select(x => x.Id).Count();
            int existingDeckCount = (await _deckDataRepo.GetAllDecks()).Count();
            if (existingDeckCount > 0)
            {
                _logger.LogWarning("LoadDeckBackups - Decks already exist, not loading anything from parsed data");
                return;
            }

            _logger.LogWarning("LoadDeckBackups - Loading parsed decks");

            string deckBackupLocation = $"{_config.BackupDirectory}{_config.DeckBackupFilename}";

            string deckBackupsDataString = await System.IO.File.ReadAllTextAsync(deckBackupLocation);
            List<BackupDeck> parsedBackupDecks = JArray.Parse(deckBackupsDataString).ToObject<List<BackupDeck>>();

            List<DeckData> newDecks = parsedBackupDecks.Select(x => new DeckData()
            {
                BasicB = x.BasicB,
                BasicG = x.BasicG,
                BasicR = x.BasicR,
                BasicU = x.BasicU,
                BasicW = x.BasicW,

                Name = x.Name,
                Notes = x.Notes,

                MagicFormatId = _dataReferenceService.GetMagicFormat(x.Format).Result.Id,

                Id = x.ExportId
            }).ToList();

            await _deckDataRepo.AddImportedDeckBatch(newDecks);

            //List<Task<int>> newDeckTasks = newDecks.Select(deck => _deckDataRepo.AddDeck(deck)).ToList();

            //await Task.WhenAll(newDeckTasks);

            _logger.LogWarning("LoadDeckBackups - Complete");
        }

        public async Task LoadSetTempData()
        {
            //set-data is considered the source-of-truth for what's up to date
            //Inserting partial records into the set table, so we can then update the partial sets with a different service

            int setCount = (await _cardDataRepo.GetAllCardSetCodes()).Count();

            //get count from repo
            //if no count, add to repo


            if (setCount > 0)
            {
                _logger.LogWarning("LoadSetTempData - set data already exists, not adding temp records");
                return;
            }

            _logger.LogWarning("LoadSetTempData - Adding set placeholder records...");

            string propsBackupLocation = $"{_config.BackupDirectory}{_config.PropsBackupFilename}";

            string propsBackupDataString = await System.IO.File.ReadAllTextAsync(propsBackupLocation);
            BackupDataProps parsedPropsBackups = JObject.Parse(propsBackupDataString).ToObject<BackupDataProps>();

            var newSets = parsedPropsBackups.SetCodes.Select(code => new CardSetData()
            {
                Code = code,
            }).ToList();

            for(int i = 0; i < newSets.Count(); i++)
            {
                await _cardDataRepo.AddOrUpdateCardSet(newSets[i]);
            }

            //var newSetRequests = parsedPropsBackups.SetCodes
            //    .Select(code => new CardSetData()
            //    {
            //        Code = code
            //    })
            //    .Select(tempSet => _cardDataRepo.AddOrUpdateCardSet(tempSet))
            //    .ToList();

            //await Task.WhenAll(newSetRequests);

            _logger.LogWarning("RestoreDb - LoadSetTempData...completed");
        }

        public async Task LoadCardBackups()
        {
            //I don't need to check if card definitions exist anymore
            //If I own a card from a set, all definitions for that set should exist in the DB at this point
            //I can safely grab any backup card by MID

            bool cardsExist = await _inventoryDataRepo.DoInventoryCardsExist();
            if (cardsExist)
            {
                _logger.LogWarning("LoadCardBackups - card data already exists, returning");
                return;
            }
            _logger.LogWarning("LoadCardBackups - preparing to load backups...");

            string cardBackupLocation = $"{_config.BackupDirectory}{_config.CardBackupFilename}";

            string cardBackupsDataString = await System.IO.File.ReadAllTextAsync(cardBackupLocation);
            List<BackupInventoryCard> parseCardsBackups = JArray.Parse(cardBackupsDataString).ToObject<List<BackupInventoryCard>>();

            _logger.LogWarning("RestoreDb - LoadCardBackups...definitions exist, mapping & saving");

            List<DataReferenceValue<int>> allVariants = await _dataReferenceService.GetAllCardVariantTypes();

            var mappedInventoryCards = parseCardsBackups.Select(x => new InventoryCardData
            {
                InventoryCardStatusId = x.InventoryCardStatusId,
                IsFoil = x.IsFoil,
                MultiverseId = x.MultiverseId,
                VariantTypeId = allVariants.FirstOrDefault(v => v.Name == x.VariantName).Id,
                DeckCards = 
                    x.DeckCards.Select(d => new DeckCardData()
                    {
                        DeckId = d.DeckId,
                        CategoryId = d.Category,
                    }).ToList(),
            }).ToList();

            await _inventoryDataRepo.AddInventoryCardBatch(mappedInventoryCards);

            _logger.LogWarning("RestoreDb - LoadCardBackups...COMPLETE!");
        }

    }
}
