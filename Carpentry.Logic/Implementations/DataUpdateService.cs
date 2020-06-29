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
    public class DataUpdateService : IDataUpdateService
    {
        private readonly ILogger<DataBackupService> _logger;

        private readonly IScryfallService _scryService;

        private readonly ICardDataRepo _cardRepo;

        private readonly IScryfallDataRepo _scryfallRepo;

        private readonly int _dbRefreshIntervalDays;

        private readonly IDataReferenceRepo _dataReferenceRepo;

        public DataUpdateService(
            ILogger<DataBackupService> logger,
            IScryfallService scryService,
            ICardDataRepo cardRepo,
            IScryfallDataRepo scryfallRepo,
            IDataReferenceRepo dataReferenceRepo
            )
        {
            _logger = logger;
            _scryService = scryService;
            _scryfallRepo = scryfallRepo;
            _cardRepo = cardRepo;
            _dataReferenceRepo = dataReferenceRepo;
            _dbRefreshIntervalDays = 30; //TODO - move to a config
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

            //await _cardRepo.AddCardDefinition(scryfallCard);
        }

        /// <summary>
        /// Updates all pricing and legality data for a card set
        /// </summary>
        /// <returns></returns>
        public async Task UpdateAllSets()
        {
            _logger.LogInformation("UpdateAllSets - Calculating sets to update");

            var setCodes = await _cardRepo.GetAllCardSetCodes();

            _logger.LogInformation($"Found {setCodes.Count()} sets in the database, checking scry definitions");

            //What if, instead of doing everything for 1 set at a time, why not...
            //  Update the scry definitions of all sets (not parsing anything)
            //  THEN, 1 at a time, parsing the scry definitions, seeing what doesn't get handled properly
            //      I should prevent this from saving a given set if there is a SINGLE unhandled card

            //Note - I know there's no way the scry data will be OLDER than the card data

            //Get all un-parsed scry data
            for (int i = 0; i < setCodes.Count(); i++)
            {
                //get scry set last updated, don't parse

            }


            //try to parse every scryfall set, expecting an error if anything doesn't parse properly
            for (int i = 0; i < setCodes.Count(); i++)
            {
                

            }


            for (int i = 0; i < setCodes.Count(); i++)
            {

                DateTime? dbLastUpdated = await _cardRepo.GetCardSetLastUpdated(setCodes[i]);

                //if (dbLastUpdated != null && dbLastUpdated.Value.AddDays(_dbRefreshIntervalDays) > DateTime.Today.Date)
                //{
                //    _logger.LogInformation($"Set code {setCodes[i]} was last updated {dbLastUpdated.ToString()}, nothing will be updated.");
                //    //continue;
                //} 
                if (dbLastUpdated != null)
                {
                    _logger.LogInformation($"Set code {setCodes[i]} was last updated {dbLastUpdated.ToString()}, nothing will be updated.");
                    //continue;
                }
                else
                {
                    string replaceReason = dbLastUpdated == null ? "has never been updated" : $"was last updated {dbLastUpdated.ToString()}";
                    _logger.LogInformation($"Set code {setCodes[i]} {replaceReason}.  Set will now be updated");
                    await UpdateSetData(setCodes[i]);
                }
            }

            _logger.LogWarning("UpdateAllSets - Done refreshing set data");

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

                await _scryfallRepo.AddOrUpdateSet(scryData);
            }
            else
            {
                scryData = await _scryfallRepo.GetSetByCode(setCode);
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
                await _scryfallRepo.AddOrUpdateSet(scryData);
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

            //AddOrUpdate all card definitions in card DB
            foreach(var card in magicCards)
            {
                CardDataDto cardDefinition = new CardDataDto()
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
                    Variants = card.Variants.Keys.Select(x => new CardVariantDto
                    {
                        Name = x,
                        Image = card.Variants[x],
                        Price = card.Prices[x],
                        PriceFoil = card.Prices[$"{x}_foil"],
                    }).ToList(),
                };

                await _cardRepo.AddOrUpdateCardDefinition(cardDefinition);
            }



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

        public async Task EnsureDatabasesCreated()
        {
            
            await _scryfallRepo.EnsureDatabaseExists();
            await _cardRepo.EnsureDatabaseExists();
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
                await _dataReferenceRepo.TryAddInventoryCardStatus(allStatuses[i]);
            }

            //var statusTasks = allStatuses.Select(s => _dataReferenceRepo.TryAddInventoryCardStatus(s));

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
                await _dataReferenceRepo.TryAddCardRarity(allRarities[i]);
            }

            //var tasks = allRarities.Select(r => _dataReferenceRepo.TryAddCardRarity(r));

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
                await _dataReferenceRepo.TryAddManaType(allManaTypes[i]);
            }

            //var tasks = allManaTypes.Select(x => _dataReferenceRepo.TryAddManaType(x));

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
                await _dataReferenceRepo.TryAddMagicFormat(allFormats[i]);
            }

            //var tasks = allFormats.Select(x => _dataReferenceRepo.TryAddMagicFormat(x));

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
                await _dataReferenceRepo.TryAddCardVariantType(allVariants[i]);
            }

            //var tasks = allVariants.Select(x => _dataReferenceRepo.TryAddCardVariantType(x));

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
                await _dataReferenceRepo.TryAddDeckCardCategory(allCategories[i]);
            }

            //var tasks = allCategories.Select(x => _dataReferenceRepo.TryAddDeckCardCategory(x));

            //await Task.WhenAll(tasks);

            _logger.LogInformation("Finished adding card categories");
        }

        //
        //
        //

        public async Task<List<SetDetailDto>> GetTrackedSets()
        {
            //Might as well just get all sets from the Carpentry DB
            //TODO - consider replacing this with a view
            var dbSets = await _cardRepo.GetAllCardSets();

            var result = dbSets.Select(s => new SetDetailDto()
            {
                Code = s.Code,
                DataLastUpdated = s.LastUpdated,
                Name = s.Name,
            }).ToList();

            foreach(var dto in result)
            {
                dto.ScryLastUpdated = await _scryfallRepo.GetSetDataLastUpdated(dto.Code);
            }

            return result;
        }

        public async Task<List<SetDetailDto>> GetUntrackedSets()
        {
            //goal - get all scryfall sets NOT currently tracked by the app
            //will exclude online-only sets, really old sets, and weird promos

            //check the scry repo for most recent data
            //  if not up to date: call scry repo, filter, and apply to DB
            //  return list of available sets (dataLastUpdated == null for all)

            var auditData = await _scryfallRepo.GetAuditData();

            if(auditData == null || auditData.DefinitionsLastUpdated == null || auditData.DefinitionsLastUpdated.Value.Date < DateTime.Today)
            {
                //get the list of sets from the scryfall service

                //maybe do some filtering

                //add the results to the DB
            }

            //var trackedSets = await GetTrackedSets();

            var trackedSetCodes = GetTrackedSets().Result.Select(x => x.Code).ToList(); //will this work?.....

            var scrySets = _scryfallRepo.GetAvailableSetOverviews().Result
                .Where(x => !trackedSetCodes.Contains(x.Code))
                .Select(x => new SetDetailDto()
                {
                    Code = x.Code,
                    DataLastUpdated = null,
                    InventoryCardCount = 0,
                    Name = x.Name,
                    ScryLastUpdated = x.LastUpdated,
                })
                .ToList(); //or this?

            return scrySets;
        }

        public async Task UpdateTrackedSetScryData(string setCode)
        {
            throw new NotImplementedException();
        }

        public async Task UpdateTrackedSetCardData(string setCode)
        {
            throw new NotImplementedException();
        }

        public async Task AddTrackedSet(string setCode)
        {
            //This should return silently if a set code already exists
            //It should add the card definitions, not just an empty shell of a set
            throw new NotImplementedException();
        }

        public async Task RemoveTrackedSet(string setCode)
        {
            throw new NotImplementedException();
        }
    }
}
