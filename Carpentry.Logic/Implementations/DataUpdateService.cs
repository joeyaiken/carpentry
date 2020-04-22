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
            _dbRefreshIntervalDays = 0; //TODO - move to a config
        }

        public async Task EnsureCardDefinitionExists(int multiverseId)
        {
            //var dbCard = await _cardRepo.QueryCardDefinitions().FirstOrDefaultAsync(x => x.Id == multiverseId);
            var dbCard = await _cardRepo.GetCardById(multiverseId);

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

            _logger.LogInformation($"Found {setCodes.Count()} sets in the database, checking last updated");

            for (int i = 0; i < setCodes.Count(); i++)
            {

                DateTime? dbLastUpdated = await _cardRepo.GetCardSetLastUpdated(setCodes[i]);

                if (dbLastUpdated != null && dbLastUpdated.Value.AddDays(_dbRefreshIntervalDays) > DateTime.Today.Date)
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
            var cardTasks = magicCards.Select(card =>
            {
                //_cardRepo.add
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

                return _cardRepo.AddOrUpdateCardDefinition(cardDefinition);
            }).ToList();

            await Task.WhenAll(cardTasks);

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

            List<Task> defaultRecordTasks = new List<Task>()
            {
                EnsureDbCardStatusesExist(),
                EnsureDbRaritiesExist(),
                EnsureDbManaTypesExist(),
                EnsureDbMagicFormatsExist(),
                EnsureDbVariantTypesExist(),
                EnsureDbDeckCardCategoriesExist(),
            };

            await Task.WhenAll(defaultRecordTasks);

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

            var statusTasks = allStatuses.Select(s => _dataReferenceRepo.TryAddInventoryCardStatus(s));

            await Task.WhenAll(statusTasks);

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

            var tasks = allRarities.Select(r => _dataReferenceRepo.TryAddCardRarity(r));

            await Task.WhenAll(tasks);

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

            var tasks = allManaTypes.Select(x => _dataReferenceRepo.TryAddManaType(x));

            await Task.WhenAll(tasks);

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

            var tasks = allFormats.Select(x => _dataReferenceRepo.TryAddMagicFormat(x));

            await Task.WhenAll(tasks);

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
            };

            var tasks = allVariants.Select(x => _dataReferenceRepo.TryAddCardVariantType(x));

            await Task.WhenAll(tasks);

            _logger.LogInformation("Finished adding card variants");
        }

        private async Task EnsureDbDeckCardCategoriesExist()
        {
            List<DeckCardCategoryData> allCategories = new List<DeckCardCategoryData>()
            {
                //null == mainboard new DeckCardCategory { Id = '', Name = "" },
                new DeckCardCategoryData { Id = 'c', Name = "Commander" },
                new DeckCardCategoryData { Id = 's', Name = "Sideboard" },
                //new DeckCardCategory { Id = '', Name = "" },
                //new DeckCardCategory { Id = '', Name = "" },
            };//TryAddDeckCardCategory

            var tasks = allCategories.Select(x => _dataReferenceRepo.TryAddDeckCardCategory(x));

            await Task.WhenAll(tasks);

            _logger.LogInformation("Finished adding card categories");
        }

    }
}
