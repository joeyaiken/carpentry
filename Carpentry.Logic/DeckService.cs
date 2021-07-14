using Carpentry.Logic.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Net;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Carpentry.CarpentryData;
using Carpentry.DataLogic;
using Carpentry.DataLogic.QueryResults;
using Carpentry.CarpentryData.Models;

namespace Carpentry.Logic
{
    public interface IDeckService
    {
        Task<int> AddDeck(DeckPropertiesDto props);
        Task AddImportedDeckBatch(List<DeckPropertiesDto> decks);
        Task UpdateDeck(DeckPropertiesDto props);
        Task DeleteDeck(int deckId);

        Task DissassembleDeck(int deckId);
        Task<int> CloneDeck(int deckId);

        Task AddDeckCard(DeckCardDto dto);
        Task AddDeckCardBatch(IEnumerable<DeckCardDto> dto);
        Task UpdateDeckCard(DeckCardDto card);
        Task DeleteDeckCard(int deckCardId);

        Task<CardTagDetailDto> GetCardTagDetails(int deckId, int cardId);
        Task AddCardTag(CardTagDto cardTag);
        Task RemoveCardTag(int cardTagId);

        Task<List<DeckOverviewDto>> GetDeckOverviews(string format = null, string sortBy = null, bool includeDissasembled = false);
        Task<DeckDetailDto> GetDeckDetail(int deckId);

        Task<string> GetDeckListExport(int deckId, string exportType);


        //Task<ValidatedDeckImportDto> ValidateDeckImport(CardImportDto dto);
        //Task AddValidatedDeckImport(ValidatedDeckImportDto validatedDto);
        //Task<string> ExportDeckList(int deckId);
    }

    class CardLine
    {
        public string Category { get; set; }
        public string CardString { get; set; }
    }

    public class DeckService : IDeckService
    {
        //Previous line of thinking was that this should never see base database classes
        //That idea was dumb, and just overly-complicated things


        //-----------


        //Question: Why shouldn't this have access to a repo?
        //  Abstracting this away just makes things more confusing, really
        //  I can still have things that make life easier in the data-layer

        //Answer (maybe): Abstracting it away makes it easier for me to possibly switch to a different database
        //  Counterpoint: That's what Entity Framework does for me anyways...

        //All methods should return a model specific to THIS project, not the data project (evevntually)

        //What if all data layer models were either
        //1 -   A DB/DataContext model
        //2 -   A DTO that contains either IDs or values but not the associations

        //Should have no access to data context classes, only repo classes
        private readonly ILogger<DeckService> _logger;

        private readonly CarpentryDataContext _cardContext;

        private readonly DeckDataRepo _deckRepo;
        private readonly IInventoryDataRepo _inventoryRepo;
        private readonly IInventoryService _inventoryService;
        public readonly ICoreDataRepo _coreDataRepo;
        private readonly ICardDataRepo _cardDataRepo;

        private static string _sideboardCategory = "Sideboard";

        private static readonly int _availability_InDeck = 1;
        private static readonly int _availability_InInventory = 2;
        private static readonly int _availability_InOtherDeck = 3;
        private static readonly int _availability_Unowned = 4;

        //public ICardImportService _cardImportService;

        public DeckService(
            DeckDataRepo deckRepo,
            IInventoryDataRepo inventoryRepo,
            IInventoryService inventoryService,
            ILogger<DeckService> logger,
            ICoreDataRepo coreDataRepo,
            ICardDataRepo cardDataRepo

            //fuck it, adding DB context directly
            , CarpentryDataContext cardContext

            //ICardImportService cardImportService
            )
        {
            _deckRepo = deckRepo;
            _inventoryRepo = inventoryRepo;
            _inventoryService = inventoryService;
            _logger = logger;
            _coreDataRepo = coreDataRepo;
            _cardDataRepo = cardDataRepo;
            //_cardImportService = cardImportService;
            _cardContext = cardContext;
        }

        #region private methods

        //        private async Task<decimal?> CalculateInventoryTotalPrice()
        //        {
        //            //Should this refresh scryfall prices?
        //            //TODO - refresh scryfall prices when querying this


        //            var priceQuery = _cardRepo.QueryInventoryCards() //_cardContext.DeckCards
        //                .Select(x => new
        //                {
        //                    x.Id,
        //                    x.Card.Variants.Where(cardVariant => cardVariant.CardVariantTypeId == x.VariantTypeId).FirstOrDefault().Price,
        //                });

        //            var totalPrice = await priceQuery.SumAsync(x => x.Price);

        //            return totalPrice;
        //        }


        /*
        Okay this is a dumb approach
        The goal is to get all the details needed for the "Deck Stats" component
        This includes:
            Total mainboard cards
            cmc breakdown of mainboard cards
            type breakdown of mainboard cards
            deck color identity (including sideboard)
            //color breakdown of mainboard cards

            Price (should this include sideboard?)


        Using the whole approach of multiple sub functions is kinda dumb, it ends up querying the same data multiple times
        Instead, it should get the data for each deck card, and process everything in-memory

        Why am I even re-querying anything? I should be able to grab anything I need 
        */
        private async Task<DeckStatsDto> GetDeckStats(int deckId)
        {
            //DeckStatsDto result = new DeckStatsDto();

            var deckStats = new DeckStatsDto();


            var dbDeck = _cardContext.Decks.Where(d => d.DeckId == deckId)
                .Include(d => d.Cards)
                .FirstOrDefault();

            //deckList[i].Colors = await _queryService.GetDeckColorIdentity(deckList[i].Id);

            var colorChars = await _deckRepo.GetDeckColorIdentity(deckId);

            deckStats.ColorIdentity = colorChars.Select(x => x.ToString()).ToList();

            //string validationResults = await ValidateDeck(deckList[i].Id);
            string validationResults = await ValidateDeck(deckId);

            if (string.IsNullOrEmpty(validationResults))
            {
                //deckList[i].IsValid = true;
                deckStats.IsValid = true;
            }
            else
            {
                deckStats.ValidationIssues = validationResults;
            }


            deckStats.IsDisassembled = !dbDeck.Cards.Where(dc => dc.InventoryCardId != null).Any();

            //get deck cards

            //Total mainboard cards

            //cmc breakdown of mainboard cards

            //type breakdown of mainboard cards

            //deck color identity (including sideboard)

            //Price (should this include sideboard?)

            return deckStats;
        }

        private async Task<DeckStatsDto> GetDeckStats_legacy(int deckId)
        {
            DeckStatsDto result = new DeckStatsDto();

            //total Count
            result.TotalCount = await _deckRepo.GetDeckCardCount(deckId);

            var statData = await _deckRepo.GetDeckCardStats(deckId);

            //total price
            decimal? totalPrice = statData.Sum(x => x.Price);
            result.TotalCost = totalPrice ?? 0;

            //
            //var priceQuery = _cardRepo.QueryDeckCards() //_cardContext.DeckCards
            //    .Where(x => x.DeckId == deckId && x.CategoryId != 's')
            //    .Select(x => new
            //    {
            //        CardName = x.InventoryCard.Card.Name,
            //        //CardVariantName = x.InventoryCard.VariantType.Name,
            //        //VariantTypeId = x.InventoryCard.VariantTypeId,
            //        ////price
            //        Price = x.InventoryCard.Card.Variants.Where(cardVariant => cardVariant.CardVariantTypeId == x.InventoryCard.VariantTypeId).FirstOrDefault().Price,

            //    });

            //var totalPrice = await priceQuery.SumAsync(x => x.Price);



            //type-breakdown

            List<string> cardTypes = statData
                .Select(x => GetCardTypeGroup(x.Type))
                .ToList();


            //var cardTypes = _cardRepo.QueryDeckCards()
            //    .Where(x => x.DeckId == deckId && x.CategoryId != 's')
            //    .Select(x => x.InventoryCard.Card.Type)
            //    .Select(x => GetCardTypeGroup(x))
            //    .ToList();

            var deck = await _deckRepo.GetDeckById(deckId);
            int basicLandCount = deck.BasicW + deck.BasicU + deck.BasicB + deck.BasicR + deck.BasicG;


            var typeCountsDict = cardTypes
                .GroupBy(x => x)
                .Select(x => new
                {
                    Name = x.Key,
                    Count = x.Count()
                })
                .AsEnumerable()
                .ToDictionary(x => x.Name, x => x.Count);

            if (typeCountsDict.Keys.Contains("Lands"))
            {
                typeCountsDict["Lands"] = typeCountsDict["Lands"] + basicLandCount;
            }
            else
            {
                typeCountsDict["Lands"] = basicLandCount;
            }

            result.TypeCounts = typeCountsDict;

            //this is for the CMC breakdown
            Dictionary<string, int> deckCardCostsDict = statData
                .Where(x => x.CategoryId != 's' && !x.Type.Contains("Land"))
                .Select(x => x.Cmc)
                .GroupBy(x => x)
                .Select(x => new
                {
                    Cmc = x.Key,
                    Count = x.Count(),
                })
                .OrderBy(x => x.Cmc)
                .ToDictionary(x => x.Cmc.ToString(), x => x.Count);

            result.CostCounts = deckCardCostsDict;


            //deck color identity
            //all of the basic lands 
            //+ every card's color identity
            //TODO - This doesn't actually include basic lands, should it?

            var deckColorIdentity = statData
                .SelectMany(card => card.ColorIdentity)
                .Distinct().ToList();


            //var cardCIQuery = _cardRepo.QueryDeckCards() //_cardContext.DeckCards
            //    .Where(x => x.DeckId == deckId)
            //    .Select(x => x.InventoryCard.Card)
            //    .SelectMany(card => card.CardColorIdentities.Select(ci => ci.ManaTypeId))
            //    .Distinct();

            result.ColorIdentity = deckColorIdentity;

            return result;
        }

        private static string GetCardTypeGroup(string cardType)
        {
            if (cardType == null)
            {
                return null;
            }
            if (cardType.ToLower().Contains("creature"))
            {
                return "Creatures";
            }
            else if (cardType.ToLower().Contains("land"))
            {
                return "Lands";
            }
            else if (cardType.ToLower().Contains("planeswalker"))
            {
                return "Planeswalkers";
            }
            else if (cardType.ToLower().Contains("enchantment"))
            {
                return "Enchantments";
            }
            else if (cardType.ToLower().Contains("artifact"))
            {
                return "Artifacts";
            }
            //else if (cardType.ToLower().Contains(""))
            //{
            //    return "";
            //}
            //else if (cardType.ToLower().Contains(""))
            //{
            //    return "";
            //}
            else
            {
                return "Spells";
            }
        }

        //        public async Task EnsureCardDefinitionExists(int multiverseId)
        //        {
        //            var dbCard = await _cardRepo.QueryCardDefinitions().FirstOrDefaultAsync(x => x.Id == multiverseId);

        //            if (dbCard != null)
        //            {
        //                return;
        //            }

        //            var scryfallCard = await _scryRepo.GetCardById(multiverseId);

        //            await _cardRepo.AddCardDefinition(scryfallCard);
        //            //_logger.LogWarning($"EnsureCardDefinitionExists added {multiverseId} - {scryfallCard.Name}");
        //        }

        //        private static IQueryable<ScryfallMagicCard> MapInventoryQueryToScryfallDto(IQueryable<Data.DataContext.Card> query)
        //        {
        //            IQueryable<ScryfallMagicCard> result = query.Select(card => new ScryfallMagicCard()
        //            {
        //                Cmc = card.Cmc,
        //                ManaCost = card.ManaCost,
        //                MultiverseId = card.Id,
        //                Name = card.Name,

        //                //Prices = card.Variants.ToDictionary(v => (v.)  )

        //                Prices = card.Variants.SelectMany(x => new[]
        //                {
        //                    new {
        //                        Name = x.Type.Name,
        //                        Price = x.Price,
        //                    },
        //                    new {
        //                        Name = $"{x.Type.Name}_foil",
        //                        Price = x.PriceFoil,
        //                    }
        //                }).ToDictionary(v => v.Name, v => v.Price),

        //                //Variants = card.Variants.ToDictionary(v => v.Type.Name, v => v.ImageUrl),
        //                Variants = card.Variants.Select(v => new { v.Type.Name, v.ImageUrl }).ToDictionary(v => v.Name, v => v.ImageUrl),
        //                Colors = card.CardColors.Select(c => c.ManaType.Name).ToList(),
        //                Rarity = card.Rarity.Name,
        //                Set = card.Set.Code,
        //                Text = card.Text,
        //                Type = card.Type,
        //                ColorIdentity = card.CardColorIdentities.Select(i => i.ManaType.Name).ToList(),
        //                Legalities = card.Legalities.Select(l => l.Format.Name).ToList(),
        //            });
        //            return result;
        //        }

        private async Task<string> ValidateDeck(int deckId)
        {
            List<string> validationErrors = new List<string>();

            //var deck = await _cardRepo.QueryDeckProperties().Where(x => x.Id == deckId).FirstOrDefaultAsync();
            var deck = await _deckRepo.GetDeckById(deckId);

            string deckFormat = deck.Format.Name.ToLower();

            #region Validate deck size

            int deckSize = await _deckRepo.GetDeckCardCount(deckId);

            int deckMinimum = 60;
            int? deckLimit = null;
            
            switch (deckFormat)
            {
                case "commander":
                    deckMinimum = 100;
                    deckLimit = 100;
                    break;

                case "jumpstart":
                    deckMinimum = 20;
                    deckLimit = 20;
                    break;

                case "oathbreaker":
                    deckLimit = 60;
                    break;

                case "brawl":
                    deckLimit = 60;
                    break;
            }

            if(deckSize < deckMinimum)
            {

                validationErrors.Add($"Below size requirement: {deckSize}/{deckMinimum} cards");
            }

            if(deckLimit.HasValue && deckSize > deckLimit)
            {
                validationErrors.Add($"Above size limit: {deckSize}/{deckLimit} cards");

            }

            #endregion

            #region Validate max per card (1 for singleton, 4 for other formats)



            #endregion

            #region Validate format legality



            #endregion

            #region Validate color rules for commander/brawl



            #endregion

            var validationResult = string.Join(" ", validationErrors);
            return validationResult;
        }

        //private static DeckPropertiesDto MapDeckDataToProperties(DeckData dbDeck)
        //{
        //    DeckPropertiesDto mappedDeck = new DeckPropertiesDto()
        //    {
        //        Id = dbDeck.Id,
        //        BasicB = dbDeck.BasicB,
        //        BasicG = dbDeck.BasicG,
        //        BasicR = dbDeck.BasicR,
        //        BasicU = dbDeck.BasicU,
        //        BasicW = dbDeck.BasicW,
        //        FormatId = dbDeck.Format.Id
        //    };
        //    return mappedDeck;
        //}


        #endregion

        #region Public methods

        #region Deck Props

        public async Task<int> AddDeck(DeckPropertiesDto props)
        {
            DataReferenceValue<int> deckFormat = await _coreDataRepo.GetMagicFormat(props.Format);

            var newDeck = new DeckData()
            {
                Name = props.Name,
                MagicFormatId = deckFormat.Id,
                Notes = props.Notes,

                BasicW = props.BasicW,
                BasicU = props.BasicU,
                BasicB = props.BasicB,
                BasicR = props.BasicR,
                BasicG = props.BasicG,
            };

            int newId = await _deckRepo.AddDeck(newDeck);

            return newId;
        }

        /// <summary>
        /// This method allows the Data Import Service to use the Import Service and not the Deck Data Repo
        /// </summary>
        /// <param name="decks"></param>
        /// <returns></returns>
        public async Task AddImportedDeckBatch(List<DeckPropertiesDto> decks)
        {
            var allFormats = await _coreDataRepo.GetAllMagicFormats();

            var newDecks = decks.Select(props => new DeckData()
            {
                DeckId = props.Id,
                Name = props.Name,
                MagicFormatId = allFormats.Where(f => f.Name.ToLower() == props.Format.ToLower()).FirstOrDefault().Id,
                Notes = props.Notes,

                BasicW = props.BasicW,
                BasicU = props.BasicU,
                BasicB = props.BasicB,
                BasicR = props.BasicR,
                BasicG = props.BasicG,
            }).ToList();

            await _deckRepo.AddImportedDeckBatch(newDecks);
        }

        public async Task UpdateDeck(DeckPropertiesDto deckDto)
        {
            DeckData existingDeck = await _deckRepo.GetDeckById(deckDto.Id);

            if (existingDeck == null)
            {
                throw new Exception("No deck found matching the specified ID");
            }

            DataReferenceValue<int> deckFormat = await _coreDataRepo.GetMagicFormat(deckDto.Format);

            existingDeck.Name = deckDto.Name;
            existingDeck.MagicFormatId = deckFormat.Id;
            existingDeck.Notes = deckDto.Notes;
            existingDeck.BasicW = deckDto.BasicW;
            existingDeck.BasicU = deckDto.BasicU;
            existingDeck.BasicB = deckDto.BasicB;
            existingDeck.BasicR = deckDto.BasicR;
            existingDeck.BasicG = deckDto.BasicG;

            await _deckRepo.UpdateDeck(existingDeck);
        }

        public async Task DeleteDeck(int deckId)
        {
            await _deckRepo.DeleteDeck(deckId);
        }


        public async Task DissassembleDeck(int deckId)
        {
            //get all cards in the deck
            var existingDeckCards = await _deckRepo.GetDeckCardsByDeckId(deckId);

            //itterate over each deck card, clearing inventoryCardId
            foreach(var deckCard in existingDeckCards)
            {
                deckCard.InventoryCardId = null;
            }

            await _deckRepo.UpdateDeckCardBatch(existingDeckCards);

            var deck = await _cardContext.Decks.FirstOrDefaultAsync(d => d.DeckId == deckId);
            deck.Stats = null;
            _cardContext.Update(deck);
            await _cardContext.SaveChangesAsync();
        }
        public async Task<int> CloneDeck(int deckId)
        {
            //get deck detail
            var existingDeck = await _deckRepo.GetDeckById(deckId);
            //copy deck props

            var newDeck = new DeckData()
            {
                Name = $"Clone of {existingDeck.Name}",
                MagicFormatId = existingDeck.MagicFormatId,
                Notes = existingDeck.Notes,
                BasicW = existingDeck.BasicW,
                BasicU = existingDeck.BasicU,
                BasicB = existingDeck.BasicB,
                BasicR = existingDeck.BasicR,
                BasicG = existingDeck.BasicG,
                Tags = existingDeck.Tags?.Select(tag => new DeckCardTagData
                {
                    CardName = tag.CardName,
                    Description = tag.Description,
                }).ToList(),
                Cards = existingDeck.Cards.Select(card => new DeckCardData
                {
                    CardName = card.CardName,
                    CategoryId = card.CategoryId,
                    InventoryCardId = null,
                }).ToList(),
            };

            //copy deck cards
            //add deck
            var newId = await _deckRepo.AddDeck(newDeck);
            //add cards in bulk

            return newId;

            //throw new NotImplementedException();



        }

        #endregion Deck Props

        #region Deck Cards

        /// <summary>
        /// Adds a card to a deck
        /// If the dto references an existing deck card, and that card is ALREADY in a deck, no new card is added
        ///     Instead, the existing card is moved to this deck
        /// If the InventoryCardId AND CardId are both null, an empty deck card is created
        /// Otherwise, if a CardId is specified, then a new inventory card is created
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task AddDeckCard(DeckCardDto dto)
        {
            //InventoryCardId != 0: Select/Move inventory card
            //Don't need to add an inventory card
            if (dto.InventoryCardId != null && dto.InventoryCardId > 0)
            {
                //if a card already exists in a deck it is "moved" to this deck
                var existingDeckCard = await _deckRepo.GetDeckCardByInventoryId(dto.InventoryCardId.Value);

                if (existingDeckCard != null)
                {
                    existingDeckCard.DeckId = dto.DeckId;
                    existingDeckCard.CategoryId = null;

                    await _deckRepo.UpdateDeckCard(existingDeckCard);
                    return;
                }

                //nothing needs to be created, the deck and inventory cards already exist
            }
            else
            {
                //InventoryCardId == 0 && CardId == 0: Add empty card
                if (dto.CardId == 0)
                {
                    dto.InventoryCardId = null;
                }
                //InventoryCardId == 0 && CardId != 0: Add new inventory card
                else
                {
                    var newInventoryCard = new InventoryCardDto()
                    {
                        CardId = dto.CardId,
                        IsFoil = dto.IsFoil,
                        StatusId = dto.InventoryCardStatusId, //1 == in inventory
                    };

                    int newInventoryCardId = await _inventoryService.AddInventoryCard(newInventoryCard);
                    dto.InventoryCardId = newInventoryCardId;
                }
            }

            //Need to add a new inventory card for this deck card
            //(check how I'm adding inventory cards atm)
            //if (dto.InventoryCardId == 0)
            //{
                
            //}

            DeckCardData cardToAdd = new DeckCardData()
            {
                DeckId = dto.DeckId,
                CardName = dto.CardName,

                CategoryId = dto.CategoryId,
                
                InventoryCardId = dto.InventoryCardId,
            };

            await _deckRepo.AddDeckCard(cardToAdd);
            //await _cardRepo.AddDeckCard(dto);
        }

        //I think I only end up adding NEW deck cards with a batch
        //IDR where exactly this gets called outside of console apps
        public async Task AddDeckCardBatch(IEnumerable<DeckCardDto> dtoBatch)
        {
            _logger.LogWarning("Beginning AddDeckCardBatch");
            var dtoArray = dtoBatch.ToArray();

            for (int i = 0; i < dtoArray.Count(); i++)
            {
                DeckCardDto dto = dtoArray[i];

                //console apps want to see this
                //_logger.LogWarning($"Adding card #{i} MID {dto.InventoryCard.MultiverseId}");

                await AddDeckCard(dto);
            }
        }

        public async Task UpdateDeckCard(DeckCardDto card)
        {
            DeckCardData dbCard = await _deckRepo.GetDeckCardById(card.Id);

            dbCard.DeckId = card.DeckId;
            dbCard.CategoryId = card.CategoryId;
            dbCard.InventoryCardId = card.InventoryCardId;

            await _deckRepo.UpdateDeckCard(dbCard);
        }

        public async Task DeleteDeckCard(int deckCardId)
        {
            await _deckRepo.DeleteDeckCard(deckCardId);
        }

        #endregion Deck Cards

        #region Card Tags

        public async Task<CardTagDetailDto> GetCardTagDetails(int deckId, int cardId)
        {
            var result = new CardTagDetailDto() { CardId = cardId };

            //get name by ID
            //var matchingCard = await _cardDataRepo.GetCardData(cardId);
            var matchingCard = await _cardContext.Cards.FirstOrDefaultAsync(c => c.CardId == cardId);

            if (matchingCard == null)
            {
                throw new Exception($"No card found for ID {cardId}");
            }

            result.CardName = matchingCard.Name;

            //get existing tags
            //  (this name, this deck, as dto)
            var existingTags = await _deckRepo.GetDeckCardTags(deckId, matchingCard.Name);

            //get tag suggestions (strings)
            //  get all from this deck
            var deckTags = await _deckRepo.GetAllDeckCardTags(deckId);

            //  get all from anywhere
            var allTags = await _deckRepo.GetAllDeckCardTags(null);

            //  remove any existing tags
            //  sort accordingly
            //      Need to know "tags in this deck" vs "all distinct tags"
            var existingTagNames = existingTags.Select(t => t.Description).ToList();

            deckTags = deckTags.Where(t => !existingTagNames.Contains(t)).OrderBy(t => t).ToList();

            allTags = allTags.Where(t => !existingTagNames.Contains(t) && !deckTags.Contains(t)).OrderBy(t => t).ToList();

            result.TagSuggestions = deckTags;
            result.TagSuggestions.AddRange(allTags);

            result.ExistingTags = existingTags.Select(t => new CardTag()
            {
                CardTagId = t.DeckCardTagId,
                Tag = t.Description,
            }).ToList();

            return result;
        }

        public async Task AddCardTag(CardTagDto cardTag)
        {
            var tagToAdd = new DeckCardTagData()
            {
                CardName = cardTag.CardName,
                DeckId = cardTag.DeckId,
                Description = cardTag.Tag,
            };

            await _deckRepo.AddDeckCardTag(tagToAdd);
        }

        public async Task RemoveCardTag(int cardTagId)
        {
            await _deckRepo.RemoveDeckCardTag(cardTagId);
        }

        #endregion Card Tags

        #region Search

        public async Task<List<DeckOverviewDto>> GetDeckOverviews(string format = null, string sortBy = null, bool includeDissasembled = false)
        {
            var deckList = await _cardContext.Decks
                .Include(x => x.Format)
                .ToListAsync();

            if(!string.IsNullOrEmpty(format))
            {
                deckList = deckList.Where(deck => deck.Format.Name.ToLower() == format.ToLower()).ToList();
            }

            var parsedStats = new Dictionary<int, DeckStatsDto>();

            for (int i = 0; i < deckList.Count(); i++)
            {
                var deckProps = deckList[i];

                var statsShouldBeUpdated = true;

                if(deckProps.Stats != null)
                {
                    try
                    {
                        var stats = JsonConvert.DeserializeObject<DeckStatsDto>(deckProps.Stats);
                        parsedStats[deckProps.DeckId] = stats;
                        statsShouldBeUpdated = false;
                    }
                    catch
                    {
                        deckProps.Stats = "";
                    }
                }

                //deck stats should be a separate class that can be saved as a string with the Deck Props

                if (statsShouldBeUpdated)
                {
                    var deckStats = await GetDeckStats(deckProps.DeckId);

                    parsedStats[deckProps.DeckId] = deckStats;

                    var deckStatsString = JsonConvert.SerializeObject(deckStats);

                    deckProps.Stats = deckStatsString;

                    _cardContext.Decks.Update(deckProps);
                }

                if (_cardContext.ChangeTracker.HasChanges()) 
                    await _cardContext.SaveChangesAsync();

                //deckList[i].Notes = await ValidateDeck(deckList[i].Id);
            }

            //save changes

            //map to object
            List<DeckOverviewDto> result = deckList.Select(dbDeck => new DeckOverviewDto()
            {
                Id = dbDeck.DeckId,
                //BasicB = dbDeck.BasicB,
                //BasicG = dbDeck.BasicG,
                //BasicR = dbDeck.BasicR,
                //BasicU = dbDeck.BasicU,
                //BasicW = dbDeck.BasicW,
                Format = dbDeck.Format.Name,
                Name = dbDeck.Name,

                Colors = parsedStats[dbDeck.DeckId].ColorIdentity,
                ValidationIssues = parsedStats[dbDeck.DeckId].ValidationIssues,
                IsValid = parsedStats[dbDeck.DeckId].IsValid,
                IsDisassembled = parsedStats[dbDeck.DeckId].IsDisassembled,


                //stats = ParseDeckStats(dbDeck.Stats);

                //don't want to populate notes here right?

            })
            .OrderBy(d => d.Name)
            .ToList();

            if (!includeDissasembled)
            {
                result = result.Where(d => !d.IsDisassembled).ToList();
            }

            if (!string.IsNullOrEmpty(sortBy) && sortBy.ToLower() != "name")
            {
                switch (sortBy)
                {
                    case "color":
                        //not figured out yet...

                        result = result.OrderBy(d => string.Join("", d.Colors)).ToList();

                        break;
                }
            }

            return result;
        }

        public async Task<DeckDetailDto> GetDeckDetail(int deckId)
        {
            var dbDeck = await _deckRepo.GetDeckById(deckId);

            DeckDetailDto result = new DeckDetailDto
            {
                //CardOverviews = new List<DeckCardOverview>(),
                //Cards = new List<DeckCardDetail>(),

                Props = new DeckPropertiesDto()
                {
                    Id = dbDeck.DeckId,
                    BasicB = dbDeck.BasicB,
                    BasicG = dbDeck.BasicG,
                    BasicR = dbDeck.BasicR,
                    BasicU = dbDeck.BasicU,
                    BasicW = dbDeck.BasicW,
                    Format = dbDeck.Format.Name,
                    Name = dbDeck.Name,
                    Notes = dbDeck.Notes,
                },
                Cards = new List<DeckCardOverview>(),
                Stats = new DeckStatsDto(),
            };

            var deckCardData = await _deckRepo.GetDeckCards(deckId);




            //for each card, I want to populate card 'availability'
            //availability options are: In this deck, In inventory, In another deck, Unowned

            //How do I map this data?


            //One option: Get all names of empty cards, call [that service used in restoring inventory, that gets available inv cards by name], use that to map

            var emptyCardNames = deckCardData.Where(dc => dc.InventoryCardId == null).Select(dc => dc.Name).Distinct();

            var inventoryCardsByName = await _inventoryRepo.GetUnusedInventoryCards(emptyCardNames);
            //GetUnusedInventoryCards


            #region Group & Map for DTO

            //need to re-think how this should all be grouped & returned
            //In the actual react app, data should be applied as a dict
            //(Should this still return overviews / details as separate objects, or have them be parent/child objects?)
            //  It doesn't ultimately matter


            /*
             
            API State:
                Props {}
                Stats {}
                Overviews[{
                    Overview Id
                    {common card props (name, count, cmc, ...}
                    Details[{
                        Id
                        CardName
                        InventoryId
                        ???
                    }]
                }]

            React app state:
                Props {}
                Stats {}
                Overviews {
                    ById[#:{
                        name; count; cmc;
                        details[#,#,#]
                    }]
                    AllIds
                }

                Details {
                    ById
                    AllIds

                }
             


            How do I...visually map details for a card overview
             */

            //No matter what, I want to group by Name&Category, & map to other records

            //Card Overviews
            //var deckOverviewsResult = await _queryService.GetDeckCardOverviews(deckId);


            //Remember this is all in-memory data.  Commonly no more than 100, probably never more than 500 records
            //var groupedCards = deckCardData
            result.Cards = deckCardData
                .GroupBy(x => new
                {
                    x.Name,
                    x.Category,
                })
                .Select((g, i) => new DeckCardOverview
                {
                    Id = (i+1),
                    Name = g.Key.Name,
                    Category = g.Key.Category ?? GetCardTypeGroup(g.First().Type),
                    Cmc = g.First().Cmc,
                    Cost = g.First().Cost,
                    Count = g.Count(),
                    //TODO - I wanted to make sure that, if I had several versions of a card in a deck, then I show the "newest" card
                    //I should investigate if that's actually happening
                    Img = g.First().Img,
                    Type = g.First().Type,
                    CardId = g.First().CardId,
                    Tags = g.First().Tags,
                    Details = g.Select(d => new DeckCardDetail
                    {
                        Id = d.DeckCardId,
                        DeckId = d.DeckId,
                        OverviewId = (i + 1),
                        Category = d.Category ?? GetCardTypeGroup(d.Type),
                        CollectorNumber = d.CollectorNumber,
                        IsFoil = d.IsFoil,
                        Name = d.Name,
                        Set = d.SetCode,
                        InventoryCardId = d.InventoryCardId,
                        CardId = d.CardId,
                        //DeckId = d.

                        //Id = i + 1, //Incremented because I want it to start at 1 instead of 0
                        
                        //Cost = x.Item.Cost,
                        //Name = x.Item.Name,
                        //Count = x.Count,
                        //Img = x.Item.Img,
                        //Type = x.Item.Type,
                        //Category = x.Item.Category ?? GetCardTypeGroup(x.Item.Type),
                        //Cmc = x.Item.Cmc,
                    }).ToList(),
                }).OrderBy(c => c.Name)
                .ToList();


            foreach(var cardOverview in result.Cards)
            {
                foreach (var cardDetail in cardOverview.Details)
                {
                    if (cardDetail.InventoryCardId != null)
                    {
                        cardDetail.AvailabilityId = _availability_InDeck;
                    }
                    else if (inventoryCardsByName.TryGetValue(cardOverview.Name, out var availableCards) && availableCards.Count > 0)
                    {
                        var firstAvailableCard = availableCards
                            .OrderBy(ic => ic.DeckCards.Count)
                            .First();

                        cardDetail.AvailabilityId = (firstAvailableCard.DeckCards.Count == 0) ? _availability_InInventory : _availability_InOtherDeck;

                        inventoryCardsByName[cardOverview.Name].Remove(firstAvailableCard);
                    }
                    else
                    {
                        cardDetail.AvailabilityId = _availability_Unowned;
                    }

                }

            }

            //result.CardOverviews = groupedCards.Select((x, i) => new DeckCardOverview()
            //{
            //    Id = i + 1, //Incremented because I want it to start at 1 instead of 0
            //    Cost = x.Item.Cost,
            //    Name = x.Item.Name,
            //    Count = x.Count,
            //    Img = x.Item.Img,
            //    Type = x.Item.Type,
            //    Category = x.Item.Category ?? GetCardTypeGroup(x.Item.Type),
            //    Cmc = x.Item.Cmc,
            //}).ToList();

            ////Card Details
            //result.Cards = deckCardData.Select(x => new DeckCardDetail()
            //{
            //    Id = x.DeckCardId,
            //    IsFoil = x.IsFoil,
            //    Name = x.Name,
            //    Category = x.Category,
            //    CollectorNumber = x.CollectorNumber,
            //    Set = x.SetId.ToString(),
            //}).ToList();
            

            #endregion

            #region deck stats

            result.Stats = new DeckStatsDto()
            {
                CostCounts = new Dictionary<string, int>(),
                TotalCost = 0,
                TotalCount = 0,
                ColorIdentity = new List<string>() { "w","r","g" },
                TypeCounts = new Dictionary<string, int>(),
            };

            //Total mainboard cards
            int basicLandCount = result.Props.BasicW + result.Props.BasicU + result.Props.BasicB + result.Props.BasicR + result.Props.BasicG;

            result.Stats.TotalCount = deckCardData.Where(c => c.Category != _sideboardCategory).Count() + basicLandCount;

            //cmc breakdown of mainboard cards
            result.Stats.CostCounts = deckCardData
                .Where(c => c.Category != _sideboardCategory && !c.Type.Contains("Land"))
                .Select(x => x.Cmc)
                .GroupBy(x => x)
                .Select(x => new
                {
                    Cmc = x.Key,
                    Count = x.Count(),
                })
                .OrderBy(x => x.Cmc)
                .ToDictionary(x => x.Cmc.ToString(), x => x.Count);

            //type breakdown of mainboard cards
            List<string> cardTypes = deckCardData
                .Where(c => c.Category != _sideboardCategory)
                .Select(x => GetCardTypeGroup(x.Type))
                .ToList();

            var typeCountsDict = cardTypes
                .GroupBy(x => x)
                .Select(x => new
                {
                    Name = x.Key,
                    Count = x.Count()
                })
                .ToDictionary(x => x.Name, x => x.Count);

            if (typeCountsDict.Keys.Contains("Lands"))
            {
                typeCountsDict["Lands"] = typeCountsDict["Lands"] + basicLandCount;
            }
            else
            {
                typeCountsDict["Lands"] = basicLandCount;
            }

            result.Stats.TypeCounts = typeCountsDict;

            //tag counts
            var cardTags = deckCardData
                .Where(c => c.Category != _sideboardCategory)
                .Select(c => c.Tags.Any() ? c.Tags : new List<string>() { "Untagged" })
                .SelectMany(tags => tags)
                //.SelectMany(card => card.Tags)
                .ToList();

            var cardTagsDict = cardTags
                .GroupBy(t => t)
                .Select(g => new
                {
                    Tag = g.Key,
                    Count = g.Count()
                })
                .ToDictionary(x => x.Tag, x => x.Count);

            if (cardTagsDict.Keys.Contains("Land"))
            {
                cardTagsDict["Land"] = cardTagsDict["Land"] + basicLandCount;
            }
            else
            {
                cardTagsDict["Land"] = basicLandCount;
            }

            result.Stats.TagCounts = cardTagsDict;

            //deck color identity(including sideboard)
            result.Stats.ColorIdentity = deckCardData
                .SelectMany(c => c.ColorIdentity.ToCharArray())
                .Distinct()
                .Select(c => c.ToString())
                .ToList();

            //Price (should this include sideboard?)
            var cardsToPrice = deckCardData;
            if(result.Props.Format.ToLower() == "commander")
            {
                cardsToPrice = cardsToPrice.Where(c => c.Category != _sideboardCategory).ToList();
            }
            result.Stats.TotalCost = cardsToPrice.Select(c => c.IsFoil ? c.PriceFoil : c.Price).Sum() ?? 0;

            #endregion

            

            return result;
        }

        #endregion Search

        #region Import / Export



        //public async Task<ValidatedDeckImportDto> ValidateDeckImport(CardImportDto dto)
        //{
        //    var validatedResult = await _cardImportService.ValidateDeckImport(dto);

        //    return validatedResult;
        //}

        //public async Task AddValidatedDeckImport(ValidatedDeckImportDto validatedDto)
        //{
        //    await _cardImportService.AddValidatedDeckImport(validatedDto);
        //}

        public async Task<string> GetDeckListExport(int deckId, string exportType)
        {
            var deckCardData = await _deckRepo.GetDeckCards(deckId);

            if (exportType == "empty") return FormatExportEmptyCards(deckCardData);

            if (exportType == "suggestions") return await FormatExportCardSuggestions(deckCardData);

            //add basic lands for default format
            var deckData = await _deckRepo.GetDeckById(deckId);

            if (deckData.BasicW > 0)
                for(var i = 0; i < deckData.BasicW; i++)
                    deckCardData.Add(new DeckCardResult() { Name = "Plains" });

            if (deckData.BasicU > 0)
                for (var i = 0; i < deckData.BasicU; i++)
                    deckCardData.Add(new DeckCardResult() { Name = "Island" });

            if (deckData.BasicB > 0)
                for (var i = 0; i < deckData.BasicB; i++)
                    deckCardData.Add(new DeckCardResult() { Name = "Swamp" });

            if (deckData.BasicR > 0)
                for (var i = 0; i < deckData.BasicR; i++)
                    deckCardData.Add(new DeckCardResult() { Name = "Mountain" });

            if (deckData.BasicG > 0)
                for (var i = 0; i < deckData.BasicG; i++)
                    deckCardData.Add(new DeckCardResult() { Name = "Forest" });

            if(exportType == "detail")
            {
                return FormatExportDeckList(deckCardData, true);
            }

            return FormatExportDeckList(deckCardData);
        }

        private string FormatExportDeckList(IEnumerable<DeckCardResult> deckCardData, bool includeTags = false)
        {
            var deckCardStrings = new List<CardLine>();

            foreach(var dc in deckCardData)
            {
                string cardString = dc.Name;

                if(dc.SetCode != null)
                    cardString = $"{cardString} ({dc.SetCode}) {dc.CollectorNumber}";

                if(dc.IsFoil)
                    cardString = $"{cardString} FOIL";

                //If I'm worried about special characters in tags, I can encode/wrap them some other way (not just comma-separated)
                if (includeTags && dc.Tags?.Count > 0)
                {
                    var tagString = string.Join(',', dc.Tags);
                    var encodedString = WebUtility.UrlEncode(tagString);
                    cardString = $"{cardString} {{{ encodedString }}}";
                }
                    
               
                deckCardStrings.Add(new CardLine
                {
                    Category = dc.Category,
                    CardString = cardString,
                });
            }

            //each card string, and its category
            var cardStrings = deckCardStrings
                .GroupBy(dc => new
                {
                    dc.Category,
                    dc.CardString,
                })
                .OrderBy(g => g.Key.CardString)
                .Select(g => new
                {
                    g.Key.Category,
                    CardString = $"{g.Count()} {g.Key.CardString}",
                })
                .ToList();

            var cardStringCategories = cardStrings
                .GroupBy(g => g.Category)
                .Select(g => new
                {
                    Category = g.Key,
                    Cards = g.Select(i => i.CardString),
                })
                .OrderBy(g => g.Category)
                .ToList();

            var exportList = new List<string>();

            foreach (var group in cardStringCategories)
            {
                exportList.Add(group.Category);
                exportList.AddRange(group.Cards);
                exportList.Add("");
            }

            var result = string.Join('\n', exportList);

            return result;
        }

        private string FormatExportEmptyCards(IEnumerable<DeckCardResult> deckCardData)
        {
            var emptyCards = deckCardData.Where(dc => dc.InventoryCardId == null);
            return FormatExportDeckList(emptyCards);
        }

        private async Task<string> FormatExportCardSuggestions(IEnumerable<DeckCardResult> deckCardData)
        {
            var emptyCards = deckCardData.Where(dc => dc.InventoryCardId == null);

            var emptyCardNames = deckCardData.Select(dc => dc.Name).Distinct();

            var availableInventoryCards = await _inventoryRepo.GetUnusedInventoryCards(emptyCardNames);

            //current plan won't involve grouping things by category, just by name
            //Will only list cards needed by name (including # needed), then list available cards

            var groupedEmptyCards = emptyCards.GroupBy(c => c.Name).OrderBy(g => g.Key);

            var exportList = new List<string>();

            foreach (var group in groupedEmptyCards)
            {
                exportList.Add($"Card: {group.Key} Count: {group.Count()}");

                if(availableInventoryCards.TryGetValue(group.Key, out var matchingCards))
                {
                    if(matchingCards == null || matchingCards.Count == 0)
                    {
                        exportList.Add("{No owned copies}");
                        continue;
                    }

                    //available cards will be grouped by [cardId,isFoil,deckId], and presented with group counts

                    var groupedMatches = matchingCards.GroupBy(c => new
                    {
                        c.CardId,
                        c.IsFoil,
                        c.DeckCards.FirstOrDefault()?.DeckId,
                    }).ToList();

                    foreach(var groupMatch in groupedMatches)
                    {
                        var invCard = groupMatch.First();

                        var deckId = invCard.DeckCards.FirstOrDefault()?.DeckId;
                        var deckName = deckId == null ? "Inventory" : invCard.DeckCards.First().Deck.Name;

                        var foil = invCard.IsFoil ? " (foil)" : "";
                        //set , collNum , foil , location , count
                        exportList.Add($"{invCard.Card.Set.Code} {invCard.Card.CollectorNumber} ({invCard.Card.RarityId}) {foil} - {deckName} - {groupMatch.Count()}");
                    }
                }

                exportList.Add("");

            }

            var result = string.Join('\n', exportList);

            return result;
        }

        #endregion Import / Export

        #endregion
    }
}
