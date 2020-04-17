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

namespace Carpentry.Logic.Implementations
{
    public class DataRestoreService : IDataRestoreService
    {
        private readonly ILogger<DataRestoreService> _logger;

        //private readonly CarpentryDataContext _cardContext;
        //private readonly ScryfallDataContext _scryContext;

        //private readonly MigrationToolConfig _config;

        //private readonly LegacyScryfallRepo _scryRepo;

        //private readonly SqliteCardRepo _cardRepo;

        //private readonly CarpentryService _carpentryService;

        private readonly IDataUpdateService _dataUpdateService;

        private readonly IDataBackupConfig _config;

        //
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
            //SqliteDataContext cardContext,
            //MigrationToolConfig config,
            //CarpentryDataContext cardContext,
            //ScryfallDataContext scryContext
            )
        {
            _logger = logger;
            _dataUpdateService = dataUpdateService;
            _dataReferenceService = dataReferenceService;
            _config = config;
            _cardDataRepo = cardDataRepo;
            _deckDataRepo = deckDataRepo;
            _inventoryDataRepo = inventoryDataRepo;
            //_cardContext = cardContext;
            //_scryContext = scryContext;
            //_config = config;
            //_scryRepo = scryRepo;
            //_cardRepo = cardRepo;
        }

        public async Task RestoreDatabase()
        {
            _logger.LogInformation("DataRestoreService - RestoreDb...");

            //Stary by ensuring a DB actually exists
            await _dataUpdateService.EnsureDatabasesCreated();

            //ensure default records exist
            await _dataUpdateService.EnsureDefaultRecordsExist();

            //load all decks
            await LoadDeckBackups();

            //try to add sets, adding only dummy info
            await LoadSetTempData();

            //run "refresh DB"
            await _dataUpdateService.UpdateAllSets();

            //add inventory & deck cards
            await LoadCardBackups();

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

            string deckBackupsDataString = await System.IO.File.ReadAllTextAsync(_config.DeckBackupLocation);
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

                //Format = _cardContext.MagicFormats.Where(f => f.Name.ToLower() == x.Format.ToLower()).FirstOrDefault(),
                Format = _dataReferenceService.GetMagicFormat(x.Format).Result,

                Id = x.ExportId
            }).ToList();


            List<Task<int>> newDeckTasks = newDecks.Select(deck => _deckDataRepo.AddDeck(deck)).ToList();

            await Task.WhenAll(newDeckTasks);

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

            string propsBackupDataString = await System.IO.File.ReadAllTextAsync(_config.PropsBackupLocation);
            BackupDataProps parsedPropsBackups = JObject.Parse(propsBackupDataString).ToObject<BackupDataProps>();




            var newSetRequests = parsedPropsBackups.SetCodes
                .Select(code => new CardSetData()
                {
                    Code = code
                })
                .Select(tempSet => _cardDataRepo.AddOrUpdateCardSet(tempSet))
                .ToList();

            await Task.WhenAll(newSetRequests);

            _logger.LogWarning("RestoreDb - LoadSetTempData...completed");
        }

        public async Task LoadCardBackups()
        {
            //I don't need to check if card definitions exist anymore
            //If I own a card from a set, all definitions for that set should exist in the DB at this point
            //I can safely grab any backup card by MID


            //int cardCount = await _cardContext.InventoryCards.Select(x => x.Id).CountAsync();
            bool cardsExist = await _inventoryDataRepo.DoInventoryCardsExist();
            if (cardsExist)
            {
                _logger.LogWarning("LoadCardBackups - card data already exists, returning");
                return;
            }
            _logger.LogWarning("LoadCardBackups - preparing to load backups...");

            string cardBackupsDataString = await System.IO.File.ReadAllTextAsync(_config.CardBackupLocation);
            List<BackupInventoryCard> parseCardsBackups = JArray.Parse(cardBackupsDataString).ToObject<List<BackupInventoryCard>>();

            //_logger.LogWarning("Load Card Backups - ensuring definitions exist (this could take some time)");

            //var uniqueMIDs = parseCardsBackups
            //    .Select(x => x.MultiverseId)
            //    .Distinct().ToList();

            //int idCount = uniqueMIDs.Count();

            //_logger.LogWarning($"Load Card Backups - adding {idCount} cards");

            //for (int i = 0; i < idCount; i++)
            //{
            //    if (i % 100 == 0)
            //    {
            //        _logger.LogWarning($"RestoreDb - LoadCardBackups...{i} / {idCount} cards loaded");
            //    }
            //    await EnsureCardDefinitionExists(uniqueMIDs[i]);
            //}

            //_logger.LogWarning($"Load Card Backups - adding {idCount} cards.  Querying card definitions...");



            //////////////////////////////
            ///// In the case of restoring the DB, there's no initial card definition to check

            ////I still need a collection of all scryfall cards
            ////in theory these should all exist in the scry DB, but they still have to be mapped to a magic card

            //var getScryCardTasks = uniqueMIDs.Select(mid => _scryRepo.GetCardById(mid)).ToList();

            //var CardsDefinitionsToAdd = await Task.WhenAll(getScryCardTasks);

            //await _cardRepo.AddCardDefinitionBatch(CardsDefinitionsToAdd.ToList());


            ////for (int i = 0; i < idCount; i++)
            ////{
            ////    if (i % 100 == 0)
            ////    {
            ////        _logger.LogWarning($"RestoreDb - LoadCardBackups...{i} / {idCount} cards loaded");
            ////    }


            ////    int multiverseId = uniqueMIDs[i];


            ////    // In the case of restoring the DB, there's no initial card definition to check
            ////    //var dbCard = await _cardRepo.QueryCardDefinitions().FirstOrDefaultAsync(x => x.Id == multiverseId);
            ////    //if (dbCard != null) return;


            ////    var scryfallCard = await _scryRepo.GetCardById(multiverseId);

            ////    await _cardRepo.AddCardDefinition(scryfallCard);

            ////}

            //////////////////////////////

            _logger.LogWarning("RestoreDb - LoadCardBackups...definitions exist, mapping & saving");



            //refService.getAllVariants

            List<CardVariantTypeData> allVariants = await _dataReferenceService.GetAllCardVariantTypes();

            var mappedInventoryCards = parseCardsBackups.Select(x => new InventoryCardData
            {
                InventoryCardStatusId = x.InventoryCardStatusId,
                IsFoil = x.IsFoil,
                MultiverseId = x.MultiverseId,
                //VariantTypeId = allVariants.FirstOrDefault(v => v.Name == x.VariantName).Id,
                VariantType = allVariants.FirstOrDefault(v => v.Name == x.VariantName),

                DeckCards = 
                    //x.DeckCards.Count() > 0 ?
                    x.DeckCards.Select(d => new DeckCardData()
                    {
                        DeckId = d.DeckId,
                        CategoryId = d.Category,
                    }).ToList(),// : null,
            }).ToList();

            for(int i = 0; i < mappedInventoryCards.Count(); i++)
            {
                await _inventoryDataRepo.AddInventoryCard(mappedInventoryCards[i]);
            }



            //var cardsInDecks = mappedInventoryCards.Where(x => x.DeckCards.Count() > 0).ToList();

            //var cardsNotInDecks = mappedInventoryCards.Where(x => x.DeckCards.Count() == 0).ToList();

            //await _inventoryDataRepo.AddInventoryCardBatch(cardsNotInDecks);
            //await _inventoryDataRepo.AddInventoryCardBatch(cardsInDecks);

            ////await _inventoryDataRepo.AddInventoryCardBatch(mappedInventoryCards);

            //_logger.LogWarning("RestoreDb - LoadCardBackups...COMPLETE!");
        }

    }
}
