using Carpentry.Data.Interfaces;
using Carpentry.Logic.Interfaces;
using Carpentry.Logic.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Carpentry.Logic.Implementations
{
    public class DeckService : IDeckService
    {
        //All methods should return a model specific to THIS project, not the data project (evevntually)

        //What if all data layer models were either
        //1 -   A DB/DataContext model
        //2 -   A DTO that contains either IDs or values but not the associations

        //Should have no access to data context classes, only repo classes
        private readonly ICardRepo _cardRepo;
        //private readonly ICardStringRepo _scryRepo;
        //private readonly ILogger<CarpentryService> _logger;

        public DeckService(
            ICardRepo cardRepo
            //,ICardStringRepo scryRepo, ILogger<CarpentryService> logger
            )
        {
            _cardRepo = cardRepo;
            //_scryRepo = scryRepo;
            //_logger = logger;
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

        //        private async Task<DeckStats> GetDeckStats(int deckId)
        //        {
        //            DeckStats result = new DeckStats();

        //            var deckProps = await _cardRepo.QueryDeckProperties().FirstOrDefaultAsync(x => x.Id == deckId);

        //            //total Count
        //            var deckCardCount = await _cardRepo.QueryDeckCards().Where(x => x.DeckId == deckId && x.CategoryId != 's').CountAsync();

        //            int basicLandCount = deckProps.BasicW + deckProps.BasicU + deckProps.BasicB + deckProps.BasicR + deckProps.BasicG;

        //            result.TotalCount = deckCardCount + basicLandCount;

        //            //total price
        //            var priceQuery = _cardRepo.QueryDeckCards() //_cardContext.DeckCards
        //                .Where(x => x.DeckId == deckId && x.CategoryId != 's')
        //                .Select(x => new
        //                {
        //                    CardName = x.InventoryCard.Card.Name,
        //                    //CardVariantName = x.InventoryCard.VariantType.Name,
        //                    //VariantTypeId = x.InventoryCard.VariantTypeId,
        //                    ////price
        //                    Price = x.InventoryCard.Card.Variants.Where(cardVariant => cardVariant.CardVariantTypeId == x.InventoryCard.VariantTypeId).FirstOrDefault().Price,

        //                });

        //            var totalPrice = await priceQuery.SumAsync(x => x.Price);

        //            result.TotalCost = totalPrice ?? 0;

        //            //type-breakdown
        //            var cardTypes = _cardRepo.QueryDeckCards()
        //                .Where(x => x.DeckId == deckId && x.CategoryId != 's')
        //                .Select(x => x.InventoryCard.Card.Type)
        //                .Select(x => GetCardTypeGroup(x))
        //                .ToList();

        //            var typeCountsDict = cardTypes
        //                .GroupBy(x => x)
        //                .Select(x => new
        //                {
        //                    Name = x.Key,
        //                    Count = x.Count()
        //                })
        //                .AsEnumerable()
        //                .ToDictionary(x => x.Name, x => x.Count);

        //            if (typeCountsDict.Keys.Contains("Lands"))
        //            {
        //                typeCountsDict["Lands"] = typeCountsDict["Lands"] + basicLandCount;
        //            }
        //            else
        //            {
        //                typeCountsDict["Lands"] = basicLandCount;
        //            }

        //            result.TypeCounts = typeCountsDict;


        //            //cost-breakdown, probably requires re-querying everything
        //            var deckCardCostsDict = _cardRepo.QueryDeckCards() //_cardContext.DeckCards
        //                .Where(x => x.DeckId == deckId && x.CategoryId != 's')
        //                .Select(x => x.InventoryCard.Card)
        //                .Where(x => !x.Type.Contains("Land"))
        //                .Select(x => x.Cmc)
        //                //.ToList();
        //                .GroupBy(x => x)
        //                .Select(x => new
        //                {
        //                    Name = x.Key,
        //                    Count = x.Count(),
        //                })
        //                .AsEnumerable()
        //                .OrderByDescending(x => x.Name)
        //                .ToDictionary(x => x.Name.ToString(), x => x.Count);

        //            result.CostCounts = deckCardCostsDict;


        //            //deck color identity
        //            //all of the basic lands 
        //            //+ every card's color identity
        //            var cardCIQuery = _cardRepo.QueryDeckCards() //_cardContext.DeckCards
        //                .Where(x => x.DeckId == deckId)
        //                .Select(x => x.InventoryCard.Card)
        //                .SelectMany(card => card.CardColorIdentities.Select(ci => ci.ManaTypeId))
        //                .Distinct();

        //            result.ColorIdentity = cardCIQuery.ToList();

        //            return result;
        //        }

        //        private static string GetCardTypeGroup(string cardType)
        //        {
        //            if (cardType.ToLower().Contains("creature"))
        //            {
        //                return "Creatures";
        //            }
        //            else if (cardType.ToLower().Contains("land"))
        //            {
        //                return "Lands";
        //            }
        //            else if (cardType.ToLower().Contains("planeswalker"))
        //            {
        //                return "Planeswalkers";
        //            }
        //            else if (cardType.ToLower().Contains("enchantment"))
        //            {
        //                return "Enchantments";
        //            }
        //            else if (cardType.ToLower().Contains("artifact"))
        //            {
        //                return "Artifacts";
        //            }
        //            //else if (cardType.ToLower().Contains(""))
        //            //{
        //            //    return "";
        //            //}
        //            //else if (cardType.ToLower().Contains(""))
        //            //{
        //            //    return "";
        //            //}
        //            else
        //            {
        //                return "Spells";
        //            }
        //        }

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

        //        private async Task<string> ValidateDeck(int deckId)
        //        {
        //            string validationResult = "";
        //            List<string> validationErrors = new List<string>();

        //            var deck = await _cardRepo.QueryDeckProperties().Where(x => x.Id == deckId).FirstOrDefaultAsync();

        //            string deckFormat = deck.Format.ToLower();

        //            #region Validate deck size

        //            int basicLandCount = deck.BasicW + deck.BasicU + deck.BasicB + deck.BasicR + deck.BasicG;

        //            var cardCount = _cardRepo.QueryDeckCards().Where(x => x.DeckId == deckId).Count();

        //            int deckSize = basicLandCount + cardCount;

        //            //what's the min deck count for this format?
        //            if (deckFormat == "commander")
        //            {
        //                //must be exactly 100 cards to be valid
        //                if (deckSize < 100)
        //                {
        //                    validationErrors.Add($"Below size requirement: {deckSize}/100 cards");
        //                }

        //                if (deckSize > 100)
        //                {
        //                    validationErrors.Add($"Above size limit: {deckSize}/100 cards");
        //                }
        //            }
        //            else
        //            {
        //                if ((deckFormat == "brawl" || deckFormat == "oathbreaker") && deckSize > 60)
        //                {
        //                    validationErrors.Add($"Above size limit: {deckSize}/60 cards");
        //                }

        //                if (deckSize < 60)
        //                {
        //                    validationErrors.Add($"Below size requirement: {deckSize}/60 cards");
        //                }
        //            }

        //            #endregion

        //            #region Validate max per card (1 for singleton, 4 for other formats)



        //            #endregion

        //            #region Validate format legality



        //            #endregion

        //            #region Validate color rules for commander/brawl



        //            #endregion

        //            validationResult = string.Join(" ", validationErrors);

        //            return validationResult;
        //        }

        #endregion

        #region Deck related methods

        public async Task<int> AddDeck(DeckProperties props)
        {

            //This layer knows of it's own models, but not the UI layer
            //The DATA layer won't know of this layer's model, so we must map to a type consumable by the data layer
            Data.LegacyDataContext.MagicFormat deckFormat = await _cardRepo.GetFormatByName(props.Format);

            Data.LegacyDataContext.Deck newDeck = new Data.LegacyDataContext.Deck()
            {
                Name = props.Name,
                Format = deckFormat,
                Notes = props.Notes,

                BasicW = props.BasicW,
                BasicU = props.BasicU,
                BasicB = props.BasicB,
                BasicR = props.BasicR,
                BasicG = props.BasicG,
            };

            int newId = await _cardRepo.AddDeck(newDeck);

            return newId;
        }

        public async Task UpdateDeck(DeckProperties props)
        {
            await Task.CompletedTask;
            throw new NotImplementedException();
            //await _cardRepo.UpdateDeck(props);
        }

        public async Task DeleteDeck(int deckId)
        {
            await Task.CompletedTask;
            throw new NotImplementedException();
            //await _cardRepo.DeleteDeck(deckId);
        }

        public async Task<IEnumerable<DeckProperties>> SearchDecks()
        {
            throw new NotImplementedException();
            ////var price = await CalculateInventoryTotalPrice();


            //List<DeckProperties> deckList = await _cardRepo.QueryDeckProperties().ToListAsync();

            //for (int i = 0; i < deckList.Count(); i++)
            //{
            //    deckList[i].Notes = await ValidateDeck(deckList[i].Id);
            //}

            //return deckList;
        }


        //TODO - A DeckDTO shouldn't really contain an InventoryOverviewDto/InventoryCardDto,
        //it should contain a specific DeckDetail and DeckOverview DTO instead, that contains fields relevant to that container
        public async Task<DeckDetail> GetDeckDetail(int deckId)
        {
            throw new NotImplementedException();
            ////Deck Props
            //DeckProperties targetDeck = await _cardRepo.QueryDeckProperties().Where(x => x.Id == deckId).FirstOrDefaultAsync();

            //DeckDto result = new DeckDto
            //{
            //    CardOverviews = new List<InventoryOverviewDto>(),
            //    CardDetails = new List<InventoryCardDto>(),
            //    Props = targetDeck,
            //    Stats = new DeckStats(),
            //};

            ////Deck Overviews
            //var cardOverviewsQuery = _cardRepo.QueryDeckCards()
            //    .Where(x => x.DeckId == deckId)
            //    .Select(x => new
            //    {
            //        DeckCardCategory = (x.CategoryId != null) ? x.Category.Name : null,
            //        x.InventoryCard.Card,
            //        x.InventoryCard.Card.Variants.FirstOrDefault(v => v.CardVariantTypeId == 1).ImageUrl,
            //    });

            ////var rawOverviews = cardOverviewsQuery.ToList();

            ////var cardOverviewsQuery = _cardRepo.QueryInventoryCardsForDeck(deckId)
            ////    .Select(x => new {
            ////        x.Card,
            ////        x.Card.Variants.FirstOrDefault(v => v.CardVariantTypeId == 1).ImageUrl,


            ////    })
            //var cardOverviewQueryResult = cardOverviewsQuery
            //    .GroupBy(x => new
            //    {
            //        x.Card.Name,
            //        x.DeckCardCategory
            //    })
            //    .Select(x => new
            //    {
            //        Name = x.Key.Name,
            //        Item = x.OrderByDescending(c => c.Card.Id).FirstOrDefault(),
            //        Count = x.Count(),
            //    }).ToList();

            ////#error this isnt properly mapping MultiverseId, trying to add it from github
            ////I was wrong, this isn't supposed to include MID because it's deck cards grouped by NAME
            //var overviewQueryResult = cardOverviewQueryResult.Select((x, i) => new InventoryOverviewDto()
            //{
            //    Id = i + 1,
            //    //MultiverseId = x.Item.MultiverseId,
            //    Cost = x.Item.Card.ManaCost,
            //    Name = x.Name,
            //    Count = x.Count,
            //    Img = x.Item.ImageUrl,
            //    Type = x.Item.Card.Type,
            //    Description = x.Item.DeckCardCategory ?? GetCardTypeGroup(x.Item.Card.Type),
            //    Cmc = x.Item.Card.Cmc,
            //}).ToList()
            //.OrderBy(x => x.Cmc).ThenBy(x => x.Name).ToList();

            //result.CardOverviews = overviewQueryResult;


            ////Deck Details
            //List<InventoryCardDto> cardDetails = _cardRepo.QueryInventoryCardsForDeck(deckId)
            //    .Select(x => new InventoryCardDto()
            //    {
            //        Id = x.Id,
            //        MultiverseId = x.MultiverseId,
            //        InventoryCardStatusId = x.InventoryCardStatusId,
            //        IsFoil = x.IsFoil,
            //        VariantType = x.VariantType.Name,
            //        Name = x.Card.Name,
            //        DeckCards = x.DeckCards.Select(deckCard => new InventoryDeckCardDto()
            //        {
            //            DeckId = deckCard.DeckId,
            //            Id = deckCard.Id,
            //            InventoryCardId = x.Id,
            //            DeckCardCategory = (deckCard.Category != null) ? deckCard.Category.Name : GetCardTypeGroup(x.Card.Type),
            //        }).ToList(),
            //    }).ToList();

            //result.CardDetails = cardDetails;

            ////Deck Stats
            //result.Stats = await GetDeckStats(deckId);


            //return result;
        }

        //if a card already exists in a deck it is "moved" to this deck
        public async Task AddDeckCard(DeckCard dto)
        {
            throw new NotImplementedException();
            //if (dto.InventoryCard.Id > 0)
            //{
            //    //is this in a deck?
            //    var existingDeckCard = await _cardRepo.QueryDeckCards()
            //        .Where(x => x.InventoryCardId == dto.InventoryCard.Id)
            //        .FirstOrDefaultAsync();

            //    if (existingDeckCard != null)
            //    {
            //        await _cardRepo.UpdateDeckCard(existingDeckCard.Id, dto.DeckId, null);
            //        return;
            //    }
            //}

            //if (dto.InventoryCard.Id == 0)
            //{
            //    await EnsureCardDefinitionExists(dto.InventoryCard.MultiverseId);

            //    //add inventory card
            //    var newInventoryCardId = await _cardRepo.AddInventoryCard(dto.InventoryCard);

            //    dto.InventoryCard.Id = newInventoryCardId;
            //}

            //await _cardRepo.AddDeckCard(dto);
        }

        //I think I only end up adding NEW deck cards with a batch
        //IDR where exactly this gets called outside of console apps
        public async Task AddDeckCardBatch(IEnumerable<DeckCard> dtoBatch)
        {
            throw new NotImplementedException();
            //_logger.LogWarning("Beginning AddDeckCardBatch");
            //var dtoArray = dtoBatch.ToArray();

            //for (int i = 0; i < dtoArray.Count(); i++)
            //{
            //    DeckCardDto dto = dtoArray[i];

            //    //console apps want to see this
            //    _logger.LogWarning($"Adding card #{i} MID {dto.InventoryCard.MultiverseId}");

            //    await _cardRepo.AddDeckCard(dto);
            //}
        }

        public async Task UpdateDeckCard(DeckCard card)
        {
            throw new NotImplementedException();
            //await _cardRepo.UpdateDeckCard(card.Id, card.DeckId, card.CategoryId);
        }

        public async Task DeleteDeckCard(int deckCardId)
        {
            throw new NotImplementedException();
            //await _cardRepo.DeleteDeckCard(deckCardId);
        }

        #endregion

    }
}
