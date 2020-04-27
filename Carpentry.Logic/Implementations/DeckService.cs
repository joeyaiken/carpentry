using Carpentry.Data.Interfaces;
using Carpentry.Logic.Interfaces;
using Carpentry.Logic.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Carpentry.Data.DataModels;
using System.Linq;
using Carpentry.Data.QueryResults;
//using Carpentry.Data.DataContext;
//using Carpentry.Data.DataModels;

namespace Carpentry.Logic.Implementations
{
    public class DeckService : IDeckService
    {
        //All methods should return a model specific to THIS project, not the data project (evevntually)

        //What if all data layer models were either
        //1 -   A DB/DataContext model
        //2 -   A DTO that contains either IDs or values but not the associations

        //Should have no access to data context classes, only repo classes
        //private readonly ILegacyCardRepo _cardRepo;
        //private readonly ICardStringRepo _scryRepo;
        private readonly ILogger<DeckService> _logger;

        private readonly IDeckDataRepo _deckRepo;

        private readonly IDataQueryService _queryService;

        private readonly IInventoryService _inventoryService;

        public IDataReferenceService _referenceService;

        public DeckService(
            IDeckDataRepo deckRepo, 
            IDataQueryService queryService, 
            IInventoryService inventoryService, 
            ILogger<DeckService> logger,
            IDataReferenceService referenceService
            )
        {
            _deckRepo = deckRepo;
            _queryService = queryService;
            _inventoryService = inventoryService;
            _logger = logger;
            _referenceService = referenceService;
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

        private async Task<DeckStatsDto> GetDeckStats(int deckId)
        {
            DeckStatsDto result = new DeckStatsDto();

            //total Count
            result.TotalCount = await _queryService.GetDeckCardCount(deckId);

            var statData = await _queryService.GetDeckCardStats(deckId);

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
            Dictionary<string,int> deckCardCostsDict = statData
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
            string validationResult = "";
            List<string> validationErrors = new List<string>();

            //var deck = await _cardRepo.QueryDeckProperties().Where(x => x.Id == deckId).FirstOrDefaultAsync();
            var deck = await _deckRepo.GetDeckById(deckId);

            string deckFormat = deck.Format.Name.ToLower();

            #region Validate deck size

            int deckSize = await _queryService.GetDeckCardCount(deckId);

            //what's the min deck count for this format?
            if (deckFormat == "commander")
            {
                //must be exactly 100 cards to be valid
                if (deckSize < 100)
                {
                    validationErrors.Add($"Below size requirement: {deckSize}/100 cards");
                }

                if (deckSize > 100)
                {
                    validationErrors.Add($"Above size limit: {deckSize}/100 cards");
                }
            }
            else
            {
                if ((deckFormat == "brawl" || deckFormat == "oathbreaker") && deckSize > 60)
                {
                    validationErrors.Add($"Above size limit: {deckSize}/60 cards");
                }

                if (deckSize < 60)
                {
                    validationErrors.Add($"Below size requirement: {deckSize}/60 cards");
                }
            }

            #endregion

            #region Validate max per card (1 for singleton, 4 for other formats)



            #endregion

            #region Validate format legality



            #endregion

            #region Validate color rules for commander/brawl



            #endregion

            validationResult = string.Join(" ", validationErrors);

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

        public async Task<int> AddDeck(DeckPropertiesDto props)
        {
            DataReferenceValue<int> deckFormat = await _referenceService.GetMagicFormat(props.Format);

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

        public async Task UpdateDeck(DeckPropertiesDto deckDto)
        {
            DeckData existingDeck = await _deckRepo.GetDeckById(deckDto.Id);

            if (existingDeck == null)
            {
                throw new Exception("No deck found matching the specified ID");
            }

            DataReferenceValue<int> deckFormat = await _referenceService.GetMagicFormat(deckDto.Format);

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

        public async Task<IEnumerable<DeckPropertiesDto>> GetDeckOverviews() //TODO - rename to "GetDeckOverviews" or "GetDeckProperties"
        {


            //List<DeckPropertiesDto> deckList = _deckRepo.GetAllDecks().Result.Select(x => MapDeckDataToProperties(x)).ToList();



            List<DeckPropertiesDto> deckList = _deckRepo.GetAllDecks().Result.Select(dbDeck => new DeckPropertiesDto()
            {
                Id = dbDeck.Id,
                BasicB = dbDeck.BasicB,
                BasicG = dbDeck.BasicG,
                BasicR = dbDeck.BasicR,
                BasicU = dbDeck.BasicU,
                BasicW = dbDeck.BasicW,
                Format = dbDeck.Format.Name,
                Name = dbDeck.Name,
                //don't want to populate notes here right?
                
            }).ToList();

            for (int i = 0; i < deckList.Count(); i++)
            {
                deckList[i].Notes = await ValidateDeck(deckList[i].Id);
            }

            return deckList;
        }


        
        //TODO - A DeckDTO shouldn't really contain an InventoryOverviewDto/InventoryCardDto,
        //it should contain a specific DeckDetail and DeckOverview DTO instead, that contains fields relevant to that container
        public async Task<DeckDetailDto> GetDeckDetail(int deckId)
        {
            //var dbDeck = await _deckRepo.GetDeckById(deckId);


            //DeckPropertiesDto mappedDeckData = MapDeckDataToProperties(await _deckRepo.GetDeckById(deckId));

            var dbDeck = await _deckRepo.GetDeckById(deckId);
            DeckPropertiesDto mappedDeckData = new DeckPropertiesDto()
            {
                Id = dbDeck.Id,
                BasicB = dbDeck.BasicB,
                BasicG = dbDeck.BasicG,
                BasicR = dbDeck.BasicR,
                BasicU = dbDeck.BasicU,
                BasicW = dbDeck.BasicW,
                Format = dbDeck.Format.Name
            };

            //DeckPropertiesDto mappedDeckData = new DeckPropertiesDto()
            //{
            //    Id = dbDeck.Id,
            //    BasicB = dbDeck.BasicB,
            //    BasicG = dbDeck.BasicG,
            //    BasicR = dbDeck.BasicR,
            //    BasicU = dbDeck.BasicU,
            //    BasicW = dbDeck.BasicW,
            //    Format = dbDeck.Format.Name
            //};

            DeckDetailDto result = new DeckDetailDto
            {
                CardOverviews = new List<InventoryOverviewDto>(),
                CardDetails = new List<InventoryCardDto>(),
                Props = mappedDeckData,
                Stats = new DeckStatsDto(),
            };

            //Card Overviews
            result.CardOverviews = (await _queryService.GetDeckCardOverviews(deckId)).Select(x => new InventoryOverviewDto()
            {
                Id = x.Id,
                Cost = x.Cost,
                Name = x.Name,
                Count = x.Count,
                Img = x.Img,
                Type = x.Type,
                Description = x.Category ?? GetCardTypeGroup(x.Type),
                Cmc = x.Cmc,
            })//.ToList()
            .OrderBy(x => x.Cmc).ThenBy(x => x.Name).ToList();

            //Card Details
            //TODO - this REALLY needs to be refactored to use a "Deck Card Overview" / "Deck Card Detail" pattern

            var queryResult = await _queryService.GetDeckInventoryCards(deckId);

            result.CardDetails = queryResult
                .Select(x => new InventoryCardDto()
                {
                    Id = x.Id,
                    MultiverseId = x.MultiverseId,
                    InventoryCardStatusId = x.InventoryCardStatusId,
                    IsFoil = x.IsFoil,
                    VariantType = x.VariantType,
                    Name = x.Name,

                    DeckCards = x.DeckCards != null ?
                        x.DeckCards.Select(deckCard => new InventoryDeckCardDto()
                        {
                            DeckId = deckCard.DeckId,
                            Id = deckCard.Id,
                            InventoryCardId = x.Id,

                            DeckCardCategory = deckCard.DeckCardCategory ?? GetCardTypeGroup(x.Type), //What is this uesd for?
                            //OH, I need category for filtering inventory cards on the client app
                        }).ToList() : null,
                }).ToList();

            //Deck Stats
            result.Stats = await GetDeckStats(deckId);

            return result;
        }

        /// <summary>
        /// Adds a card to a deck
        /// If the dto references an existing deck card, and that card is ALREADY in a deck, no new card is added
        ///     Instead, the existing card is moved to this deck
        /// If the card exists, but isn't in a deck, then a new deck card is created
        /// If the inventory card doesn't exist, then a new one is mapped
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task AddDeckCard(DeckCardDto dto)
        {
            //Don't need to add an inventory card
            if (dto.InventoryCard.Id > 0)
            {
                //if a card already exists in a deck it is "moved" to this deck
                var existingDeckCard = await _deckRepo.GetDeckCardByInventoryId(dto.InventoryCard.Id);

                if (existingDeckCard != null)
                {
                    existingDeckCard.DeckId = dto.DeckId;
                    existingDeckCard.CategoryId = null;

                    await _deckRepo.UpdateDeckCard(existingDeckCard);
                    return;
                }

                //nothing needs to be created, the deck and inventory cards already exist
            }
            
            //Need to add a new inventory card for this deck card
            //(check how I'm adding inventory cards atm)
            if (dto.InventoryCard.Id == 0)
            {
                int newInventoryCardId = await _inventoryService.AddInventoryCard(dto.InventoryCard);
                dto.InventoryCard.Id = newInventoryCardId;
            }

            DeckCardData cardToAdd = new DeckCardData()
            {
                CategoryId = dto.CategoryId,
                DeckId = dto.DeckId,
                InventoryCardId = dto.InventoryCard.Id,
            };

            await _deckRepo.AddDeckCard(cardToAdd);
            //await _cardRepo.AddDeckCard(dto);
        }

        //I think I only end up adding NEW deck cards with a batch
        //IDR where exactly this gets called outside of console apps
        public async Task AddDeckCardBatch(IEnumerable<DeckCardDto> dtoBatch)
        {
            ////#error not implemented
            //            throw new NotImplementedException();
            _logger.LogWarning("Beginning AddDeckCardBatch");
            var dtoArray = dtoBatch.ToArray();

            for (int i = 0; i < dtoArray.Count(); i++)
            {
                DeckCardDto dto = dtoArray[i];

                //console apps want to see this
                _logger.LogWarning($"Adding card #{i} MID {dto.InventoryCard.MultiverseId}");

                await AddDeckCard(dto);
            }
        }

        public async Task UpdateDeckCard(DeckCardDto card)
        {
            DeckCardData dbCard = await _deckRepo.GetDeckCardById(card.Id);

            dbCard.DeckId = card.DeckId;
            dbCard.CategoryId = card.CategoryId;

            await _deckRepo.UpdateDeckCard(dbCard);
        }

        public async Task DeleteDeckCard(int deckCardId)
        {
            await _deckRepo.DeleteDeckCard(deckCardId);
        }

        #endregion

    }
}
