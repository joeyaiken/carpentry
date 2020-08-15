using Carpentry.Data.DataContext;
using Carpentry.Data.DataModels;
using Carpentry.Data.DataModels.QueryResults;
using Carpentry.Data.Interfaces;
using Carpentry.Data.Models;
using Carpentry.Data.QueryParameters;
using Carpentry.Data.QueryResults;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carpentry.Data.Implementations
{
    public class InventoryDataRepo : IInventoryDataRepo
    {
        private readonly CarpentryDataContext _cardContext;
        private readonly string[] _allColors;
        public InventoryDataRepo(CarpentryDataContext cardContext)
        {
            _cardContext = cardContext;

            _allColors = new string[] { "W", "U", "B", "R", "G" };
        }


        #region private

        private static IQueryable<CardDataDto> MapInventoryQueryToScryfallDto(IQueryable<CardData> query)
        {
            IQueryable<CardDataDto> result = query.Select(card => new CardDataDto()
            {
                CardId = card.Id,
                Cmc = card.Cmc,
                ManaCost = card.ManaCost,
                MultiverseId = card.Id,
                Name = card.Name,
                ImageUrl = card.ImageUrl,
                Price = card.Price,
                TixPrice = card.TixPrice,
                PriceFoil = card.PriceFoil,
                CollectorNumber = card.CollectorNumber ?? 0, //TODO -remove the "?? 0"

                //Variants = card.Variants.Select(v => new CardVariantDto()
                //{
                //    Image = v.ImageUrl,
                //    Name = v.Type.Name,
                //    Price = v.Price,
                //    PriceFoil = v.PriceFoil,
                //}).ToList(),
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
                //Colors = card.CardColors.Select(c => c.ManaType.Name).ToList(),
                Colors = card.Color.Split().ToList(),
                ColorIdentity = card.ColorIdentity.Split().ToList(),
                Rarity = card.Rarity.Name,
                Set = card.Set.Code,
                //SetId = card.Set.Id,
                Text = card.Text,
                Type = card.Type,
                //ColorIdentity = card.CardColorIdentities.Select(i => i.ManaType.Name).ToList(),
                Legalities = card.Legalities.Select(l => l.Format.Name).ToList(),
            }); ;
            return result;
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
                //

                //atm I'm trying to be strict in my matching.  If a color isn't in the list, I'll exclude any card containing that color

                var excludedColors = _allColors.Where(x => !filters.Colors.Contains(x)).Select(x => x).ToList();

                ////var includedColors = filters.Colors;

                ////only want cards where every color is an included color
                ////cardsQuery = cardsQuery.Where(x => !x.CardColorIdentities.Any() || x.CardColorIdentities.Any(color => includedColors.Contains(color.ManaTypeId.ToString())));

                ////alternative query, no excluded colors
                ////cardsQuery = cardsQuery.Where(x => !(x.CardColorIdentities.Any(color => excludedColors.Contains(color.ManaTypeId))));
                //cardsQuery = cardsQuery.Where(x => x.ColorIdentity.Split("").ToList().Contains  // .Any(color => excludedColors.Contains(color)))

                cardsQuery = cardsQuery.Where(x => x.ColorIdentity.Split().ToList().Any(color => excludedColors.Contains(color)));
            }

            if (!string.IsNullOrEmpty(filters.Format))
            {
                //var matchingLegality = _cardContext.MagicFormats.Where(x => x.Name.ToLower() == param.Format.ToLower()).FirstOrDefault();
                var matchingFormatId = await GetFormatIdByName(filters.Format);
                cardsQuery = cardsQuery.Where(x => x.Legalities.Where(l => l.FormatId == matchingFormatId).Any());
            }

            if (filters.ExclusiveColorFilters)
            {
                cardsQuery = cardsQuery.Where(x => x.ColorIdentity.Length == filters.Colors.Count());
            }

            if (filters.MultiColorOnly)
            {
                cardsQuery = cardsQuery.Where(x => x.ColorIdentity.Length > 1);
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

        #endregion


        public async Task<InventoryCardData> GetInventoryCardById(int inventoryCardId)
        {
            var result = await _cardContext.InventoryCards.Where(x => x.Id == inventoryCardId).FirstOrDefaultAsync();
            return result;
        }

        /// <summary>
        /// Adds a new card to the inventory
        /// Does not handle adding deck cards
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<int> AddInventoryCard(InventoryCardData cardToAdd)
        {
            var matchingCard = _cardContext.Cards.FirstOrDefault(x => x.Id == cardToAdd.CardId);
            var first6Card = _cardContext.Cards.FirstOrDefault();
            _cardContext.InventoryCards.Add(cardToAdd);
            await _cardContext.SaveChangesAsync();

            return cardToAdd.Id;
        }

        public async Task AddInventoryCardBatch(List<InventoryCardData> cardBatch)
        {
            _cardContext.InventoryCards.AddRange(cardBatch);
            await _cardContext.SaveChangesAsync();
        }


        /// <summary>
        /// Updates an inventory card
        /// In theory, the only fieds I'd practically want to update would be Status and maybe IsFoil??
        /// This one might need to wait until variants are handled better...
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task UpdateInventoryCard(InventoryCardData cardToUpdate)
        {
            //todo - actually check if exists?
            _cardContext.InventoryCards.Update(cardToUpdate);
            await _cardContext.SaveChangesAsync();
        }

        /// <summary>
        /// Deletes a card from the inventory
        /// Can only delete cards that don't belong to a deck
        /// </summary>
        /// <param name="id">Id of card to delete</param>
        /// <returns></returns>
        public async Task DeleteInventoryCard(int id)
        {
            var deckCardsReferencingThisCard = _cardContext.DeckCards.Where(x => x.DeckId == id).Count();

            if (deckCardsReferencingThisCard > 0)
            {
                throw new Exception("Cannot delete a card that's currently in a deck");
            }

            var cardToRemove = _cardContext.InventoryCards.First(x => x.Id == id);

            _cardContext.InventoryCards.Remove(cardToRemove);

            await _cardContext.SaveChangesAsync();
        }

        public async Task<bool> DoInventoryCardsExist()
        {
            InventoryCardData firstCard = await _cardContext.InventoryCards.FirstOrDefaultAsync();
            return (firstCard != null);
        }

        public async Task<IEnumerable<CardOverviewResult>> GetInventoryOverviews(InventoryQueryParameter param)
        {

            //TODO - filtering should be moved to the logic layer

            //var cardsQuery = await QueryFilteredCards(param);

            IEnumerable<CardOverviewResult> query;

            //var test = QueryCardsByUnique().Take(100).ToList();

            //var anotherTest = QueryCardsByUnique().ToList();

            //var doesThisBreak = QueryCardsByUnique().Select(x => x).ToList();

            //var confusion = QueryCardsByUnique().Select(x => new CardOverviewResult() { }).ToList();

            //var queryTest = QueryCardsByUnique()

            //            .Select((x, i) => new CardOverviewResult()
            //            {
            //                //Id = i + 1, //When querying by unique, the MID is NOT a unique value
            //                //SetCode = x.SetCode,
            //                //Cmc = x.Cmc,
            //                //Cost = x.ManaCost,
            //                //Count = x.CardCount,
            //                //Img = x.ImageUrl,
            //                //Name = x.Name,
            //                //Type = x.Type,
            //                //IsFoil = x.IsFoil,
            //                //Price = x.Price,
            //                //Variant = x.VariantName,
            //                //Category = null,
            //                SetCode = "abcd",

            //            }).ToList();




            switch (param.GroupBy)
            {
                case "name":

                    query = QueryCardsByName().AsEnumerable()

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
                    query = QueryCardsByUnique().AsEnumerable()

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
                    query = QueryCardsByCustom().AsEnumerable()

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

                    query = QueryCardsByMid().AsEnumerable()

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
                //

                //atm I'm trying to be strict in my matching.  If a color isn't in the list, I'll exclude any card containing that color

                //var excludedColors = _allColors.Where(x => !param.Colors.Contains(x)).Select(x => x).ToList();

                //query = query.Where(x => x.ColorIdentity.Split().ToList().Any(color => excludedColors.Contains(color)));
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

        public IQueryable<InventoryCardByNameResult> QueryCardsByName()
        {
            return _cardContext.InventoryCardByName.AsQueryable();
        }

        public IQueryable<InventoryCardByMidResult> QueryCardsByMid()
        {
            return _cardContext.InventoryCardByMid.AsQueryable();
        }

        public IQueryable<InventoryCardByUniqueResult> QueryCardsByUnique()
        {
            return _cardContext.InventoryCardByUnique.AsQueryable();
        }

        public IQueryable<InventoryCardByCustomResult> QueryCardsByCustom()
        {
            return _cardContext.InventoryCardByCustom.AsQueryable();
        }

        
        //public async Task<IEnumerable<CardOverviewResult>> GetDeckCardOverviews(int deckId)
        //{
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




        public async Task<IEnumerable<InventoryCardResult>> GetInventoryCardsByName(string cardName)
        {
            List<InventoryCardResult> inventoryCards = await _cardContext.Cards.Where(x => x.Name == cardName)
                .SelectMany(x => x.InventoryCards)
                .Select(x => new InventoryCardResult()
                {
                    Id = x.Id,
                    IsFoil = x.IsFoil,
                    InventoryCardStatusId = x.InventoryCardStatusId,
                    //MultiverseId = x.MultiverseId,
                    //VariantType = x.VariantType.Name,
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
                var excludedColors = _allColors.Where(x => !filters.Colors.Contains(x)).Select(x => x).ToList();
                cardsQuery = cardsQuery.Where(x => x.ColorIdentity.Split().ToList().Any(color => excludedColors.Contains(color)));
            }

            if (!string.IsNullOrEmpty(filters.Format))
            {
                //var matchingLegality = _cardContext.MagicFormats.Where(x => x.Name.ToLower() == param.Format.ToLower()).FirstOrDefault();
                var matchingFormatId = await GetFormatIdByName(filters.Format);
                cardsQuery = cardsQuery.Where(x => x.Legalities.Where(l => l.FormatId == matchingFormatId).Any());
            }

            if (filters.ExclusiveColorFilters)
            {
                cardsQuery = cardsQuery.Where(x => x.ColorIdentity.Length == filters.Colors.Count());
            }

            if (filters.MultiColorOnly)
            {
                cardsQuery = cardsQuery.Where(x => x.ColorIdentity.Length > 1);
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


        //NOTE: This is the query that needs to be updated for CardSearch, it's probably useless though, and should be replaced by SearchInventoryCards
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
                var excludedColors = _allColors.Where(x => !filters.ColorIdentity.Contains(x)).Select(x => x).ToList();
                query = query.Where(x => x.ColorIdentity.Split().ToList().Any(color => excludedColors.Contains(color)));

            }

            if (filters.ExclusiveColorFilters)
            {
                query = query.Where(x => x.ColorIdentity.Length == filters.ColorIdentity.Count());
            }

            if (filters.MultiColorOnly)
            {
                query = query.Where(x => x.ColorIdentity.Length > 1);
            }

            //query = query.Where(x => filters.Rarity.Contains(x.Rarity.ToLower()));

            if (filters.Rarity.DefaultIfEmpty().Any() && filters.Rarity.Count() > 0)
            {
                //rarity values coming in are char codes, not names
                query = query.Where(x => filters.Rarity.Contains(x.Rarity.Id.ToString()));
                //query = query.Where(x => filters.Rarity.Contains(x.Rarity.Name.ToLower()));

            }

            query = query.OrderBy(x => x.Name);

            List<CardDataDto> mappedResult = MapInventoryQueryToScryfallDto(query).ToList();

            return mappedResult;
        }


    }
}
