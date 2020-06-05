using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Carpentry.Data.DataContext;
using Carpentry.Data.DataModels;
using Carpentry.Data.DataModels.QueryResults;
using Carpentry.Data.Interfaces;
using Carpentry.Data.Models;
using Carpentry.Data.QueryParameters;
using Carpentry.Data.QueryResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Carpentry.Data.Implementations
{
    /// <summary>
    /// This class will preform all of the overview/detail queries on the DB
    /// It will NOT return DataModel classes
    /// It will return QueryResults classes
    /// It will accept single parameters, either default types, or something in the QueryParameters folder
    /// </summary>

    public class DataQueryService : IDataQueryService
    {
        private readonly CarpentryDataContext _cardContext;
        private readonly ILogger<DataQueryService> _logger;

        public DataQueryService(CarpentryDataContext cardContext, ILogger<DataQueryService> logger)
        {
            _cardContext = cardContext;
            _logger = logger;
        }

        private async Task<IQueryable<CardData>> QueryFilteredCards(InventoryQueryParameter filters)
        {
            var cardsQuery = _cardContext.Cards.AsQueryable();

            if (!string.IsNullOrEmpty(filters.Set))
            {
                var matchingSetId = _cardContext.Sets.Where(x => x.Code.ToLower() == filters.Set.ToLower()).Select(x => x.Id).FirstOrDefault();
                cardsQuery = cardsQuery.Where(x => x.SetId == matchingSetId);
            }

            if (filters.StatusId > 0)
            {
                //cardsQuery = cardsQuery.Where(x => x.)
            }

            if (filters.Colors != null && filters.Colors.Any())
            {
                //var allowedColorIDs = filters.Colors.

                var excludedColors = await _cardContext.ManaTypes.Where(x => !filters.Colors.Contains(x.Id.ToString())).Select(x => x.Id).ToListAsync();

                //var includedColors = filters.Colors;

                //only want cards where every color is an included color
                //cardsQuery = cardsQuery.Where(x => !x.CardColorIdentities.Any() || x.CardColorIdentities.Any(color => includedColors.Contains(color.ManaTypeId.ToString())));

                //alternative query, no excluded colors
                cardsQuery = cardsQuery.Where(x => !(x.CardColorIdentities.Any(color => excludedColors.Contains(color.ManaTypeId))));

            }

            if (!string.IsNullOrEmpty(filters.Format))
            {
                //var matchingLegality = _cardContext.MagicFormats.Where(x => x.Name.ToLower() == param.Format.ToLower()).FirstOrDefault();
                var matchingFormatId = await GetFormatIdByName(filters.Format);
                cardsQuery = cardsQuery.Where(x => x.Legalities.Where(l => l.FormatId == matchingFormatId).Any());
            }

            if (filters.ExclusiveColorFilters)
            {
                cardsQuery = cardsQuery.Where(x => x.CardColorIdentities.Count() == filters.Colors.Count());
            }

            if (filters.MultiColorOnly)
            {
                cardsQuery = cardsQuery.Where(x => x.CardColorIdentities.Count() > 1);
            }

            if (!string.IsNullOrEmpty(filters.Type))
            {
                cardsQuery = cardsQuery.Where(x => x.Type.Contains(filters.Type));
            }

            if (filters.Rarity != null && filters.Rarity.Any())
            {
                cardsQuery = cardsQuery.Where(x => filters.Rarity.Contains(x.Rarity.Name.ToLower()));

            }

            if (!string.IsNullOrEmpty(filters.Text))
            {
                cardsQuery = cardsQuery.Where(x =>
                    x.Text.ToLower().Contains(filters.Text.ToLower())
                    ||
                    x.Name.ToLower().Contains(filters.Text.ToLower())
                    ||
                    x.Type.ToLower().Contains(filters.Text.ToLower())
                );
            }

            return cardsQuery;
        }

        private async Task<int> GetFormatIdByName(string formatName)
        {
            var format = await _cardContext.MagicFormats.Where(x => x.Name.ToLower() == formatName.ToLower()).FirstOrDefaultAsync();
            if (format == null)
            {
                throw new Exception($"Could not find format matching name: {formatName}");
            }
            return format.Id;
        }

        //TODO - this should be moved to the logic layer
        public async Task<IEnumerable<CardOverviewResult>> GetInventoryOverviews(InventoryQueryParameter param)
        {
            //var cardsQuery = await QueryFilteredCards(param);

            IEnumerable<CardOverviewResult> query;

            switch (param.GroupBy)
            {
                case "name":

                    query = QueryCardsByName()

                        .Select((x, i) => new CardOverviewResult
                        {
                            Id = i + 1,
                            Cmc = x.Cmc,
                            Cost = x.ManaCost,
                            Count = x.OwnedCount,
                            Img = x.ImageUrl,
                            Name = x.Name,
                            Type = x.Type,
                        });

                    break;
               
                case "unique":
                    query = QueryCardsByUnique()

                        .Select((x, i) => new CardOverviewResult()
                        {
                            Id = i + 1, //When querying by unique, the MID is NOT a unique value
                            SetCode = x.SetCode,
                            Cmc = x.Cmc,
                            Cost = x.ManaCost,
                            Count = x.CardCount,
                            Img = x.ImageUrl,
                            Name = x.Name,
                            Type = x.Type,
                            IsFoil = x.IsFoil,
                            Price = x.Price,
                            Variant = x.VariantName,
                        });

                    break;

                case "custom":
                    query = QueryCardsByCustom()

                        .Select((x, i) => new CardOverviewResult()
                        {
                            Id = i + 1, //When querying by unique, the MID is NOT a unique value
                            SetCode = x.SetCode,
                            Cmc = x.Cmc,
                            Cost = x.ManaCost,
                            Count = x.CardCount,
                            Img = x.ImageUrl,
                            Name = x.Name,
                            Type = x.Type,
                            IsFoil = x.IsFoil,
                            Price = x.Price,
                            Variant = x.VariantName,
                        });

                    break;

                //case "mid":
                default: //assuming group by mid for default

                    query = QueryCardsByMid()

                        .Select(x => new CardOverviewResult()
                        {
                            Id = x.MultiverseId,
                            SetCode = x.SetCode,
                            Cmc = x.Cmc,
                            Cost = x.ManaCost,
                            Count = x.OwnedCount,
                            Img = x.ImageUrl,
                            Name = x.Name,
                            Type = x.Type,
                        });

                    break;
            }


            #region Filters

            if (!string.IsNullOrEmpty(param.Set))
            {
                //var matchingSetId = _cardContext.Sets.Where(x => x.Code.ToLower() == param.Set.ToLower()).Select(x => x.Id).FirstOrDefault();
                query = query.Where(x => x.SetCode == param.Set.ToLower());
            }

            if (param.StatusId > 0)
            {
                //cardsQuery = cardsQuery.Where(x => x.)
            }

            if (param.Colors != null && param.Colors.Any())
            {
                ////var allowedColorIDs = param.Colors.

                //var excludedColors = await _cardContext.ManaTypes.Where(x => !param.Colors.Contains(x.Id.ToString())).Select(x => x.Id).ToListAsync();

                ////var includedColors = param.Colors;

                ////only want cards where every color is an included color
                ////cardsQuery = cardsQuery.Where(x => !x.CardColorIdentities.Any() || x.CardColorIdentities.Any(color => includedColors.Contains(color.ManaTypeId.ToString())));

                ////alternative query, no excluded colors
                //cardsQuery = cardsQuery.Where(x => !(x.CardColorIdentities.Any(color => excludedColors.Contains(color.ManaTypeId))));

            }

            if (!string.IsNullOrEmpty(param.Format))
            {
                ////var matchingLegality = _cardContext.MagicFormats.Where(x => x.Name.ToLower() == param.Format.ToLower()).FirstOrDefault();
                //var matchingFormatId = await GetFormatIdByName(param.Format);
                //cardsQuery = cardsQuery.Where(x => x.Legalities.Where(l => l.FormatId == matchingFormatId).Any());
            }

            if (param.ExclusiveColorFilters)
            {
                //cardsQuery = cardsQuery.Where(x => x.CardColorIdentities.Count() == param.Colors.Count());
            }

            if (param.MultiColorOnly)
            {
                //cardsQuery = cardsQuery.Where(x => x.CardColorIdentities.Count() > 1);
            }

            if (!string.IsNullOrEmpty(param.Type))
            {
                //cardsQuery = cardsQuery.Where(x => x.Type.Contains(param.Type));
            }

            if (param.Rarity != null && param.Rarity.Any())
            {
                //cardsQuery = cardsQuery.Where(x => param.Rarity.Contains(x.Rarity.Name.ToLower()));

            }

            if (!string.IsNullOrEmpty(param.Text))
            {
                //cardsQuery = cardsQuery.Where(x =>
                //    x.Text.ToLower().Contains(param.Text.ToLower())
                //    ||
                //    x.Name.ToLower().Contains(param.Text.ToLower())
                //    ||
                //    x.Type.ToLower().Contains(param.Text.ToLower())
                //);
            }


            #endregion

            var executed = query.ToList();

            if (param.MinCount > 0)
            {
                query = query.Where(x => x.Count >= param.MinCount);
            }

            if (param.MaxCount > 0)
            {
                query = query.Where(x => x.Count <= param.MinCount);
            }

            switch (param.Sort)
            {
                case "count":
                    query = query.OrderByDescending(x => x.Count);
                    break;

                case "name":
                    query = query.OrderBy(x => x.Name);
                    break;

                case "cmc":
                    query = query.OrderBy(x => x.Cmc)
                        .ThenBy(x => x.Name);
                    break;

                case "price":
                    if (param.SortDescending)
                    {
                        query = query.OrderByDescending(x => x.Price)
                            .ThenBy(x => x.Name);
                    }
                    else
                    {
                        query = query.OrderBy(x => x.Price)
                            .ThenBy(x => x.Name);
                    }
                    break;

                default:
                    query = query.OrderByDescending(x => x.Id);
                    break;
            }

            if (param.Take > 0)
            {
                query = query.Skip(param.Skip).Take(param.Take);//.OrderByDescending(x => x.Count);
            }

            IEnumerable<CardOverviewResult> result = query.ToList();
            return result;
        }


        public IEnumerable<InventoryCardByNameResult> QueryCardsByName()
        {
            return _cardContext.InventoryCardByName.AsQueryable();
        }

        public IEnumerable<InventoryCardByMidResult> QueryCardsByMid()
        {
            return _cardContext.InventoryCardByMid.AsQueryable();
        }

        public IEnumerable<InventoryCardByUniqueResult> QueryCardsByUnique()
        {
            return _cardContext.InventoryCardByUnique.AsQueryable();
        }

        public IEnumerable<InventoryCardByCustomResult> QueryCardsByCustom()
        {
            return _cardContext.InventoryCardByCustom.AsQueryable();
        }

        //public async Task<IEnumerable<CardOverviewResult>> GetDeckCardOverviews(int deckId)
        //{
        //    //throw new NotImplementedException();
        //    //Deck Overviews
        //    var cardOverviewsQuery = _cardContext.DeckCards
        //        .Where(x => x.DeckId == deckId)
        //        .Select(x => new
        //        {
        //            DeckCardCategory = (x.CategoryId != null) ? x.Category.Name : null,
        //            x.InventoryCard.Card,
        //            x.InventoryCard.Card.Variants.FirstOrDefault(v => v.CardVariantTypeId == 1).ImageUrl,
        //        })
        //        //.Include(x => x.Card)
        //        .ToList();

        //    //var rawOverviews = cardOverviewsQuery.ToList();

        //    //var cardOverviewsQuery = _cardRepo.QueryInventoryCardsForDeck(deckId)
        //    //    .Select(x => new {
        //    //        x.Card,
        //    //        x.Card.Variants.FirstOrDefault(v => v.CardVariantTypeId == 1).ImageUrl,


        //    //    })
        //    var cardOverviewQueryResult = cardOverviewsQuery
        //        .GroupBy(x => new
        //        {
        //            x.Card.Name,
        //            x.DeckCardCategory
        //        })
        //        .Select(x => new
        //        {
        //            Name = x.Key.Name,
        //            Item = x.OrderByDescending(c => c.Card.Id).FirstOrDefault(),
        //            Count = x.Count(),
        //        }).ToList();

        //    //remember "InventoryOverview" could be renamed to "CardOverview"

        //    //#error this isnt properly mapping MultiverseId, trying to add it from github
        //    //I was wrong, this isn't supposed to include MID because it's deck cards grouped by NAME
        //    List<CardOverviewResult> result = cardOverviewQueryResult.Select((x, i) => new CardOverviewResult()
        //    {
        //        Id = i + 1,
        //        Cost = x.Item.Card.ManaCost,
        //        Name = x.Name,
        //        Count = x.Count,
        //        Img = x.Item.ImageUrl,
        //        Type = x.Item.Card.Type,
        //        Category = x.Item.DeckCardCategory,
        //        Cmc = x.Item.Card.Cmc,
        //    })//.ToList()
        //    .OrderBy(x => x.Cmc).ThenBy(x => x.Name).ToList();

        //    return result;
        //}

        //public async Task<IEnumerable<InventoryCardResult>> GetDeckInventoryCards(int deckId)
        //{
        //    List<InventoryCardResult> cardDetails = await _cardContext.DeckCards
        //        .Where(d => d.DeckId == deckId)
        //        .Select(c => c.InventoryCard)

        //        .Select(x => new InventoryCardResult()
        //        {
        //            Id = x.Id,
        //            MultiverseId = x.MultiverseId,
        //            InventoryCardStatusId = x.InventoryCardStatusId,
        //            IsFoil = x.IsFoil,
        //            VariantType = x.VariantType.Name,
        //            Name = x.Card.Name,
        //            DeckCards = x.DeckCards.Select(deckCard => new DeckCardResult()
        //            {
        //                DeckId = deckCard.DeckId,
        //                Id = deckCard.Id,
        //                //InventoryCardId = x.Id,
        //                DeckCardCategory = (deckCard.Category != null) ? deckCard.Category.Name : null,
        //            }).ToList(),
        //            Type = x.Card.Type,
        //            Set = x.Card.Set.Code,
        //        }).ToListAsync();

        //    return cardDetails;
        //    //.Include(x => x.Card.Variants)





        //    //List<InventoryCard> cardDetails = _cardRepo.QueryInventoryCardsForDeck(deckId)
        //    //    .Select(x => new InventoryCard()
        //    //    {
        //    //        Id = x.Id,
        //    //        MultiverseId = x.MultiverseId,
        //    //        InventoryCardStatusId = x.InventoryCardStatusId,
        //    //        IsFoil = x.IsFoil,
        //    //        VariantType = x.VariantType.Name,
        //    //        Name = x.Card.Name,
        //    //        DeckCards = x.DeckCards.Select(deckCard => new InventoryDeckCard()
        //    //        {
        //    //            DeckId = deckCard.DeckId,
        //    //            Id = deckCard.Id,
        //    //            InventoryCardId = x.Id,
        //    //            DeckCardCategory = (deckCard.Category != null) ? deckCard.Category.Name : GetCardTypeGroup(x.Card.Type),
        //    //        }).ToList(),
        //    //    }).ToList();


        //}

        public async Task<List<DeckCardResult>> GetDeckCards(int deckId)
        {
            var deckCards = await _cardContext.DeckCards
                .Where(x => x.DeckId == deckId)
                .Select(x => new DeckCardResult()
                {
                    Id = x.Id,
                    Category = x.CategoryId == null ? null : x.Category.Name,
                    Cmc = x.InventoryCard.Card.Cmc,
                    Cost = x.InventoryCard.Card.ManaCost,
                    Img = x.InventoryCard.Card.Variants.FirstOrDefault(v => v.CardVariantTypeId == 1).ImageUrl,
                    IsFoil = x.InventoryCard.IsFoil,
                    MultiverseId = x.InventoryCard.MultiverseId,
                    Name = x.InventoryCard.Card.Name,
                    Set = x.InventoryCard.Card.Set.Code,
                    Type = x.InventoryCard.Card.Type,
                    VariantType = x.InventoryCard.VariantType.Name,
                }).ToListAsync();

            return deckCards;
        }

        public async Task<List<string>> GetDeckColorIdentity(int deckId)
        {

            var deckCardColors = await _cardContext.DeckCards
                .Where(x => x.DeckId == deckId)
                .SelectMany(x => x.InventoryCard.Card.CardColorIdentities)
                .Select(ci => ci.ManaTypeId.ToString())
                .Distinct()
                .ToListAsync();

            var dbDeck = _cardContext.Decks.Where(x => x.Id == deckId).FirstOrDefault();

            if(dbDeck.BasicW > 0 && !deckCardColors.Contains("W"))
            {
                deckCardColors.Add("W");
            }

            if (dbDeck.BasicU > 0 && !deckCardColors.Contains("U"))
            {
                deckCardColors.Add("U");
            }

            if (dbDeck.BasicB > 0 && !deckCardColors.Contains("B"))
            {
                deckCardColors.Add("B");
            }

            if (dbDeck.BasicR > 0 && !deckCardColors.Contains("R"))
            {
                deckCardColors.Add("R");
            }

            if (dbDeck.BasicG > 0 && !deckCardColors.Contains("G"))
            {
                deckCardColors.Add("G");
            }

            return deckCardColors;
        }

        public async Task<int> GetDeckCardCount(int deckId)
        {
            int basicLandCount = await _cardContext.Decks.Where(x => x.Id == deckId).Select(deck => deck.BasicW + deck.BasicU + deck.BasicB + deck.BasicR + deck.BasicG).FirstOrDefaultAsync();
            int cardCount = await _cardContext.DeckCards.Where(x => x.DeckId == deckId).CountAsync();
            return basicLandCount + cardCount;
        }

        public async Task<IEnumerable<DeckCardStatResult>> GetDeckCardStats(int deckId)
        {
            var query = _cardContext.DeckCards.Where(x => x.DeckId == deckId)
                .Select(x => new
                {
                    Card =  x.InventoryCard.Card,
                    Variant = x.InventoryCard.Card.Variants
                        .Where(cardVariant => cardVariant.CardVariantTypeId == x.InventoryCard.VariantTypeId)
                        .FirstOrDefault(),
                    DeckCard = x,
                    IsFoil = x.InventoryCard.IsFoil,
                    //ColorIdentity = x.InventoryCard.Card.CardColorIdentities.SelectMany<char>(ci => ci.ManaTypeId)
                    ColorIdentity = x.InventoryCard.Card.CardColorIdentities.Select(ci => ci.ManaTypeId).ToList(),
                });

            List<DeckCardStatResult> results = await query.Select(x => new DeckCardStatResult()
            {
                CategoryId = x.DeckCard.CategoryId,
                Cmc = x.Card.Cmc,
                ColorIdentity = x.ColorIdentity,
                Price = (x.IsFoil ? x.Variant.PriceFoil : x.Variant.Price),
                Type = x.Card.Type,
            }).ToListAsync();

            return results;

            //List<DeckCardStatResult> results = _cardContext.DeckCards.Where(x => x.DeckId == deckId)
            //    .Select(x => new DeckCardStatResult()
            //    {
            //        CategoryId = x.CategoryId,
            //        Cmc = x.InventoryCard.Card.Cmc,
            //        ColorIdentity = null,
            //        //SHIT this doesn't properly grab foils

            //        Price = x.InventoryCard.Card.Variants
            //            .Where(cardVariant => cardVariant.CardVariantTypeId == x.InventoryCard.VariantTypeId).FirstOrDefault()
            //        Type = x.InventoryCard.Card.Type,


            //    }).ToList();
        }

        public async Task<IEnumerable<InventoryCardResult>> GetInventoryCardsByName(string cardName)
        {
            List<InventoryCardResult> inventoryCards = await _cardContext.Cards.Where(x => x.Name == cardName)
                .SelectMany(x => x.InventoryCards)
                .Select(x => new InventoryCardResult()
                {
                    Id = x.Id,
                    IsFoil = x.IsFoil,
                    InventoryCardStatusId = x.InventoryCardStatusId,
                    MultiverseId = x.MultiverseId,
                    VariantType = x.VariantType.Name,
                    Name = x.Card.Name,
                    Set = x.Card.Set.Code,
                    //DeckCards = x.DeckCards.Select(c => new DeckCardResult()
                    //{
                    //    Id = c.Id,
                    //    DeckId = c.DeckId,
                    //    InventoryCardId = c.InventoryCardId,
                    //    DeckName = c.Deck.Name,
                    //}).ToList()
                })
                .OrderBy(x => x.Id)
                .ToListAsync();

            return inventoryCards;

        }

        public async Task<IEnumerable<CardDataDto>> SearchInventoryCards(InventoryQueryParameter filters)
        {
            var cardsQuery = _cardContext.Cards.AsQueryable();

            if (!string.IsNullOrEmpty(filters.Set))
            {
                var matchingSetId = _cardContext.Sets.Where(x => x.Code.ToLower() == filters.Set.ToLower()).Select(x => x.Id).FirstOrDefault();
                cardsQuery = cardsQuery.Where(x => x.SetId == matchingSetId);
            }

            if (filters.StatusId > 0)
            {
                //cardsQuery = cardsQuery.Where(x => x.)
            }

            if (filters.Colors.Any())
            {
                //var allowedColorIDs = filters.Colors.

                var excludedColors = await _cardContext.ManaTypes.Where(x => !filters.Colors.Contains(x.Id.ToString())).Select(x => x.Id).ToListAsync();

                //var includedColors = filters.Colors;

                //only want cards where every color is an included color
                //cardsQuery = cardsQuery.Where(x => !x.CardColorIdentities.Any() || x.CardColorIdentities.Any(color => includedColors.Contains(color.ManaTypeId.ToString())));

                //alternative query, no excluded colors
                cardsQuery = cardsQuery.Where(x => !(x.CardColorIdentities.Any(color => excludedColors.Contains(color.ManaTypeId))));

            }

            if (!string.IsNullOrEmpty(filters.Format))
            {
                //var matchingLegality = _cardContext.MagicFormats.Where(x => x.Name.ToLower() == param.Format.ToLower()).FirstOrDefault();
                var matchingFormatId = await GetFormatIdByName(filters.Format);
                cardsQuery = cardsQuery.Where(x => x.Legalities.Where(l => l.FormatId == matchingFormatId).Any());
            }

            if (filters.ExclusiveColorFilters)
            {
                cardsQuery = cardsQuery.Where(x => x.CardColorIdentities.Count() == filters.Colors.Count());
            }

            if (filters.MultiColorOnly)
            {
                cardsQuery = cardsQuery.Where(x => x.CardColorIdentities.Count() > 1);
            }

            if (!string.IsNullOrEmpty(filters.Type))
            {
                cardsQuery = cardsQuery.Where(x => x.Type.Contains(filters.Type));
            }

            if (filters.Rarity.DefaultIfEmpty().Any())
            {
                cardsQuery = cardsQuery.Where(x => filters.Rarity.Contains(x.Rarity.Name.ToLower()));

            }

            if (!string.IsNullOrEmpty(filters.Text))
            {
                cardsQuery = cardsQuery.Where(x =>
                    x.Text.ToLower().Contains(filters.Text.ToLower())
                    ||
                    x.Name.ToLower().Contains(filters.Text.ToLower())
                    ||
                    x.Type.ToLower().Contains(filters.Text.ToLower())
                );
            }

            var query = MapInventoryQueryToScryfallDto(cardsQuery);

            var groupedQuery = query
                .GroupBy(x => x.Name)
                .Select(x => x.OrderByDescending(i => i.MultiverseId).First());

            groupedQuery = groupedQuery.OrderBy(x => x.Name);

            if (filters.Take > 0)
            {
                groupedQuery = groupedQuery.Skip(filters.Skip).Take(filters.Take);
            }

            var result = groupedQuery.ToList();

            return result;
        }
        //private static IQueryable<MagicCard> MapInventoryQueryToScryfallDto(IQueryable<Data.LegacyDataContext.Card> query)
        //{
        //    IQueryable<MagicCard> result = query.Select(card => new MagicCard()
        //    {
        //        Cmc = card.Cmc,
        //        ManaCost = card.ManaCost,
        //        MultiverseId = card.Id,
        //        Name = card.Name,

        //        Prices = card.Variants.ToDictionary(v => (v.))

        //        Prices = card.Variants.SelectMany(x => new[]
        //        {
        //                    new {
        //                        Name = x.Type.Name,
        //                        Price = x.Price,
        //                    },
        //                    new {
        //                        Name = $"{x.Type.Name}_foil",
        //                        Price = x.PriceFoil,
        //                    }
        //                }).ToDictionary(v => v.Name, v => v.Price),

        //        Variants = card.Variants.ToDictionary(v => v.Type.Name, v => v.ImageUrl),
        //        Variants = card.Variants.Select(v => new { v.Type.Name, v.ImageUrl }).ToDictionary(v => v.Name, v => v.ImageUrl),
        //        Colors = card.CardColors.Select(c => c.ManaType.Name).ToList(),
        //        Rarity = card.Rarity.Name,
        //        Set = card.Set.Code,
        //        Text = card.Text,
        //        Type = card.Type,
        //        ColorIdentity = card.CardColorIdentities.Select(i => i.ManaType.Name).ToList(),
        //        Legalities = card.Legalities.Select(l => l.Format.Name).ToList(),
        //    });
        //    return result;
        //}

        private static IQueryable<CardDataDto> MapInventoryQueryToScryfallDto(IQueryable<CardData> query)
        {
            IQueryable<CardDataDto> result = query.Select(card => new CardDataDto()
            {
                Cmc = card.Cmc,
                ManaCost = card.ManaCost,
                MultiverseId = card.Id,
                Name = card.Name,
                Variants = card.Variants.Select(v => new CardVariantDto() 
                { 
                    Image = v.ImageUrl,
                    Name = v.Type.Name,
                    Price = v.Price,
                    PriceFoil = v.PriceFoil,
                }).ToList(),
                //Prices = card.Variants.ToDictionary(v => (v.)  )

                //Prices = card.Variants.SelectMany(x => new[]
                //{
                //            new {
                //                Name = x.Type.Name,
                //                Price = x.Price,
                //            },
                //            new {
                //                Name = $"{x.Type.Name}_foil",
                //                Price = x.PriceFoil,
                //            }
                //        }).ToDictionary(v => v.Name, v => v.Price),

                //Variants = card.Variants.ToDictionary(v => v.Type.Name, v => v.ImageUrl),
                //Variants = card.Variants.Select(v => new { v.Type.Name, v.ImageUrl }).ToDictionary(v => v.Name, v => v.ImageUrl),
                Colors = card.CardColors.Select(c => c.ManaType.Name).ToList(),
                Rarity = card.Rarity.Name,
                Set = card.Set.Code,
                Text = card.Text,
                Type = card.Type,
                ColorIdentity = card.CardColorIdentities.Select(i => i.ManaType.Name).ToList(),
                Legalities = card.Legalities.Select(l => l.Format.Name).ToList(),
            });
            return result;
        }

        public async Task<IEnumerable<CardDataDto>> SearchCardSet(CardSearchQueryParameter filters)
        {
            int matchingSetId = _cardContext.Sets
                .Where(x => x.Code.ToLower() == filters.Set.ToLower())
                .Select(x => x.Id)
                .FirstOrDefault();




            var query = _cardContext.Cards
                .Where(x => x.SetId == matchingSetId);
                //.Where(x => x.InventoryCards.Count > 0)
                //.ToList();




            //var allCardsQuery = await _scryContext.Cards.Where(x => x.Set.Code != null && x.Set.Code.ToLower() == setCode.ToLower()).ToListAsync();

            //var query = allCardsQuery.Select(x => JObject.Parse(x.StringData).ToObject<ScryfallMagicCard>());

            //return query.AsQueryable();


            //TODO - This filtering could/should be moved to the Data project
            //IQueryable<ScryfallMagicCard> query = await _inventoryRepo.QueryCardsBySet(filters.SetCode);

            if (!string.IsNullOrEmpty(filters.Type))
            {
                query = query.Where(x => x.Type.ToLower().Contains(filters.Type.ToLower()));
            }

            if (filters.ColorIdentity.Any())
            {
                //var allowedColorIDs = filters.Colors.

                var excludedColors = await _cardContext.ManaTypes.Where(x => !filters.ColorIdentity.Contains(x.Id.ToString())).Select(x => x.Id).ToListAsync();

                //var includedColors = filters.Colors;

                //only want cards where every color is an included color
                //cardsQuery = cardsQuery.Where(x => !x.CardColorIdentities.Any() || x.CardColorIdentities.Any(color => includedColors.Contains(color.ManaTypeId.ToString())));

                //alternative query, no excluded colors
                query = query.Where(x => !(x.CardColorIdentities.Any(color => excludedColors.Contains(color.ManaTypeId))));

            }

            if (filters.ExclusiveColorFilters)
            {
                query = query.Where(x => x.CardColorIdentities.Count() == filters.ColorIdentity.Count());
            }
            
            if (filters.MultiColorOnly)
            {
                query = query.Where(x => x.CardColorIdentities.Count() > 1);
            }

            //query = query.Where(x => filters.Rarity.Contains(x.Rarity.ToLower()));

            if (filters.Rarity.DefaultIfEmpty().Any() && filters.Rarity.Count() > 0)
            {
                query = query.Where(x => filters.Rarity.Contains(x.Rarity.Name.ToLower()));

            }

            query = query.OrderBy(x => x.Name);

            List<CardDataDto> mappedResult = MapInventoryQueryToScryfallDto(query).ToList();

            return mappedResult;
        }

    }
}
