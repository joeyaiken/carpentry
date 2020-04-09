using Carpentry.Data.DataContext;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using Card = Carpentry.Data.DataContext.Card;
using InventoryCard = Carpentry.Data.DataContext.InventoryCard;
using InventoryCardStatus = Carpentry.Data.DataContext.InventoryCardStatus;
using Deck = Carpentry.Data.DataContext.Deck;
using DeckCard = Carpentry.Data.DataContext.DeckCard;
using System.Linq;
using Carpentry.Data.Models;
using Newtonsoft.Json;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Carpentry.Data.Implementations;
using Carpentry.Implementations;
using Carpentry.Data.MigrationTool.Models;
using Microsoft.EntityFrameworkCore;

namespace Carpentry.Data.MigrationTool
{
    public class DataMigrationService : IDataMigrationService
    {
        private readonly ILogger<DataMigrationService> _logger;

        private readonly SqliteDataContext _cardContext;
        //private readonly ScryfallDataContext _scryContext;

        private readonly MigrationToolConfig _config;

        private readonly ScryfallRepo _scryRepo;

        private readonly SqliteCardRepo _cardRepo;

        //private readonly CarpentryService _carpentryService;

        public DataMigrationService(
            ILoggerFactory loggerFactory,
            SqliteDataContext cardContext,
            MigrationToolConfig config,
            ScryfallRepo scryRepo,
            SqliteCardRepo cardRepo
            )
        {
            _logger = loggerFactory.CreateLogger<DataMigrationService>();
            _cardContext = cardContext;
            _config = config;
            _scryRepo = scryRepo;
            _cardRepo = cardRepo;
        }

        #region Public

        public async Task RestoreDB()
        {
            _logger.LogWarning("DataRestoreService - RestoreDb...");

            //Stary by ensuring a DB actually exists
            await EnsureDatabasesCreated();

            //ensure default records exist
            await EnsureDefaultRecordsExist();

            //load all decks
            await LoadDeckBackups();

            //try to add sets, adding only dummy info
            await LoadSetTempData();

            //run "refresh DB"
            await RefreshDB();

            //add inventory & deck cards
            await LoadCardBackups();

            _logger.LogWarning("DataRestoreService - RestoreDb...Completed!");
        }

        public async Task RefreshDB()
        {
            //Updates all pricing and legality data
            //When a scry set is updated, all data cards in that set should also be updated

            _logger.LogWarning("RefreshDB - Calculating sets to update");

            var setCodesToRefresh = await _scryRepo.QueryScrySets()
                .Where(set => set.LastUpdated == null || set.LastUpdated.Value.AddDays(_config.DataRefreshIntervalDays) < DateTime.Today.Date)
                .Select(x => x.Code).ToListAsync();

            _logger.LogWarning($"Found {setCodesToRefresh.Count()} sets to refresh");

            for (int i = 0; i < setCodesToRefresh.Count(); i++)
            {
                await RefreshSetData(setCodesToRefresh[i]);
            }

            _logger.LogWarning("RefreshDB - Done refreshing set data");
        }

        #endregion

        #region Private

        #region Defaults

        private async Task EnsureDefaultRecordsExist()
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

            _logger.LogWarning("Finished adding default records");
        }

        private async Task EnsureDbCardStatusesExist()
        {
            /*
             Statuses:
             1 - Inventory/Owned
             2 - Buylist
             3 - SellList
             */

            List<InventoryCardStatus> allStatuses = new List<InventoryCardStatus>()
            {
                new InventoryCardStatus { Id = 1, Name = "Inventory" },
                new InventoryCardStatus { Id = 2, Name = "Buy List" },
                new InventoryCardStatus { Id = 3, Name = "Sell List" },
            };

            allStatuses.ForEach(status =>
            {
                var existingStatus = _cardContext.CardStatuses.FirstOrDefault(x => x.Name == status.Name);
                if (existingStatus == null)
                {
                    _logger.LogWarning($"Adding card status {status.Name}");
                    _cardContext.CardStatuses.Add(status);
                }
            });

            await _cardContext.SaveChangesAsync();
            _logger.LogWarning("Finished adding card statuses");
        }

        private async Task EnsureDbRaritiesExist()
        {
            List<CardRarity> allRarities = new List<CardRarity>()
            {
                new CardRarity
                {
                    Id = 'M',
                    Name = "mythic",
                },
                new CardRarity
                {
                    Id = 'R',
                    Name = "rare",
                },
                new CardRarity
                {
                    Id = 'U',
                    Name = "uncommon",
                },
                new CardRarity
                {
                    Id = 'C',
                    Name = "common",
                },
            };

            allRarities.ForEach(rarity =>
            {
                var existingRecord = _cardContext.Rarities.FirstOrDefault(x => x.Id == rarity.Id);
                if (existingRecord == null)
                {
                    _logger.LogWarning($"Adding card rarity {rarity.Name}");
                    _cardContext.Rarities.Add(rarity);
                }
            });

            await _cardContext.SaveChangesAsync();

            _logger.LogWarning("Finished adding card rarities");
        }

        private async Task EnsureDbManaTypesExist()
        {
            List<ManaType> allManaTypes = new List<ManaType>()
            {
                new ManaType{
                    Id = 'W',
                    Name = "White"
                },
                new ManaType{
                    Id = 'U',
                    Name = "Blue"
                },
                new ManaType{
                    Id = 'B',
                    Name = "Black"
                },
                new ManaType{
                    Id = 'R',
                    Name = "Red"
                },
                new ManaType{
                    Id = 'G',
                    Name = "Green"
                },
            };

            allManaTypes.ForEach(type =>
            {
                var existingRecord = _cardContext.ManaTypes.FirstOrDefault(x => x.Id == type.Id);
                if (existingRecord == null)
                {
                    _logger.LogWarning($"Adding mana type {type.Name}");
                    _cardContext.ManaTypes.Add(type);
                }
            });

            await _cardContext.SaveChangesAsync();

            _logger.LogWarning("Finished adding mana types");
        }

        private async Task EnsureDbMagicFormatsExist()
        {
            //Should I just comment out formats I don't care about?
            List<MagicFormat> allFormats = new List<MagicFormat>()
            {
                new MagicFormat { Name = "standard" },
                //new MagicFormat { Name = "future" },
                //new MagicFormat { Name = "historic" },
                new MagicFormat { Name = "pioneer" },
                new MagicFormat { Name = "modern" },
                //new MagicFormat { Name = "legacy" },
                new MagicFormat { Name = "pauper" },
                //new MagicFormat { Name = "vintage" },
                //new MagicFormat { Name = "penny" },
                new MagicFormat { Name = "commander" },
                new MagicFormat { Name = "brawl" },
                //new MagicFormat { Name = "duel" },
                //new MagicFormat { Name = "oldschool" },
            };

            allFormats.ForEach(format =>
            {
                var existingFormat = _cardContext.MagicFormats.FirstOrDefault(x => x.Name == format.Name);
                if (existingFormat == null)
                {
                    _cardContext.MagicFormats.Add(format);
                    _logger.LogWarning($"Adding format {format.Name}");
                }
            });

            await _cardContext.SaveChangesAsync();

            _logger.LogWarning("Finished adding formats");
        }

        private async Task EnsureDbVariantTypesExist()
        {
            List<CardVariantType> allVariants = new List<CardVariantType>()
            {
                new CardVariantType { Name = "normal" },
                new CardVariantType { Name = "borderless" },
                new CardVariantType { Name = "showcase" },
                new CardVariantType { Name = "extendedart" },
                new CardVariantType { Name = "inverted" },
                new CardVariantType { Name = "promo" },
                new CardVariantType { Name = "ja" },
            };

            allVariants.ForEach(variant =>
            {
                var existingVariant = _cardContext.VariantTypes.FirstOrDefault(x => x.Name == variant.Name);
                if (existingVariant == null)
                {
                    _cardContext.VariantTypes.Add(variant);
                    _logger.LogWarning($"Adding variant {variant.Name}");
                }
            });

            await _cardContext.SaveChangesAsync();

            _logger.LogWarning("Finished adding card variants");
        }

        private async Task EnsureDbDeckCardCategoriesExist()
        {
            List<DeckCardCategory> allCategories = new List<DeckCardCategory>()
            {
                //null == mainboard new DeckCardCategory { Id = '', Name = "" },
                new DeckCardCategory { Id = 'c', Name = "Commander" },
                new DeckCardCategory { Id = 's', Name = "Sideboard" },
                //new DeckCardCategory { Id = '', Name = "" },
                //new DeckCardCategory { Id = '', Name = "" },
            };

            allCategories.ForEach(category =>
            {
                var existingCategory = _cardContext.DeckCardCategories.FirstOrDefault(x => x.Id == category.Id);
                if (existingCategory == null)
                {
                    _cardContext.DeckCardCategories.Add(category);
                    _logger.LogWarning($"Adding category {category.Name}");
                }
            });

            await _cardContext.SaveChangesAsync();

            _logger.LogWarning("Finished adding card categories");
        }
        #endregion

        #region Data

        private async Task EnsureDatabasesCreated()
        {
            await _cardContext.Database.EnsureCreatedAsync();
            await _scryRepo.EnsureDatabaseCreated();
        }

        private async Task LoadDeckBackups()
        {
            int existingDeckCount = _cardContext.Decks.Select(x => x.Id).Count();
            if (existingDeckCount > 0)
            {
                _logger.LogWarning("LoadDeckBackups - Decks already exist, not loading anything from parsed data");
                return;
            }

            _logger.LogWarning("LoadDeckBackups - Loading parsed decks");

            string deckBackupsDataString = await System.IO.File.ReadAllTextAsync(_config.DeckBackupLocation);
            List<BackupDeck> parsedBackupDecks = JArray.Parse(deckBackupsDataString).ToObject<List<BackupDeck>>();

            IEnumerable<Deck> newDecks = parsedBackupDecks.Select(x => new Deck()
            {
                BasicB = x.BasicB,
                BasicG = x.BasicG,
                BasicR = x.BasicR,
                BasicU = x.BasicU,
                BasicW = x.BasicW,

                Name = x.Name,
                Notes = x.Notes,

                Format = _cardContext.MagicFormats.Where(f => f.Name.ToLower() == x.Format.ToLower()).FirstOrDefault(),

                Id = x.ExportId
            }

            );//.ToList();

            await _cardContext.Decks.AddRangeAsync(newDecks);
            await _cardContext.SaveChangesAsync();
            _logger.LogWarning("LoadDeckBackups - Complete");
        }

        private async Task LoadSetTempData()
        {
            int setCount = await _cardContext.Sets.Select(x => x.Id).CountAsync();

            int scryCount = await _scryRepo.QueryScrySets().Select(x => x.Id).CountAsync();

            string propsBackupDataString = await System.IO.File.ReadAllTextAsync(_config.PropsBackupLocation);
            BackupDataProps parsedPropsBackups = JObject.Parse(propsBackupDataString).ToObject<BackupDataProps>();

            if (setCount > 0)
            {
                _logger.LogWarning("LoadSetTempData - set data already exists, not adding temp records");
            }
            else
            {
                _logger.LogWarning("LoadSetTempData - Adding set placeholder records...");

                var tempSets = parsedPropsBackups.SetCodes.Select(code => new CardSet()
                {
                    Code = code
                });

                await _cardContext.Sets.AddRangeAsync(tempSets);
                await _cardContext.SaveChangesAsync();
            }

            if (scryCount > 0)
            {
                _logger.LogWarning("LoadSetTempData - scry data already exists, not adding temp records");
            }
            else
            {
                _logger.LogWarning("LoadSetTempData - Adding scryfall placeholder set records...");

                var tempScrySets = parsedPropsBackups.SetCodes.Select(code => new ScryfallSet()
                {
                    Code = code,
                    LastUpdated = null,
                });

                var newSetTasks = tempScrySets.Select(x => _scryRepo.AddScrySet(x));

                await Task.WhenAll(newSetTasks);

                //await _scryContext.Sets.AddRangeAsync(tempScrySets);
                //await _scryContext.SaveChangesAsync();
            }

            _logger.LogWarning("RestoreDb - LoadSetTempData...completed");
        }

        private async Task LoadCardBackups()
        {

            int cardCount = await _cardContext.InventoryCards.Select(x => x.Id).CountAsync();
            if (cardCount > 0)
            {
                _logger.LogWarning("LoadCardBackups - card data already exists, returning");
                return;
            }
            _logger.LogWarning("LoadCardBackups - preparing to load backups...");

            string cardBackupsDataString = await System.IO.File.ReadAllTextAsync(_config.CardBackupLocation);
            List<BackupInventoryCard> parseCardsBackups = JArray.Parse(cardBackupsDataString).ToObject<List<BackupInventoryCard>>();

            _logger.LogWarning("Load Card Backups - ensuring definitions exist (this could take some time)");

            var uniqueMIDs = parseCardsBackups
                .Select(x => x.MultiverseId)
                .Distinct().ToList();

            int idCount = uniqueMIDs.Count();

            _logger.LogWarning($"Load Card Backups - adding {idCount} cards");

            for (int i = 0; i < idCount; i++)
            {
                if (i % 100 == 0)
                {
                    _logger.LogWarning($"RestoreDb - LoadCardBackups...{i} / {idCount} cards loaded");
                }
                await EnsureCardDefinitionExists(uniqueMIDs[i]);
            }

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

            var allVariants = _cardContext.VariantTypes.ToList();

            var mappedInventoryCards = parseCardsBackups.Select(x => new InventoryCard
            {
                InventoryCardStatusId = x.InventoryCardStatusId,
                IsFoil = x.IsFoil,
                MultiverseId = x.MultiverseId,
                VariantTypeId = allVariants.FirstOrDefault(v => v.Name == x.VariantName).Id,
                DeckCards = x.DeckCards.Select(d => new DeckCard()
                {
                    DeckId = d.DeckId,
                    CategoryId = d.Category,
                }).ToList()
            }).ToList();

            await _cardContext.AddRangeAsync(mappedInventoryCards);

            await _cardContext.SaveChangesAsync();

            _logger.LogWarning("RestoreDb - LoadCardBackups...COMPLETE!");

        }

        private async Task RefreshSetData(string setCode)
        {
            _logger.LogError($"Refresh Set Data - Beginning process of refreshing {setCode}");

            //var existingScryfallSet = _scryContext.Sets.Where(x => x.Code == setCode).FirstOrDefault();
            var existingScryfallSet = await _scryRepo.GetScrySetByCode(setCode);

            //get a full set from scryfall
            var setFromScryfall = await _scryRepo.RequestFullScryfallSet(setCode);

            //update all scryfall card data for that set
            //var scryDbSet = _scryContext.Sets.FirstOrDefault(x => x.Code == setCode);

            var tasks = setFromScryfall.Cards.Select(async card =>
            {
                var storedCard = await _scryRepo.GetScryCardById(card.MultiverseId);

                if (storedCard != null)
                {
                    storedCard.StringData = card.Serialize();
                    return _scryRepo.UpdateScryCard(storedCard);
                }
                else
                {
                    var cardToAdd = new ScryfallCard()
                    {
                        MultiverseId = card.MultiverseId,
                        StringData = card.Serialize(),
                        Set = existingScryfallSet
                    };
                    return _scryRepo.AddScryCard(cardToAdd);
                }


                //return _scryRepo.UpdateScryCard()
            });

            await Task.WhenAll(tasks);


            //setFromScryfall.Cards.ForEach(card =>
            //{


            //    var storedCard = _scryContext.Cards.FirstOrDefault(x => x.MultiverseId == card.MultiverseId);
            //    if (storedCard != null)
            //    {
            //        storedCard.StringData = card.Serialize();
            //        //_scryContext.Update(storedCard);
            //        _scryRepo.UpdateScryCard(storedCard);
            //    }
            //    else
            //    {
            //        var cardToAdd = new ScryfallCard()
            //        {
            //            MultiverseId = card.MultiverseId,
            //            StringData = card.Serialize(),
            //            Set = existingScryfallSet
            //        };
            //        _scryRepo.AddScryCard(cardToAdd);
            //        _scryContext.Cards.Add(cardToAdd);
            //    }
            //});


            //update the set definition (name/release date)
            var setDefinition = _cardContext.Sets.FirstOrDefault(x => x.Code == setCode);

            setDefinition.Name = setFromScryfall.Name;
            setDefinition.ReleaseDate = DateTime.Parse(setFromScryfall.ReleaseDate);
            _cardContext.Sets.Update(setDefinition);
            await _cardContext.SaveChangesAsync();

            //update any card definitions for the set
            var cardsToUpdate = await _cardContext.Cards.Where(x => x.SetId == setDefinition.Id).ToListAsync();

            if (cardsToUpdate.Any())
            {
                //update all cards
                var updateTasks = cardsToUpdate.Select(card =>
                {
                    var thisCardFromScryfall = setFromScryfall.Cards.Where(c => c.MultiverseId == card.Id).FirstOrDefault();
                    return _cardRepo.UpdateCardDefinition(thisCardFromScryfall);
                });

                await Task.WhenAll(updateTasks);
            }


            //update the scry set record
            existingScryfallSet.LastUpdated = DateTime.Now;

            await _scryRepo.UpdateScrySet(existingScryfallSet);

            _logger.LogError($"Refresh Set Data - Completed process of refreshing {setCode}");
        }

        #endregion

        #endregion

        public async Task EnsureCardDefinitionExists(int multiverseId)
        {
            var dbCard = await _cardRepo.QueryCardDefinitions().FirstOrDefaultAsync(x => x.Id == multiverseId);

            if (dbCard != null)
            {
                return;
            }

            var scryfallCard = await _scryRepo.GetCardById(multiverseId);

            await _cardRepo.AddCardDefinition(scryfallCard);
            //_logger.LogWarning($"EnsureCardDefinitionExists added {multiverseId} - {scryfallCard.Name}");
        }

    }
}