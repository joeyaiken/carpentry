using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Carpentry.Data.DataContext;
using Carpentry.Data.Interfaces;
using Carpentry.Data.MigrationTool.Models;
using Carpentry.Interfaces;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;

namespace Carpentry.Data.MigrationTool.Services
{
    
    class DataRestoreService
    {
        private readonly ILogger<DataRestoreService> _logger;
        private readonly SqliteDataContext _cardContext;
        private readonly ICardStringRepo _scryRepo;
        private readonly MigrationToolConfig _config;
        private readonly ScryfallDataContext _scryContext;

        private readonly ICarpentryService _carpentryService;

        public DataRestoreService(
            ILoggerFactory loggerFactory,
            SqliteDataContext cardContext,
            ICardStringRepo scryRepo,
            MigrationToolConfig config,
            ScryfallDataContext scryContext,
            ICarpentryService carpentryService
            )
        {
            _logger = loggerFactory.CreateLogger<DataRestoreService>();
            _cardContext = cardContext;
            _config = config;
            _scryRepo = scryRepo;
            _scryContext = scryContext;
            _carpentryService = carpentryService;


        }

        public async Task RestoreDb()
        {
            _logger.LogWarning("DataRestoreService - RestoreDb...");
            //Okay so what's the general process?

            //Stary by ensuring a DB actually exists
            await _cardContext.Database.EnsureCreatedAsync();
            await _scryContext.Database.EnsureCreatedAsync();

            //ensure default records exist
            await EnsureDefaultRecordsExist();

            //load all decks
            string deckBackupsDataString = System.IO.File.ReadAllText(_config.DeckBackupLocation);
            List<BackupDeck> parsedDecks = JArray.Parse(deckBackupsDataString).ToObject<List<BackupDeck>>();
            await LoadDeckBackups(parsedDecks);

            //look at props, load all sets in props
            string propsBackupDataString = System.IO.File.ReadAllText(_config.PropsBackupLocation);
            BackupDataProps parsedPropsBackups = JObject.Parse(propsBackupDataString).ToObject<BackupDataProps>();
            await LoadScrySetBackups(parsedPropsBackups);
            await LoadCardSetBackups(parsedPropsBackups);

            //load card backups, adding deck cards along the way
            string cardBackupsDataString = System.IO.File.ReadAllText(_config.CardBackupLocation);
            List<BackupInventoryCard> parseCardsBackups = JArray.Parse(cardBackupsDataString).ToObject<List<BackupInventoryCard>>();
            await LoadCardBackups(parseCardsBackups);

            _logger.LogWarning("DataRestoreService - RestoreDb...Completed!");
        }

        #region private methods

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
            List<Carpentry.Data.DataContext.ManaType> allManaTypes = new List<Carpentry.Data.DataContext.ManaType>()
            {
                new Carpentry.Data.DataContext.ManaType{
                    Id = 'W',
                    Name = "White"
                },
                new Carpentry.Data.DataContext.ManaType{
                    Id = 'U',
                    Name = "Blue"
                },
                new Carpentry.Data.DataContext.ManaType{
                    Id = 'B',
                    Name = "Black"
                },
                new Carpentry.Data.DataContext.ManaType{
                    Id = 'R',
                    Name = "Red"
                },
                new Carpentry.Data.DataContext.ManaType{
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

        private async Task LoadDeckBackups(List<BackupDeck> parsedBackupDecks)
        {
            int existingDeckCount = _cardContext.Decks.Select(x => x.Id).Count();
            if(existingDeckCount > 0)
            {
                _logger.LogWarning("LoadDeckBackups - Decks already exist, not loading anything from parsed data");
                return;
            }
            _logger.LogWarning("LoadDeckBackups - Loading parsed decks");

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

        private async Task LoadScrySetBackups(BackupDataProps parsedProps)
        {
            //also assuming this is all-or-nothing?

            //TODO / NOTE: This should be refactored, it should insert basic sets into the scryfall DB, with null dates, then call whatever updates prices

            int setCount = _scryContext.Sets.Select(x => x.Id).Count();
            if (setCount > 0)
            {
                _logger.LogWarning("RestoreDb - scry data already exists, returning");
                return;
            }

            _logger.LogWarning("RestoreDb - LoadScrySetBackups...");
            for(int i = 0; i < parsedProps.SetCodes.Count(); i++)
            {
                _logger.LogWarning($"RestoreDb - LoadScrySetBackups...checking {parsedProps.SetCodes[i]}");
                await _scryRepo.EnsureSetExistsLocally(parsedProps.SetCodes[i]);
                await Task.Delay(100);
            }

            _logger.LogWarning("RestoreDb - LoadScrySetBackups...completed");
        }

        private async Task LoadCardSetBackups(BackupDataProps parsedProps)
        {
            int setCount = _cardContext.Sets.Select(x => x.Id).Count();
            
            if (setCount > 0)
            {
                _logger.LogWarning("RestoreDb - set data already exists, returning");
                return;
            }
            _logger.LogWarning("RestoreDb - LoadCardSetBackups...");
            var mappedSets = _scryContext.Sets.Select(x => new DataContext.CardSet()
            {
                Code = x.Code,
                Name = x.Code,
            });

            await _cardContext.Sets.AddRangeAsync(mappedSets);
            await _cardContext.SaveChangesAsync();
            _logger.LogWarning("RestoreDb - LoadCardSetBackups...completed");
        }

        private async Task LoadCardBackups(List<BackupInventoryCard> parsedInventoryCards)
        {
            //will be adding both deck and inventory cards in the same pass
            int cardCount = _cardContext.Cards.Select(x => x.Id).Count();

            if (cardCount > 0)
            {
                _logger.LogWarning("RestoreDb - card data already exists, returning");
                return;
            }
            _logger.LogWarning("RestoreDb - LoadCardBackups...");


            var allVariants = _cardContext.VariantTypes.ToList();



            var mappedInventoryCards = parsedInventoryCards.Select(x => new InventoryCard
            {
                InventoryCardStatusId = x.InventoryCardStatusId,
                IsFoil = x.IsFoil,
                MultiverseId = x.MultiverseId,
                VariantTypeId = allVariants.FirstOrDefault(v => v.Name == x.VariantName).Id,
                DeckCards = x.DeckCards.Select(d => new DeckCard()
                {
                    DeckId = d.DeckId,
                    CategoryId = null,// TODO - replace with actual val from backup
                }).ToList()
            }).ToList();

            _logger.LogWarning("RestoreDb - LoadCardBackups...ensuring definitions exist");

            var uniqueMIDs = mappedInventoryCards
                .Select(x => x.MultiverseId)
                .Distinct().ToList();

            int idCount = uniqueMIDs.Count();

            _logger.LogWarning($"RestoreDb - LoadCardBackups...adding {idCount} cards");

            for (int i = 0; i < idCount; i++)
            {
                if(i % 100 == 0)
                {
                    _logger.LogWarning($"RestoreDb - LoadCardBackups...{i} / {idCount} cards loaded");
                }
                await _carpentryService.EnsureCardDefinitionExists(uniqueMIDs[i]);
            }


            var uniqueMidTasks = mappedInventoryCards
                .Select(x => x.MultiverseId)
                .Distinct()
                .Select(x => _carpentryService.EnsureCardDefinitionExists(x));

            await Task.WhenAll(uniqueMidTasks);
            _logger.LogWarning("RestoreDb - LoadCardBackups...definitions exist, saving");
            await _cardContext.AddRangeAsync(mappedInventoryCards);

            await _cardContext.SaveChangesAsync();
            _logger.LogWarning("RestoreDb - LoadCardBackups...COMPLETE!");



            //_cardContext.






        }

        #endregion


        #endregion

    }
}
