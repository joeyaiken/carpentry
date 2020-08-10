using Carpentry.Data.DataContext;
using Carpentry.Data.Interfaces;
using Carpentry.Data.DataModels;
using Carpentry.Logic.Interfaces;
using Carpentry.Logic.Models;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Carpentry.Data.Models;

namespace Carpentry.Logic.Implementations
{
    //TODO - consider renaming this to "DataMaintenanceService" or "DataIntegrityService"
    //Idea being this checks if sets / cards exist, and can check if DB defaults
    // Maybe just CardDataService?
    //      Would that be a good spot to also get filter options?
    public class DataUpdateService : IDataUpdateService
    {
        private readonly ILogger<DataBackupService> _logger;

        private readonly IScryfallService _scryService;

        private readonly ICardDataRepo _cardRepo;

        private readonly IScryfallDataRepo _scryfallRepo;

        private readonly int _dbRefreshIntervalDays;

        private readonly ICoreDataRepo _coreDataRepo;


        public DataUpdateService(
            ILogger<DataBackupService> logger,
            IScryfallService scryService,
            ICardDataRepo cardRepo,
            IScryfallDataRepo scryfallRepo,
            ICoreDataRepo coreDataRepo
            //IDataQueryService dataQueryService
            )
        {
            _logger = logger;
            _scryService = scryService;
            _scryfallRepo = scryfallRepo;
            _cardRepo = cardRepo;
            _coreDataRepo = coreDataRepo;
            _dbRefreshIntervalDays = 30; //TODO - move to a config
            //_dataQueryService = dataQueryService;
        }

        public async Task EnsureCardDefinitionExists(int multiverseId)
        {
            //var dbCard = await _cardRepo.QueryCardDefinitions().FirstOrDefaultAsync(x => x.Id == multiverseId);
            var dbCard = await _cardRepo.GetCardData(multiverseId);

            if (dbCard != null)
            {
                return;
            }

            var scryfallCard = await _scryService.GetCardByMid(multiverseId);

            await UpdateSetData(scryfallCard.Set);
        }

        /// <summary>
        /// Update the card and scry data for a particular set
        /// </summary>
        /// <param name="setCode"></param>
        /// <returns></returns>
        public async Task UpdateSetData(string setCode)
        {
            //steps to update a set:

            //check if the set is actually up to date, return if it is
            DateTime? dbLastUpdated = await _cardRepo.GetCardSetLastUpdated(setCode);
            if (dbLastUpdated != null && dbLastUpdated.Value.AddDays(_dbRefreshIntervalDays) > DateTime.Today.Date)
            {
                _logger.LogInformation($"UpdateSetData - Card set {setCode} is already up to date, nothing will be updated.");
                return;
            }

            //check if the scry set is up to date
            DateTime? scryDataLastUpdated = await _scryfallRepo.GetSetDataLastUpdated(setCode);

            ScryfallSetData scryData = null;

            //If not, get from SF
            if (scryDataLastUpdated == null || scryDataLastUpdated.Value.AddDays(_dbRefreshIntervalDays) < DateTime.Today.Date)
            {
                var scryfallPayload = await _scryService.GetFullSet(setCode);

                scryData = new ScryfallSetData
                {
                    Code = scryfallPayload.Code,
                    Name = scryfallPayload.Name,
                    DataIsParsed = false,
                    LastUpdated = DateTime.Now,
                    ReleasedAt = DateTime.Parse(scryfallPayload.ReleaseDate),
                    CardData = JsonConvert.SerializeObject(scryfallPayload.CardTokens)
                };

                await _scryfallRepo.AddOrUpdateSet(scryData, true);
            }
            else
            {
                scryData = await _scryfallRepo.GetSetByCode(setCode, true);
            }

            List<MagicCardDto> magicCards = null;

            //check if the scry set is parsed
            if (scryData.DataIsParsed)
            {
                //deserialize as MagicCard
                magicCards = JsonConvert.DeserializeObject<List<MagicCardDto>>(scryData.CardData);
            }
            else
            {
                //deserialize as JToken list
                List<JToken> unparsedCards = JsonConvert.DeserializeObject<List<JToken>>(scryData.CardData);

                //map
                magicCards = _scryService.MapScryfallDataToCards(unparsedCards).Select(x => x.ToMagicCard()).ToList();

                //Serialize & apply to set
                scryData.CardData = JsonConvert.SerializeObject(magicCards);
                scryData.DataIsParsed = true;
                await _scryfallRepo.AddOrUpdateSet(scryData, true);
            }

            //Ensure the set definition exists / get the set ID
            CardSetData cardSet = new CardSetData()
            {
                Code = scryData.Code,
                Name = scryData.Name,
                LastUpdated = null,
                ReleaseDate = scryData.ReleasedAt,
            };
            cardSet.Id = await _cardRepo.AddOrUpdateCardSet(cardSet);


            //need an opperation for "AddCardDataForSet(
            //AddCardDataBatch
            List<CardDataDto> cardsToAdd = magicCards.Select(card => new CardDataDto()
            {
                Cmc = card.Cmc,
                ColorIdentity = card.ColorIdentity,
                Colors = card.Colors,
                Legalities = card.Legalities,
                ManaCost = card.ManaCost,
                MultiverseId = card.MultiverseId,
                Name = card.Name,
                Rarity = card.Rarity,
                //Set = card.Set,
                Text = card.Text,
                Type = card.Type,
                Variants = card.Variants.Keys.Select(x => new CardVariantDto
                {
                    Name = x,
                    Image = card.Variants[x],
                    Price = card.Prices[x],
                    PriceFoil = card.Prices[$"{x}_foil"],
                }).ToList(),
            }).ToList();


            await _cardRepo.AddCardDataBatch(cardsToAdd);


            //AddOrUpdate all card definitions in card DB
            //foreach (var card in magicCards)
            //{
            //    CardDataDto cardDefinition = new CardDataDto()
            //    {
            //        Cmc = card.Cmc,
            //        ColorIdentity = card.ColorIdentity,
            //        Colors = card.Colors,
            //        Legalities = card.Legalities,
            //        ManaCost = card.ManaCost,
            //        MultiverseId = card.MultiverseId,
            //        Name = card.Name,
            //        Rarity = card.Rarity,
            //        Set = card.Set,
            //        Text = card.Text,
            //        Type = card.Type,
            //        Variants = card.Variants.Keys.Select(x => new CardVariantDto
            //        {
            //            Name = x,
            //            Image = card.Variants[x],
            //            Price = card.Prices[x],
            //            PriceFoil = card.Prices[$"{x}_foil"],
            //        }).ToList(),
            //    };

            //    await _cardRepo.AddOrUpdateCardDefinition(cardDefinition);
            //}



            //var cardTasks = magicCards.Select(card =>
            //{
            //    //_cardRepo.add
            //    CardDataDto cardDefinition = new CardDataDto()
            //    {
            //        Cmc = card.Cmc,
            //        ColorIdentity = card.ColorIdentity,
            //        Colors = card.Colors,
            //        Legalities = card.Legalities,
            //        ManaCost = card.ManaCost,
            //        MultiverseId = card.MultiverseId,
            //        Name = card.Name,
            //        Rarity = card.Rarity,
            //        Set = card.Set,
            //        Text = card.Text,
            //        Type = card.Type,
            //        Variants = card.Variants.Keys.Select(x => new CardVariantDto
            //        {
            //            Name = x,
            //            Image = card.Variants[x],
            //            Price = card.Prices[x],
            //            PriceFoil = card.Prices[$"{x}_foil"],
            //        }).ToList(),
            //    };

            //    return _cardRepo.AddOrUpdateCardDefinition(cardDefinition);
            //}).ToList();

            //await Task.WhenAll(cardTasks);

            //Update the LastUpdated timestamp after cards have been successfully updated
            cardSet.LastUpdated = DateTime.Now;
            await _cardRepo.AddOrUpdateCardSet(cardSet);
            
            //////////////////////

            //_logger.LogError($"Refresh Set Data - Beginning process of refreshing {setCode}");

            ////update any card definitions for the set
            //var cardsToUpdate = await _cardContext.Cards.Where(x => x.SetId == setDefinition.Id).ToListAsync();

            //if (cardsToUpdate.Any())
            //{
            //    //update all cards
            //    var updateTasks = cardsToUpdate.Select(card =>
            //    {
            //        var thisCardFromScryfall = setFromScryfall.Cards.Where(c => c.MultiverseId == card.Id).FirstOrDefault();
            //        return _cardRepo.UpdateCardDefinition(thisCardFromScryfall);
            //    });

            //    await Task.WhenAll(updateTasks);
            //}


            ////update the scry set record
            //existingScryfallSet.LastUpdated = DateTime.Now;

            //await _scryRepo.UpdateScrySet(existingScryfallSet);

            //_logger.LogError($"Refresh Set Data - Completed process of refreshing {setCode}");
        }



        private async Task<List<CardDataDto>> GetUpdatedScrySet(int setId, string setCode)
        {
            //check if the scry set is up to date
            DateTime? scryDataLastUpdated = await _scryfallRepo.GetSetDataLastUpdated(setCode);

            ScryfallSetData scryData = null;

            //If not, get from SF
            if (scryDataLastUpdated == null || scryDataLastUpdated.Value.AddDays(_dbRefreshIntervalDays) < DateTime.Today.Date)
            {
                var scryfallPayload = await _scryService.GetFullSet(setCode);

                scryData = new ScryfallSetData
                {
                    Code = scryfallPayload.Code,
                    Name = scryfallPayload.Name,
                    DataIsParsed = false,
                    LastUpdated = DateTime.Now,
                    ReleasedAt = DateTime.Parse(scryfallPayload.ReleaseDate),
                    CardData = JsonConvert.SerializeObject(scryfallPayload.CardTokens)
                };

                await _scryfallRepo.AddOrUpdateSet(scryData, true);
            }
            else
            {
                scryData = await _scryfallRepo.GetSetByCode(setCode, true);
            }

            List<MagicCardDto> magicCards = null;

            //check if the scry set is parsed
            if (scryData.DataIsParsed)
            {
                //deserialize as MagicCard
                magicCards = JsonConvert.DeserializeObject<List<MagicCardDto>>(scryData.CardData);
            }
            else
            {
                //deserialize as JToken list
                List<JToken> unparsedCards = JsonConvert.DeserializeObject<List<JToken>>(scryData.CardData);

                //map
                magicCards = _scryService.MapScryfallDataToCards(unparsedCards).Select(x => x.ToMagicCard()).ToList();

                //Serialize & apply to set
                scryData.CardData = JsonConvert.SerializeObject(magicCards);
                scryData.DataIsParsed = true;
                await _scryfallRepo.AddOrUpdateSet(scryData, true);
            }

            List<CardDataDto> mappedCards = magicCards.Select(card => new CardDataDto()
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
                //SetId = setId,
                Text = card.Text,
                Type = card.Type,
                Variants = card.Variants.Keys.Select(x => new CardVariantDto
                {
                    Name = x,
                    Image = card.Variants[x],
                    Price = card.Prices[x],
                    PriceFoil = card.Prices[$"{x}_foil"],
                }).ToList(),
            }).ToList();

            return mappedCards;
        }

        public async Task EnsureDatabasesCreated()
        {
            
            await _scryfallRepo.EnsureDatabaseExists();
            await _coreDataRepo.EnsureDatabaseExists();
            //await _cardContext.Database.EnsureCreatedAsync();
            //await _scryRepo.EnsureDatabaseCreated();
        }

        public async Task EnsureDefaultRecordsExist()
        {
            _logger.LogWarning("Adding default records");

            await EnsureDbCardStatusesExist();
            await EnsureDbRaritiesExist();
            await EnsureDbManaTypesExist();
            await EnsureDbMagicFormatsExist();
            await EnsureDbVariantTypesExist();
            await EnsureDbDeckCardCategoriesExist();


            //List<Task> defaultRecordTasks = new List<Task>()
            //{
            //    EnsureDbCardStatusesExist(),
            //    EnsureDbRaritiesExist(),
            //    EnsureDbManaTypesExist(),
            //    EnsureDbMagicFormatsExist(),
            //    EnsureDbVariantTypesExist(),
            //    EnsureDbDeckCardCategoriesExist(),
            //};

            //await Task.WhenAll(defaultRecordTasks);

            _logger.LogInformation("Finished adding default records");
        }
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
                new InventoryCardStatusData { Id = 1, Name = "Inventory" },
                new InventoryCardStatusData { Id = 2, Name = "Buy List" },
                new InventoryCardStatusData { Id = 3, Name = "Sell List" },
            };

            for(int i = 0; i < allStatuses.Count(); i++)
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
                    Id = 'M',
                    Name = "mythic",
                },
                new CardRarityData
                {
                    Id = 'R',
                    Name = "rare",
                },
                new CardRarityData
                {
                    Id = 'U',
                    Name = "uncommon",
                },
                new CardRarityData
                {
                    Id = 'C',
                    Name = "common",
                },
            };

            for(int i = 0; i < allRarities.Count(); i++)
            {
                await _coreDataRepo.TryAddCardRarity(allRarities[i]);
            }

            //var tasks = allRarities.Select(r => _coreDataRepo.TryAddCardRarity(r));

            //await Task.WhenAll(tasks);

            _logger.LogInformation("Finished adding card rarities");
        }

        private async Task EnsureDbManaTypesExist()
        {
            List<ManaTypeData> allManaTypes = new List<ManaTypeData>()
            {
                new ManaTypeData{
                    Id = 'W',
                    Name = "White"
                },
                new ManaTypeData{
                    Id = 'U',
                    Name = "Blue"
                },
                new ManaTypeData{
                    Id = 'B',
                    Name = "Black"
                },
                new ManaTypeData{
                    Id = 'R',
                    Name = "Red"
                },
                new ManaTypeData{
                    Id = 'G',
                    Name = "Green"
                },
            };
            
            for(int i = 0; i < allManaTypes.Count(); i++)
            {
                await _coreDataRepo.TryAddManaType(allManaTypes[i]);
            }

            //var tasks = allManaTypes.Select(x => _coreDataRepo.TryAddManaType(x));

            //await Task.WhenAll(tasks);

            _logger.LogInformation("Finished adding mana types");
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
                //new MagicFormat { Name = "duel" },
                //new MagicFormat { Name = "oldschool" },
            };

            for(int i = 0; i < allFormats.Count(); i++)
            {
                await _coreDataRepo.TryAddMagicFormat(allFormats[i]);
            }

            //var tasks = allFormats.Select(x => _coreDataRepo.TryAddMagicFormat(x));

            //await Task.WhenAll(tasks);

            _logger.LogInformation("Finished adding formats");
        }

        private async Task EnsureDbVariantTypesExist()
        {
            List<CardVariantTypeData> allVariants = new List<CardVariantTypeData>()
            {
                new CardVariantTypeData { Name = "normal" },
                new CardVariantTypeData { Name = "borderless" },
                new CardVariantTypeData { Name = "showcase" },
                new CardVariantTypeData { Name = "extendedart" },
                new CardVariantTypeData { Name = "inverted" },
                new CardVariantTypeData { Name = "promo" },
                new CardVariantTypeData { Name = "ja" },
                new CardVariantTypeData { Name = "unknown" }
            };

            for(int i = 0; i < allVariants.Count(); i++)
            {
                await _coreDataRepo.TryAddCardVariantType(allVariants[i]);
            }

            //var tasks = allVariants.Select(x => _coreDataRepo.TryAddCardVariantType(x));

            //await Task.WhenAll(tasks);

            _logger.LogInformation("Finished adding card variants");
        }

        private async Task EnsureDbDeckCardCategoriesExist()
        {
            List<DeckCardCategoryData> allCategories = new List<DeckCardCategoryData>()
            {
                //null == mainboard new DeckCardCategory { Id = '', Name = "" },
                new DeckCardCategoryData { Id = 'c', Name = "Commander" },
                new DeckCardCategoryData { Id = 's', Name = "Sideboard" },
                //Companion

                //new DeckCardCategory { Id = '', Name = "" },
                //new DeckCardCategory { Id = '', Name = "" },
            };//TryAddDeckCardCategory

            for(int i = 0; i < allCategories.Count(); i++)
            {
                await _coreDataRepo.TryAddDeckCardCategory(allCategories[i]);
            }

            //var tasks = allCategories.Select(x => _coreDataRepo.TryAddDeckCardCategory(x));

            //await Task.WhenAll(tasks);

            _logger.LogInformation("Finished adding card categories");
        }

        //
        //
        //

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

            if(dbSet == null)
            {
                throw new ArgumentException($"No set matching provided set ID of {setId}");
            }

            if (dbSet.IsTracked)
            {
                return;
            }

            //get current scry data
            var scryData = await GetUpdatedScrySet(setId, dbSet.Code);

            await _cardRepo.AddCardDataBatch(scryData);

            dbSet.IsTracked = true;
            dbSet.LastUpdated = DateTime.Now;
            await _cardRepo.AddOrUpdateCardSet(dbSet);
            //throw new NotImplementedException();
        }

        public async Task AddTrackedSet(string setCode)
        {
            throw new NotImplementedException();
        }

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

            if (dbSet.LastUpdated != null && dbSet.LastUpdated.Value.AddDays(_dbRefreshIntervalDays) > DateTime.Today.Date)
            {
                _logger.LogInformation($"UpdateSetData - Card set {dbSet.Code} is already up to date, nothing will be updated.");
                return;
            }

            var scryData = await GetUpdatedScrySet(setId, dbSet.Code);

            await _cardRepo.UpdateCardDataBatch(scryData);

            //get data from scryfall

            //update scryfall if out of date

            //1:    itterate over each card, checking the existing DB card
            //2:    Remove all CardVariant & CardLegalities for the cards, & re-populate just those records

            dbSet.LastUpdated = DateTime.Now;
            await _cardRepo.AddOrUpdateCardSet(dbSet);
            //throw new NotImplementedException();
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
            
            if(setTotals.InventoryCount > 0 || setTotals.CollectedCount > 0)
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
            //throw new NotImplementedException();
        }

        /// <summary>
        /// Update the list of sets available to track (does not update the data of cards in the set)
        /// Gets called by DataRestore service to populate possible sets
        /// </summary>
        /// <returns></returns>
        public async Task TryUpdateAvailableSets()
        {
            //Update scry data, if not updated today
            var auditData = await _scryfallRepo.GetAuditData();
            if (auditData == null || auditData.DefinitionsLastUpdated == null || auditData.DefinitionsLastUpdated.Value.Date < DateTime.Today)
            {
                //get the list of sets from the scryfall service
                var allSetsResult = await _scryService.GetAllSets();

                //update scry data
                foreach(var setResult in allSetsResult)
                {
                    var scryRepoSet = await _scryfallRepo.GetSetByCode(setResult.Code, false);

                    if (scryRepoSet == null)
                    {
                        scryRepoSet = new ScryfallSetData();
                    }

                    scryRepoSet.Code = setResult.Code;
                    scryRepoSet.Name = setResult.Name;
                    scryRepoSet.ReleasedAt = DateTime.Parse(setResult.ReleasedAtString);
                    scryRepoSet.SetType = setResult.SetType;
                    scryRepoSet.CardCount = setResult.CardCount;
                    scryRepoSet.Digital = setResult.Digital;
                    scryRepoSet.NonfoilOnly = setResult.NonfoilOnly;
                    scryRepoSet.FoilOnly = setResult.FoilOnly;

                    await _scryfallRepo.AddOrUpdateSet(scryRepoSet, false);
                }

                await _scryfallRepo.SetAuditData();
            }

            //get the set definition overviews from the scry DB
            var scrySets = await _scryfallRepo.GetAvailableSetOverviews();

            var existingCardSets = await _cardRepo.GetAllCardSets();

            foreach (var scrySet in scrySets)
            {
                //for each result, see if it exists in the DB
                var existingSet = existingCardSets.Where(x => x.Code.ToLower() == scrySet.Code.ToLower()).FirstOrDefault();
                
                //if it doesn't, create a new one that can be added to the db
                if(existingSet == null)
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
    }
}
