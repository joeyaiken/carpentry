using Carpentry.Logic.Models;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Carpentry.DataLogic;
using Carpentry.CarpentryData.Models;
using Carpentry.DataLogic.Models;
using Carpentry.ScryfallData.Models;
using Carpentry.CarpentryData;
using Microsoft.EntityFrameworkCore;

namespace Carpentry.Logic
{
    public interface IDataUpdateService
    {
        Task ValidateDatabase();
        Task EnsureDatabasesCreated(); //Currently only used by QuickRestore, I don't know where these would otherwise get called (validateDB)
        Task EnsureDefaultRecordsExist();

        Task<List<SetDetailDto>> GetTrackedSets(bool showUntracked, bool update);
        Task AddTrackedSet(int setId);
        Task RemoveTrackedSet(int setId);
        Task UpdateTrackedSet(int setId);

        Task TryUpdateAvailableSets();

    }

    //TODO - consider renaming this to "DataMaintenanceService" or "DataIntegrityService"
    //Idea being this checks if sets / cards exist, and can check if DB defaults
    // Maybe just CardDataService?
    //      Would that be a good spot to also get filter options?
    //
    //Note on "premium" cards
    //  For WAR, scryfall includes 36 Japanese alt-art plainswalkers
    //  For now, I'm intentionally not adding them to the system
    //  IDK what other sets include premium cards, but they may exist
    //  A premium card is an card with a collector_number that includes a star/★
    public class DataUpdateService : IDataUpdateService
    {
        private readonly ILogger<DataExportService> _logger;
        private readonly IScryfallService _scryService;
        private readonly CarpentryDataContext _cardContext;
        private readonly int _dbRefreshIntervalDays;

        private readonly ICardDataRepo _cardRepo;
        private readonly IScryfallDataRepo _scryfallRepo;
        private readonly ICoreDataRepo _coreDataRepo;

        public DataUpdateService(
            ILogger<DataExportService> logger,
            IScryfallService scryService,
            ICardDataRepo cardRepo,
            IScryfallDataRepo scryfallRepo,
            ICoreDataRepo coreDataRepo
            //IDataQueryService dataQueryService
            , CarpentryDataContext cardContext
            )
        {
            _logger = logger;
            _scryService = scryService;
            _scryfallRepo = scryfallRepo;
            _cardRepo = cardRepo;
            _coreDataRepo = coreDataRepo;
            _dbRefreshIntervalDays = 0; //TODO - move to a config
            //_dataQueryService = dataQueryService;
            _cardContext = cardContext;
        }

        #region Private Methods - Default Records

        private async Task EnsureDbCardStatusesExist()
        {
            /*
             Statuses:
             1 - Inventory/Owned
             2 - Buylist
             3 - SellList
             */

            List<InventoryCardStatusData> allStatuses = new List<InventoryCardStatusData>()
            {
                new InventoryCardStatusData { CardStatusId = 1, Name = "Inventory" },
                new InventoryCardStatusData { CardStatusId = 2, Name = "Buy List" },
                new InventoryCardStatusData { CardStatusId = 3, Name = "Sell List" },
            };

            for (int i = 0; i < allStatuses.Count(); i++)
            {
                await _coreDataRepo.TryAddInventoryCardStatus(allStatuses[i]);
            }

            //var statusTasks = allStatuses.Select(s => _coreDataRepo.TryAddInventoryCardStatus(s));

            //await Task.WhenAll(statusTasks);

            _logger.LogInformation("Finished adding card statuses");
        }

        private async Task EnsureDbRaritiesExist()
        {
            List<CardRarityData> allRarities = new List<CardRarityData>()
            {
                new CardRarityData
                {
                    RarityId = 'M',
                    Name = "mythic",
                },
                new CardRarityData
                {
                    RarityId = 'R',
                    Name = "rare",
                },
                new CardRarityData
                {
                    RarityId = 'U',
                    Name = "uncommon",
                },
                new CardRarityData
                {
                    RarityId = 'C',
                    Name = "common",
                },
                new CardRarityData
                {
                    RarityId = 'S',
                    Name = "special",
                },
            };

            for (int i = 0; i < allRarities.Count(); i++)
            {
                await _coreDataRepo.TryAddCardRarity(allRarities[i]);
            }

            //var tasks = allRarities.Select(r => _coreDataRepo.TryAddCardRarity(r));

            //await Task.WhenAll(tasks);

            _logger.LogInformation("Finished adding card rarities");
        }

        private async Task EnsureDbMagicFormatsExist()
        {
            //Should I just comment out formats I don't care about?
            List<MagicFormatData> allFormats = new List<MagicFormatData>()
            {
                new MagicFormatData { Name = "standard" },
                //new MagicFormat { Name = "future" },
                //new MagicFormat { Name = "historic" },
                new MagicFormatData { Name = "pioneer" },
                new MagicFormatData { Name = "modern" },
                //new MagicFormat { Name = "legacy" },
                new MagicFormatData { Name = "pauper" },
                //new MagicFormat { Name = "vintage" },
                //new MagicFormat { Name = "penny" },
                new MagicFormatData { Name = "commander" },
                new MagicFormatData { Name = "brawl" },
                new MagicFormatData { Name = "jumpstart" },
                new MagicFormatData { Name = "sealed" },
                //new MagicFormat { Name = "duel" },
                //new MagicFormat { Name = "oldschool" },
            };

            for (int i = 0; i < allFormats.Count(); i++)
            {
                await _coreDataRepo.TryAddMagicFormat(allFormats[i]);
            }

            //var tasks = allFormats.Select(x => _coreDataRepo.TryAddMagicFormat(x));

            //await Task.WhenAll(tasks);

            _logger.LogInformation("Finished adding formats");
        }

        private async Task EnsureDbDeckCardCategoriesExist()
        {
            List<DeckCardCategoryData> allCategories = new List<DeckCardCategoryData>()
            {
                //null == mainboard new DeckCardCategory { Id = '', Name = "" },
                new DeckCardCategoryData { DeckCardCategoryId = 'c', Name = "Commander" },
                new DeckCardCategoryData { DeckCardCategoryId = 's', Name = "Sideboard" },
                //Companion

                //new DeckCardCategory { Id = '', Name = "" },
                //new DeckCardCategory { Id = '', Name = "" },
            };//TryAddDeckCardCategory

            for (int i = 0; i < allCategories.Count(); i++)
            {
                await _coreDataRepo.TryAddDeckCardCategory(allCategories[i]);
            }

            //var tasks = allCategories.Select(x => _coreDataRepo.TryAddDeckCardCategory(x));

            //await Task.WhenAll(tasks);

            _logger.LogInformation("Finished adding card categories");
        }

        #endregion

        #region Get an up-to-date scryfall set

        /// <summary>
        /// This gets a list of card definitions from scryfall, mapped to a different class type
        /// </summary>
        /// <param name="setId"></param>
        /// <param name="setCode"></param>
        /// <returns></returns>
        private async Task<List<CardDataDto>> GetUpdatedScrySet(/*int setId,*/ string setCode)
        {
            //This should probably just be _scryService.GetSetDetail | GetSetCards | ??? (aka just a call to the normal service method)
            var scryPayload = await _scryService.GetSetDetail(setCode);

            var mappedCards = scryPayload.Cards.Select(card => new CardDataDto()
            {
                Cmc = card.Cmc,
                ColorIdentity = card.ColorIdentity,
                Colors = card.Colors,
                Legalities = card.Legalities,
                ManaCost = card.ManaCost,
                MultiverseId = card.MultiverseId,
                Name = card.Name,
                Rarity = card.Rarity,
                Set = card.Set,
                Text = card.Text,
                Type = card.Type,
                CollectorNumber = card.CollectionNumber,
                ImageUrl = card.ImageUrl,
                Price = card.Price,
                PriceFoil = card.PriceFoil,
                TixPrice = card.PriceTix,
            }).ToList();

            return mappedCards;
        }

        #endregion

        #region Public Methods

        public async Task ValidateDatabase()
        {
            await EnsureDatabasesCreated();
            await EnsureDefaultRecordsExist();
        }

        public async Task EnsureDatabasesCreated()
        {
            await _scryfallRepo.EnsureDatabaseExists();
            await _coreDataRepo.EnsureDatabaseExists();
        } 

        public async Task EnsureDefaultRecordsExist()
        {
            _logger.LogWarning("Adding default records");

            await EnsureDbCardStatusesExist();
            await EnsureDbRaritiesExist();
            await EnsureDbMagicFormatsExist();
            await EnsureDbDeckCardCategoriesExist();

            _logger.LogInformation("Finished adding default records");
        }

        //reset database?(SomeConfirmationClass dto){ ; }

        public async Task<List<SetDetailDto>> GetTrackedSets(bool showUntracked, bool update)
        {
            //update first?
            if (update)
            {
                await TryUpdateAvailableSets();
            }

            //Get list of sets from vwSetTotals, filtering if requested
            var dbSetTotals = _cardRepo.QuerySetTotals()
                .Where(s => showUntracked || s.IsTracked == true)
                .ToList();

            var result = dbSetTotals.Select(s => new SetDetailDto()
            {
                SetId = s.SetId,
                Code = s.Code,
                DataLastUpdated = s.LastUpdated,
                Name = s.Name,
                CollectedCount = s.CollectedCount ?? 0,
                InventoryCount = s.InventoryCount ?? 0,
                IsTracked = s.IsTracked,
                //ScryLastUpdated = null,
                TotalCount = s.TotalCount,
                ReleaseDate = s.ReleaseDate,
            })
            .OrderByDescending(s => s.ReleaseDate)
            .ToList();

            //foreach(var dto in result)
            //{
            //    dto.ScryLastUpdated = await _scryfallRepo.GetSetDataLastUpdated(dto.Code);
            //}

            return result;
        }

        public async Task AddTrackedSet(int setId)
        {
            //get the set for this ID
            var dbSet = await _cardRepo.GetCardSetById(setId);

            if (dbSet == null)
            {
                throw new ArgumentException($"No set matching provided set ID of {setId}");
            }

            if (dbSet.IsTracked)
            {
                return;
            }

            //get current scry data
            var scryData = await GetUpdatedScrySet(dbSet.Code);

            await _cardRepo.AddCardDataBatch(scryData);

            dbSet.IsTracked = true;
            dbSet.LastUpdated = DateTime.Now;
            await _cardRepo.AddOrUpdateCardSet(dbSet);
            //throw new NotImplementedException();
        }

        /// <summary>
        /// Updates the card definitions for a tracked set
        /// Will throw an error if the SetId isn't valid
        /// Will not update anything if the set isn't tracked
        /// </summary>
        /// <param name="setId"></param>
        /// <returns></returns>
        public async Task UpdateTrackedSet(int setId)
        {
            var dbSet = await _cardRepo.GetCardSetById(setId);

            if (dbSet == null)
            {
                throw new ArgumentException($"No set matching provided set ID of {setId}");
            }

            if (!dbSet.IsTracked)
            {
                return; //Don't want to update untracked sets
            }

            //Only update a set if it's "stale enough"
            if (dbSet.LastUpdated != null && dbSet.LastUpdated.Value.AddDays(_dbRefreshIntervalDays) > DateTime.Today.Date)
            {
                _logger.LogInformation($"UpdateSetData - Card set {dbSet.Code} is already up to date, nothing will be updated.");
                return;
            }

            //Get the updated card definitions, mapped to the expected CardData object
            var scryData = await GetUpdatedScrySet(dbSet.Code);

            await _cardRepo.UpdateCardDataBatch(scryData); // Should this really use a DTO?? w/e for now...

            //Finally, update the Set Definition so we know it's up to date
            dbSet.LastUpdated = DateTime.Now;
            await _cardRepo.AddOrUpdateCardSet(dbSet);
        }

        public async Task RemoveTrackedSet(int setId)
        {
            var dbSet = await _cardRepo.GetCardSetById(setId);
            var setTotals = _cardRepo.QuerySetTotals().Where(s => s.SetId == setId).FirstOrDefault();

            if (setTotals == null)
            {
                throw new ArgumentException($"No set matching provided set ID of {setId}");
            }

            if (!setTotals.IsTracked)
            {
                return; //If not tracked, just return
            }
            //get DB set

            //verify 0 owned/collected

            if (setTotals.InventoryCount > 0 || setTotals.CollectedCount > 0)
            {
                return; //won't delete anything with owned cards
                //TODO - should be able to remove a set with only buylist cards?
            }

            //remove card definitions
            await _cardRepo.RemoveAllCardDefinitionsForSetId(setId);

            //update set
            dbSet.LastUpdated = null;
            dbSet.IsTracked = false;
            await _cardRepo.AddOrUpdateCardSet(dbSet);
        }

        /// <summary>
        /// Update the list of sets available to track (does not update the data of cards in the set)
        /// Gets called by DataRestore service to populate possible sets
        /// </summary>
        /// <returns></returns>
        public async Task TryUpdateAvailableSets()
        {
            //get the set definition overviews from the scry DB
            var scrySets = await _scryService.GetSetOverviews();

            var existingCardSets = await _cardContext.Sets.ToListAsync();

            foreach (var scrySet in scrySets)
            {
                //for each result, see if it exists in the DB
                var existingSet = existingCardSets.Where(x => x.Code.ToLower() == scrySet.Code.ToLower()).FirstOrDefault();

                //if it doesn't, create a new one that can be added to the db
                if (existingSet == null)
                {
                    existingSet = new CardSetData()
                    {
                        Code = scrySet.Code,
                        IsTracked = false,
                        LastUpdated = null,
                    };
                }

                //This mainly makes sure these fields are updated in the DB
                existingSet.Name = scrySet.Name;
                existingSet.ReleaseDate = scrySet.ReleasedAt;

                await _cardRepo.AddOrUpdateCardSet(existingSet);
            }
        }

        #endregion

    }
}
