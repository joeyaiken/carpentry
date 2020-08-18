﻿using Carpentry.Data.Interfaces;
using Carpentry.Data.Models;
using Carpentry.Data.QueryParameters;
using Carpentry.Data.QueryResults;
using Carpentry.Logic.Interfaces;
using Carpentry.Logic.Models;
using Carpentry.Logic.Models.Scryfall;
using Carpentry.Logic.Search;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carpentry.Logic.Implementations
{
    public class SearchService : ISearchService
    {
        //Should have no access to data context classes, only repo classes
        private readonly IScryfallService _scryService;
        private readonly IInventoryDataRepo _inventoryRepo;

        public SearchService(
            IInventoryDataRepo inventoryRepo,
            IScryfallService scryService
            )
        {
            _inventoryRepo = inventoryRepo;
            _scryService = scryService;
        }

        /// <summary>
        /// Searches card definitions for the Card Search section
        /// </summary>
        /// <param name="filters"></param>
        /// <returns></returns>
        public async Task<List<MagicCardDto>> SearchCards(InventoryQueryParameter filters)
        {
            var query = _inventoryRepo.QueryCardsByPrint()
                //filters
                ;









            var result = query.Select(x => new MagicCardDto()
            {
                Cmc = x.Cmc,
                CollectionNumber = x.CollectorNumber,
                ColorIdentity = x.ColorIdentity.Split().ToList(),
                Colors = x.Color.Split().ToList(),
                ImageUrl = x.ImageUrl,
                Legalities = null,
                ManaCost = x.ManaCost,
                MultiverseId = x.MultiverseId,
                Name = x.Name,
                Price = x.Price,
                PriceFoil = x.PriceFoil,
                PriceTix = x.TixPrice,
                Rarity = x.RarityId.ToString(),
                Set = x.SetCode,
                Text = x.Text,
                Type = x.Type,
            }).ToList();

            return result;
        }

        /// <summary>
        /// Searches inventory cards for the Inventory section
        /// </summary>
        /// <returns></returns>
        public async Task<List<InventoryOverviewDto>> SearchInventory(InventoryQueryParameter param)
        {
            IEnumerable<CardOverviewResult> query;

            #region query

            switch (param.GroupBy)
            {
                case "name":

                    query = _inventoryRepo.QueryCardsByName().AsEnumerable()

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
                    query = _inventoryRepo.QueryCardsByUnique().AsEnumerable()

                        .Select((x, i) => new CardOverviewResult()
                        {
                            Id = i + 1, //When querying by unique, the MID is NOT a unique value
                            SetCode = x.SetCode,
                            Name = x.Name,
                            Type = x.Type,
                            Cost = x.ManaCost,
                            Cmc = x.Cmc,
                            IsFoil = x.IsFoil,
                            Price = x.Price,
                            Count = x.CardCount ?? 0,
                            Img = x.ImageUrl,
                        });

                    break;

                //case "print":
                default: //assuming group by print for default

                    query = _inventoryRepo.QueryCardsByPrint().AsEnumerable()

                        .Select(x => new CardOverviewResult()
                        {
                            Id = x.CardId,
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

            #endregion

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

            //var executed = query.ToList();

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

            List<CardOverviewResult> result = query.ToList();
            return result;

        }

        //What searches the inventory for the card search container? Same SearchInventory api? or...





        #region Things from InventoryDataRepo

        public async Task<IEnumerable<CardOverviewResult>> GetInventoryOverviews(InventoryQueryParameter param)
        {


            //var aTest = QueryCardsByUnique().ToList();

            //int breakpoint = 1;
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

                    query = _inventoryRepo.QueryCardsByName().AsEnumerable()

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
                    query = _inventoryRepo.QueryCardsByUnique().AsEnumerable()

                        .Select((x, i) => new CardOverviewResult()
                        {
                            Id = i + 1, //When querying by unique, the MID is NOT a unique value
                            SetCode = x.SetCode,
                            Name = x.Name,
                            Type = x.Type,
                            Cost = x.ManaCost,
                            Cmc = x.Cmc,
                            //Rarity
                            //CollectorNumber
                            IsFoil = x.IsFoil,
                            Price = x.Price,
                            Count = x.CardCount ?? 0,
                            Img = x.ImageUrl,
                        });

                    break;

                //case "custom":
                //    query = QueryCardsByCustom().AsEnumerable()

                //        .Select((x, i) => new CardOverviewResult()
                //        {
                //            Id = i + 1, //When querying by unique, the MID is NOT a unique value
                //            SetCode = x.SetCode,
                //            Cmc = x.Cmc,
                //            Cost = x.ManaCost,
                //            Count = x.CardCount,
                //            Img = x.ImageUrl,
                //            Name = x.Name,
                //            Type = x.Type,
                //            IsFoil = x.IsFoil,
                //            Price = x.Price,
                //            Variant = x.VariantName,
                //        });

                //    break;

                //case "mid":
                default: //assuming group by mid for default

                    query = _inventoryRepo.QueryCardsByPrint().AsEnumerable()

                        .Select(x => new CardOverviewResult()
                        {
                            Id = x.CardId,
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


        #endregion

        #region legacy private methods

        //private static IQueryable<MagicCard> MapInventoryQueryToScryfallDto(IQueryable<Data.LegacyDataContext.Card> query)
        //{
        //    IQueryable<MagicCard> result = query.Select(card => new MagicCard()
        //    {
        //        Cmc = card.Cmc,
        //        ManaCost = card.ManaCost,
        //        MultiverseId = card.Id,
        //        Name = card.Name,

        //        //Prices = card.Variants.ToDictionary(v => (v.)  )

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

        //        //Variants = card.Variants.ToDictionary(v => v.Type.Name, v => v.ImageUrl),
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

        private static MagicCardDto MapCardDataToDto(CardDataDto card)
        {
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

            MagicCardDto result = new MagicCardDto()
            {
                Cmc = card.Cmc,
                ManaCost = card.ManaCost,
                MultiverseId = card.MultiverseId,
                Name = card.Name,
                CollectionNumber = card.CollectorNumber,
                ImageUrl = card.ImageUrl,
                Price = card.Price,
                PriceFoil = card.PriceFoil,
                PriceTix = card.TixPrice,
                //        Prices = card.Variants.ToDictionary(v => (v.))

                //Prices = card.Variants.SelectMany(x => new[]
                //{
                //    new {
                //        Name = x.Name,
                //        Price = x.Price,
                //    },
                //    new {
                //        Name = $"{x.Name}_foil",
                //        Price = x.PriceFoil,
                //    }
                //}).ToDictionary(v => v.Name, v => v.Price),

                //        Variants = card.Variants.ToDictionary(v => v.Type.Name, v => v.ImageUrl),
                //Variants = card.Variants.Select(v => new { v.Name, v.Image }).ToDictionary(v => v.Name, v => v.Image),

                Colors = card.Colors,
                Rarity = card.Rarity,
                Set = card.Set,
                Text = card.Text,
                Type = card.Type,
                ColorIdentity = card.ColorIdentity,
                Legalities = card.Legalities,
            };
            return result;
        }

        #endregion

        #region legacy Card Search related methods

        /// <summary>
        /// Returns a list of Card Search (overview) objects, taken from cards in the inventory
        /// </summary>
        /// <param name="filters"></param>
        /// <returns></returns>
        public async Task<IEnumerable<MagicCardDto>> SearchCardsFromInventory(InventoryQueryParameter filters)
        {
            var dbCards = await _inventoryRepo.SearchInventoryCards(filters);

            List<MagicCardDto> mappedCards = dbCards.Select(x => MapCardDataToDto(x)).ToList();


            //var cardsQuery = await _inventoryRepo.QueryFilteredCards(filters);

            //var query = MapInventoryQueryToScryfallDto(cardsQuery);

            //var groupedQuery = query
            //    .GroupBy(x => x.Name)
            //    .Select(x => x.OrderByDescending(i => i.MultiverseId).First());

            //groupedQuery = groupedQuery.OrderBy(x => x.Name);

            //if (filters.Take > 0)
            //{
            //    groupedQuery = groupedQuery.Skip(filters.Skip).Take(filters.Take);
            //}

            //var result = groupedQuery.ToList();

            return mappedCards;
        }

        /// <summary>
        /// Returns all of the card definitions for a given set, with specified filters applied
        /// </summary>
        /// <param name="filters"></param>
        /// <returns></returns>
        public async Task<IEnumerable<MagicCardDto>> SearchCardsFromSet(CardSearchQueryParameter filters)
        {
            if (string.IsNullOrEmpty(filters.Set))
            {
                throw new ArgumentNullException("Set code filter cannot be null");
            }

            var dbCards = await _inventoryRepo.SearchCardSet(filters);

            List<MagicCardDto> mappedCards = dbCards.Select(x => MapCardDataToDto(x)).ToList();

            return mappedCards;
        }


        public async Task<IEnumerable<MagicCardDto>> SearchCardsFromWeb(NameSearchQueryParameter filters)
        {
            List<ScryfallMagicCard> queryResult = await _scryService.SearchScryfallByName(filters.Name, filters.Exclusive);

            List<MagicCardDto> mappedResult = queryResult.Select(x => x.ToMagicCard()).ToList();

            return mappedResult;
        }

        #endregion
    }
}
