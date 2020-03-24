using Carpentry.Data.DataContext;
using Carpentry.Data.DataContextLegacy;
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

namespace Carpentry.Data.MigrationTool.Services
{
    public class LegacySetComparer : EqualityComparer<Carpentry.Data.DataContextLegacy.CardSet>
    {
        public override bool Equals(Carpentry.Data.DataContextLegacy.CardSet s1, Carpentry.Data.DataContextLegacy.CardSet s2)
        {
            return (s1.Code == s2.Code);
        }
        public override int GetHashCode(Carpentry.Data.DataContextLegacy.CardSet s)
        {
            return s.Code.GetHashCode();
        }
    }

    public class DataMigrationService //: IDataMigrationService
    {

        private readonly ILogger<DataMigrationService> _logger;

        readonly LegacySqliteDataContext _legacyContext;
        readonly SqliteDataContext _cardContext;
        readonly ScryfallDataContext _scryContext;
        readonly HttpClient _client;

        public DataMigrationService(
            ILoggerFactory loggerFactory,
            LegacySqliteDataContext legacyContext,
            SqliteDataContext cardContext,
            ScryfallDataContext scryContext,
            HttpClient client
            )
        {
            _logger = loggerFactory.CreateLogger<DataMigrationService>();

            _legacyContext = legacyContext;
            _cardContext = cardContext;
            _scryContext = scryContext;
            _client = client;
        }

        #region Public (updated)

        #endregion


        #region Private (updated)


        #endregion

        #region unknown status

        //public DbSet<InventoryCardStatus> CardStatuses { get; set; }
        public void EnsureDbCardStatusesExist()
        {

            //TODO - Actually add some statuses


            //what are the potential card statuses?

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
                    _cardContext.CardStatuses.Add(status);
                }
            });

            _cardContext.SaveChanges();
        }

        //public DbSet<CardRarity> Rarities { get; set; }
        public void EnsureDbRaritiesExist()
        {
            List<Carpentry.Data.DataContext.CardRarity> allRarities = new List<Carpentry.Data.DataContext.CardRarity>()
            {
                new Carpentry.Data.DataContext.CardRarity
                {
                    Id = 'M',
                    Name = "mythic",
                },
                new Carpentry.Data.DataContext.CardRarity
                {
                    Id = 'R',
                    Name = "rare",
                },
                new Carpentry.Data.DataContext.CardRarity
                {
                    Id = 'U',
                    Name = "uncommon",
                },
                new Carpentry.Data.DataContext.CardRarity
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
                    _cardContext.Rarities.Add(rarity);
                }
            });

            _cardContext.SaveChanges();

            //TODO: Remove any non-default values
        }

        public void EnsureDbManaTypesExist()
        {
            /*
            public class ManaType
            {
                public char Id { get; set; }
                public string Name { get; set; }
            }
            */
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
                    _cardContext.ManaTypes.Add(type);
                }
            });

            _cardContext.SaveChanges();

            //TODO: Remove any non-default values
        }

        //public DbSet<MagicFormat> MagicFormats { get; set; }
        public void EnsureDbMagicFormatsExist()
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
                }
            });

            _cardContext.SaveChanges();
        }

        //public DbSet<CardVariantType> VariantTypes { get; set; }
        public void EnsureDbVariantTypesExist()
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
                }
            });

            _cardContext.SaveChanges();
        }

        //Sets
        public void MigrateLegacySets()
        {
            //So a set is just the code that cards are joined with
            //Might as well migrate 100% of the sets in the legacy database

            //It's safe to assume that the set migration is an all-or-nothing process
            //Therefore, if ANY sets exist, there is no reason to perform a migration

            _logger.LogWarning("MigrateLegacySets, checking existing sets");

            if (_cardContext.Sets.Count() > 0)
            {
                _logger.LogWarning("Set data already exists, no migration necessary");
                return;
            }

            //get all sets from legacy database
            //create new sets to add to database

            //TODO: Make sure a scryfall / price update also updates the names of card sets (if name == code)?
            var setsToCreate = _legacyContext.Sets
                .AsEnumerable().Distinct(new LegacySetComparer())
                .Select(x => new Carpentry.Data.DataContext.CardSet
                {
                    Code = x.Code,
                    Name = x.Name
                }).ToList();

            //Save to DB
            _cardContext.Sets.AddRange(setsToCreate);
            _cardContext.SaveChanges();
            _logger.LogWarning("Finished MigrateSets");

        }

        //Cards, ColorIdentities, CardColors, CardLegalities, CardVariants
        public void MigrateLegacyCards()
        {
            //So, this will be a beefy one

            //With the heavy refactor, I don't think I have enough data in a legacy card to build a new one

            //I might just have to get all distinct multiverse IDs of the legacy database, then populate each item in the new DB from scryfall
            //If I do that, it won't have to be an 'all-or-nothing' approach right?



            //Starting by getting all legacy MIDs
            //Id == Muiltiverse Id
            var allLegacyMIDs = _legacyContext.Cards.Select(x => x.Id).ToList();

            var allNewIDs = _cardContext.Cards.Select(x => x.Id).ToList();

            var allIDsToMigrate = allLegacyMIDs.Where(x => !allNewIDs.Contains(x)).ToList();

            var allFormats = _cardContext.MagicFormats.ToList();


            //fuck it, lets just try adding 5 cards

            int migrateCount = allIDsToMigrate.Count;

            _logger.LogWarning($"Beginning to migrate all cards, detected {migrateCount} cards to migrate");

            //allLegacyMIDs.ForEach(legacyId =>
            for (int i = 0; i < migrateCount; i++)
            {
                int legacyId = allIDsToMigrate[i];
                var scryfallDBCard = _scryContext.Cards.Where(x => x.MultiverseId == legacyId).FirstOrDefault();

                var cardAlreadyInDB = _cardContext.Cards.Where(x => x.Id == legacyId).FirstOrDefault();
                if (cardAlreadyInDB != null)
                {
                    _logger.LogWarning($"{cardAlreadyInDB.Name} already exists in the DB, not re-adding [{i}/{migrateCount}]");
                    continue;
                }

                if (scryfallDBCard == null)
                {
                    string errorString = $"Could not find scryfall with multivere ID {legacyId}";
                    _logger.LogError(errorString);
                    throw new Exception(errorString);
                }

                ScryfallMagicCard scryfallCard = JsonConvert.DeserializeObject<ScryfallMagicCard>(scryfallDBCard.StringData);

                //besides just adding the card, also need to add

                //ColorIdentities
                var scryfallCardColorIdentities = scryfallCard.ColorIdentity ?? new List<string>();
                var relevantColorIdentityManaTypes = _cardContext.ManaTypes.Where(x => scryfallCardColorIdentities.Contains(x.Id.ToString())).ToList();

                //CardColors,
                var scryfallCardColors = scryfallCard.Colors ?? new List<string>();
                var relevantColorManaTypes = _cardContext.ManaTypes.Where(x => scryfallCardColors.Contains(x.Id.ToString())).ToList();


                //CardLegalities, 
                var scryCardLegalities = scryfallCard.Legalities.ToList();

                //NOTE - This ends up ignoring any formats that don't actually exist in the DB
                //as of 11-16-2019, this is the desired effect
                var relevantDbFormats = _cardContext.MagicFormats.Where(x => scryCardLegalities.Contains(x.Name)).ToList();

                //CardVariants
                var scryCardVariants = scryfallCard.Variants.Keys.ToList();
                var relevantDBVariantTypes = _cardContext.VariantTypes.Where(x => scryCardVariants.Contains(x.Name)).ToList();


                var dbSet = _cardContext.Sets.First(x => x.Code == scryfallCard.Set);

                char rarity;
                switch (scryfallCard.Rarity)
                {
                    case "mythic":
                        rarity = 'M';
                        break;

                    case "rare":
                        rarity = 'R';
                        break;
                    case "uncommon":
                        rarity = 'U';
                        break;
                    case "common":
                        rarity = 'C';
                        break;
                    default:
                        throw new Exception("Error reading scryfall rarity");

                }

                //Then I could add the actual card?
                var newCard = new Card
                {
                    Id = scryfallCard.MultiverseId,
                    Cmc = scryfallCard.Cmc,
                    ManaCost = scryfallCard.ManaCost,
                    Name = scryfallCard.Name,
                    Text = scryfallCard.Text,
                    Type = scryfallCard.Type,

                    //Set
                    Set = dbSet, //Should this be the ID instead?

                    //Rarity
                    RarityId = rarity,

                    //Color
                    //jank
                    CardColors = relevantColorManaTypes.Select(x => new CardColor
                    {
                        ManaType = x,
                    }).ToList(),


                    //Color Identity
                    CardColorIdentities = relevantColorIdentityManaTypes.Select(x => new Carpentry.Data.DataContext.CardColorIdentity
                    {
                        ManaType = x,
                    }).ToList(),

                    //Variants
                    //still janky
                    Variants = relevantDBVariantTypes.Select(x => new CardVariant
                    {
                        ImageUrl = scryfallCard.Variants[x.Name],
                        Price = scryfallCard.Prices[x.Name],
                        PriceFoil = scryfallCard.Prices[$"{x.Name}_foil"],
                        Type = x,
                    }).ToList(),

                    //Legalities
                    //This feels janky
                    Legalities = relevantDbFormats.Select(x => new CardLegality { Format = x }).ToList(),

                };


                //_logger.LogWarning($"Adding card {newCard.Name} [{i}/{migrateCount}]");

                _cardContext.Cards.Add(newCard);

                //going to save every ___ cards
                if (i % 500 == 0)
                {
                    _logger.LogWarning($"Saving changes [{i}/{migrateCount}]");
                    _cardContext.SaveChangesAsync();
                }

            }
            _cardContext.SaveChangesAsync();
            return;
        }

        //InventoryCards
        public void MigrateLegacyInventoryCards()
        {
            _logger.LogWarning("Beginning MigrateLegacyInventoryCards");
            //I guess this should happen before deck cards

            //This assumes Cards have already been migrated

            //Should this be all-or-nothing?
            //  Why not for now...
            var existingCardCount = _cardContext.InventoryCards.Count();
            if (existingCardCount > 0)
            {
                return;
            }

            //need to migrate everything

            var legacyInventory = _legacyContext.InventoryCards.ToList();

            var normalVariant = _cardContext.VariantTypes.FirstOrDefault(x => x.Name == "normal");

            var mappedItems = legacyInventory.Select(x => new InventoryCard
            {
                Id = x.Id,
                MultiverseId = x.MultiverseId,
                IsFoil = x.IsFoil,
                //status
                InventoryCardStatusId = x.InventoryCardStatusId,

                //variant
                //all legacy cards are "normal" variant
                VariantTypeId = normalVariant.Id,
            });

            _cardContext.AddRange(mappedItems);

            _cardContext.SaveChangesAsync();
            _logger.LogWarning("Finished MigrateLegacyInventoryCards");
            return;

        }

        //Decks
        public void MigrateLegacyDecks()
        {
            _logger.LogWarning("Beginning MigrateDecks");

            if (_cardContext.Decks.Count() > 0)
            {
                _logger.LogWarning("Decks detected, not migrating any legacy decks");
                return;
            }

            //all legacy decks are modern I guess
            var modernFormat = _cardContext.MagicFormats.FirstOrDefault(x => x.Name.ToLower() == "modern");

            var newDecks = _legacyContext.Decks.Select(x => new Deck
            {
                Id = x.Id,
                Name = x.Name,
                Notes = x.Notes,
                Format = modernFormat, //"Modern", #error This should be a FK
                BasicW = x.BasicW,
                BasicU = x.BasicU,
                BasicB = x.BasicB,
                BasicR = x.BasicR,
                BasicG = x.BasicG,
            }).ToList();

            _logger.LogWarning($"Adding {newDecks.Count()} decks");

            _cardContext.Decks.AddRange(newDecks);

            _cardContext.SaveChanges();

            _logger.LogWarning("Finished MigrateDecks");

            return;
        }

        //DeckCards
        public void MigrateLegacyDeckCards()
        {

            _logger.LogWarning("Beginning MigrateLegacyDeckCards");

            if (_cardContext.DeckCards.Any())
            {
                _logger.LogWarning("Existing deck cards detected, won't migrate any legacy deck cards");
                return;
            }

            //Does this give the same error-catching potential as the other approach?
            var mappedDeckCards = _legacyContext.DeckCards.Select(x => new DeckCard
            {
                Id = x.Id,
                DeckId = x.DeckId,
                InventoryCardId = x.InventoryCardId,
            });

            _cardContext.DeckCards.AddRange(mappedDeckCards);

            _cardContext.SaveChangesAsync();

            _logger.LogWarning("Finished MigrateLegacyDeckCards");
            return;
        }

        public async Task UpdateScryfallData()
        {
            var setsToUpdate = _scryContext.Sets.Where(x => x.LastUpdated == null || x.LastUpdated.Value.AddDays(50) < DateTime.Today.Date).ToList();

            string setNames = setsToUpdate.Select(x => x.Code).Aggregate((a, b) => a + ",\n " + b);
            _logger.LogWarning($"Found {setsToUpdate.Count} sets to udpate: {setNames}");

            for (var i = 0; i < setsToUpdate.Count; i++)
            {
                var set = setsToUpdate[i];

                _logger.LogWarning($"Set {set.Code} last updated {set.LastUpdated.ToString()}");

                if (set.LastUpdated == null || set.LastUpdated.Value.AddDays(50) < DateTime.Today.Date)
                {
                    _logger.LogWarning($"Begin Updating {set.Code}");
                    //needs to be updated
                    await ForceUpdateScryfallSet(set.Code);
                    _logger.LogWarning($"Finished Updating {set.Code}");
                }
            }
        }

        public async Task TryUpdateScryfallSet(string setCode)
        {
            var set = _scryContext.Sets.Where(x => x.Code == setCode).FirstOrDefault();
            if (set.LastUpdated == null || set.LastUpdated.Value.AddDays(50) < DateTime.Today.Date)
            {
                await ForceUpdateScryfallSet(set.Code);
            }

        }

        private async Task<List<JToken>> RequestFullScryfallSet(string setCode)
        {
            List<JToken> result = new List<JToken>();

            var setEndpoint = $"https://api.scryfall.com/sets/{setCode}";
            var setResponseString = await _client.GetStringAsync(setEndpoint);

            JObject setResponseObject = JObject.Parse(setResponseString);

            //just because I don't want to wait when looping this cal

            bool searchHasMore = true;
            string cardSearchUri = setResponseObject.Value<string>("search_uri");

            while (searchHasMore)
            {
                await Task.Delay(1000);
                var cardSearchResponse = await _client.GetStringAsync(cardSearchUri);
                JObject cardSearchJobject = JObject.Parse(cardSearchResponse);

                JArray dataBatch = cardSearchJobject.Value<JArray>("data");

                result.AddRange(dataBatch);
                searchHasMore = cardSearchJobject.Value<bool>("has_more");
                if (searchHasMore)
                {
                    cardSearchUri = cardSearchJobject.Value<string>("next_page");
                }
            }

            return result;
        }

        private List<ScryfallMagicCard> MapScryfallDataToCards(List<JToken> cardSearchData)
        {
            try
            {


                _logger.LogWarning("Begin MapScryfallDataToCards");

                List<ScryfallMagicCard> updatedCards = new List<ScryfallMagicCard>();
                List<JToken> specialCards = new List<JToken>();

                //for each card
                cardSearchData.ForEach(card =>
                {
                    try
                    {
                        //does it have at least 1 MID?
                        int? parsedMID = (int?)card.SelectToken("multiverse_ids[0]");

                        if (parsedMID != null)
                        {
                            ScryfallMagicCard cardToAdd = new ScryfallMagicCard();
                            cardToAdd.RefreshFromToken(card);

                            updatedCards.Add(cardToAdd);



                        }
                        else
                        {
                            specialCards.Add(card);
                        }
                    }
                    catch (Exception e)
                    {
                        throw;
                    }
                });

                //a 'special card' is a unique variation of a named card already in the set
                specialCards.ForEach(specialCard =>
                {

                    try
                    {
                        string cardName = specialCard.Value<string>("name");
                        var cardToUpdate = updatedCards.Where(x => x.Name == cardName).FirstOrDefault(); //should this just be First()?
                        if (cardToUpdate != null)
                        {
                            //_logger.LogWarning($"Applying variant to {cardName}");

                            cardToUpdate.ApplyVariant(specialCard);
                        }
                        else
                        {
                            _logger.LogError($"Could not find matching card for special card: {cardName}");
                        }

                    }
                    catch (Exception e)
                    {
                        throw;
                    }
                });

                _logger.LogWarning("Completed MapScryfallDataToCards");



                return updatedCards;

            }
            catch (Exception e)
            {
                throw;
            }
        }

        public async Task ForceUpdateScryfallSet(string setCode)
        {

            var thisSet = _scryContext.Sets.Where(x => x.Code == setCode).FirstOrDefault();

            if (thisSet == null)
            {
                ScryfallSet newSet = new ScryfallSet()
                {
                    Code = setCode,
                    LastUpdated = null
                };

                _scryContext.Sets.Add(newSet);
                thisSet = newSet;
            }

            //Get the set data from scryfall
            List<JToken> cardSearchData = await RequestFullScryfallSet(setCode);

            //make sure Last Updated is up to date
            thisSet.LastUpdated = DateTime.Now.Date;

            //Parse the scryfall data
            List<ScryfallMagicCard> updatedCards = MapScryfallDataToCards(cardSearchData);

            #region Save everything to the DB

            updatedCards.ForEach(item =>
            {
                try
                {
                    var storedCard = _scryContext.Cards.FirstOrDefault(x => x.MultiverseId == item.MultiverseId);
                    if (storedCard != null)
                    {
                        storedCard.StringData = item.Serialize();
                        //_scryContext.Update(storedCard);
                    }
                    else
                    {
                        var cardToAdd = new ScryfallCard()
                        {
                            MultiverseId = item.MultiverseId,
                            StringData = item.Serialize(),
                            Set = thisSet
                        };
                        _scryContext.Cards.Add(cardToAdd);
                    }
                }
                catch (Exception ex)
                {
                    throw;
                }
            });
            //_scryContext.Sets.Update(thisSet);

            //do I need to be calling .Update() on everything??
            await _scryContext.SaveChangesAsync();

            #endregion

        }

        public void ClearDb()
        {
            _cardContext.Database.EnsureDeleted();
            _cardContext.Database.EnsureCreated();
        }
        //public void GetAllScryfallSetNames()s

        #endregion

        #region legacy methods

        private async Task ReloadDbFromCards()
        {
            throw new NotImplementedException();
            //    try
            //    {
            //        await ClearDb();

            //        string savedCards = System.IO.File.ReadAllText(@"C:\DotNet\Carpentry\Carpentry\CardBackups.txt");

            //        var parsedCardsObj = JObject.Parse(savedCards);

            //        List<Data.Card> parsedCards = parsedCardsObj["cards"].ToObject<List<Data.Card>>();

            //        List<string> storedSetCodes = new List<string>();

            //        //fuck it
            //        //loading just known sets
            //        List<string> setsToLoad = new List<string>()
            //        {
            //            "mh1",
            //            "m20",//,
            //            "war"
            //        };

            //        //need to make sure we don't throw too much at the API
            //        for (int i = 0; i < setsToLoad.Count(); i++)
            //        {
            //            await EnsureSetExistsLocally(setsToLoad[i]);
            //            await Task.Delay(1000);
            //        }


            //        //for each card
            //        parsedCards.ForEach(card =>
            //        {
            //            //I need to check if I should load the set

            //            //if(!storedSetCodes.Contains(card.))
            //            var thisDBCard = _context.CardDetails.FirstOrDefault(x => x.MultiverseId == card.MultiverseId);
            //            card.Data = thisDBCard;
            //            _context.Cards.Add(card);
            //        });


            //        await _context.SaveChangesAsync();
            //    }
            //    catch (Exception ex)
            //    {
            //        throw ex;
            //    }

        }

        private async Task LoadDb()
        {
            throw new NotImplementedException();
            //    try
            //    {
            //        await ReloadDbFromCards();
            //        return;


            //        string savedText = System.IO.File.ReadAllText(@"C:\DotNet\Carpentry\Carpentry\InventoryDbBackup.txt");

            //        var parsedStore = JObject.Parse(savedText);

            //        var parsedCards = parsedStore["cards"].ToObject<List<Data.Card>>();
            //        var parsedCardData = parsedStore["data"].ToObject<List<Data.CardDetail>>();
            //        //var parsedSets = parsedStore["sets"].ToObject<List<S4SetDetail>>();

            //        //Data.SetDetail mh1Set = new Data.SetDetail()
            //        //{
            //        //    Code = "MH1",
            //        //    StringData = "{}"
            //        //};



            //        parsedCards.ForEach((card) =>
            //        {
            //            card.Data = parsedCardData.FirstOrDefault(x => x.MultiverseId == card.MultiverseId);
            //        });

            //        List<SetDetail> newSetList = new List<SetDetail>();

            //        parsedCardData.ForEach(detail =>
            //        {
            //            //detail.SetDetail = mh1Set;
            //            //this will be a pain...

            //            //parse this card data (card data doesn't hold sets yet)
            //            //MagicCardDto thisCard = new MagicCardDto(detail.StringData);
            //            MagicCardDto thisCard = JObject.Parse(detail.StringData).ToObject<MagicCardDto>();

            //            detail.SetCode = thisCard.Set;

            //            var setInList = newSetList.FirstOrDefault(x => x.Code == thisCard.Set);
            //            if (setInList == null)
            //            {
            //                setInList = new SetDetail()
            //                {
            //                    Code = thisCard.Set,
            //                    StringData = null,
            //                    LastUpdated = null
            //                };

            //                newSetList.Add(setInList);
            //            }
            //            //else
            //            //{

            //            //}
            //            detail.SetDetail = setInList;
            //            //setInList.Cards.Add(detail);

            //            //  actually not the biggest pain
            //        });

            //        await ClearDb();


            //        //await _context.Sets.AddAsync(mh1Set);
            //        await _context.Sets.AddRangeAsync(newSetList);

            //        await _context.CardDetails.AddRangeAsync(parsedCardData);
            //        await _context.Cards.AddRangeAsync(parsedCards);

            //        await _context.SaveChangesAsync();
            //    }
            //    catch (Exception ex)
            //    {
            //        throw;
            //    }
        }

        #endregion
    }
}
