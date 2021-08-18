﻿using Carpentry.CarpentryData;
using Carpentry.CarpentryData.Models;
using Carpentry.Logic.Models;
using Carpentry.Logic.Models.Search;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Carpentry.Logic
{
    public interface ISearchService
    {
        Task<List<InventoryOverviewDto>> SearchInventoryCards(InventoryQueryParameter param);
        Task<List<CardSearchResultDto>> SearchCardDefinitions(CardSearchQueryParameter filters);
    }

    public class SearchService : ISearchService
    {
        private readonly string[] _allColors;
        private readonly CarpentryDataContext _cardContext;

        public SearchService(CarpentryDataContext cardContext)
        {
            _cardContext = cardContext;
            _allColors = new string[] { "W", "U", "B", "R", "G" };
        }

        /// <summary>
        /// Searches card definitions for the Card Search section
        /// </summary>
        /// <param name="filters"></param>
        /// <returns></returns>
        public async Task<List<CardSearchResultDto>> SearchCardDefinitions(CardSearchQueryParameter filters)
        {
            //Thoughts on result object
            //  When searching for cards, I want to see cards listed BY NAME
            //      Scenario 1: adding new cards to inventory.  I'll be filtering by set, so this works great
            //          I'm also grouping my collection by NAME, not by CollectorNumber
            //  When searching for a deck, I'd ALSO like to see things grouped by name, then see what varieties of that card-name I could add

            //  So, do I look at the ByName view, or do I group ByPrint?
            //  Also, how do I fitler on legality?

            //  When searching for cards for a deck, do I load inventory cards with the full list?
            //      I could keep doing my current plan of making an API call for that




            //Scenario 1: searching cards (filtered by set) to add to inventory
            //  Want both owned and unowned cards
            //  Want all possible variations of the card

            //Scenario 2: searching cards to add to deck
            //  Want only owned cards
            //  

            var query = _cardContext.InventoryCardByPrint.AsQueryable();

            //map first or filters first?

            #region Filters

            if (!string.IsNullOrEmpty(filters.Set))
            {
                query = query.Where(x => x.SetCode == filters.Set);
            }

            if (!string.IsNullOrEmpty(filters.Type))
            {
                query = query.Where(x => x.Type.ToLower().Contains(filters.Type.ToLower()));
            }

            //if (filters.ColorIdentity.Any())
            if (filters.ColorIdentity?.Count > 0)
            {
                var excludedColors = _allColors.Where(x => !filters.ColorIdentity.Contains(x)).Select(x => x).ToList();
                //query = query.Where(x => x.ColorIdentity.Split().ToList().Any(color => excludedColors.Contains(color)));
                //query = query.Where(x => x.ColorIdentity.ToCharArray().Any(color => excludedColors.Contains(color.ToString())));
                //query = query.Where(x => !excludedColors.Any(color => x.ColorIdentity.Contains(color)));

                foreach (var color in excludedColors)
                {
                    query = query.Where(x => !x.ColorIdentity.Contains(color));
                }

            }

            if (filters.ExclusiveColorFilters)
            {
                query = query.Where(x => x.ColorIdentity.Length == filters.ColorIdentity.Count());
            }

            if (filters.MultiColorOnly)
            {
                query = query.Where(x => x.ColorIdentity.Length > 1);
            }

            if (
                //filters.Rarity.DefaultIfEmpty().Any() && 
                filters.Rarity?.Count > 0)
            {
                //rarity values coming in are char codes, not names
                query = query.Where(x => filters.Rarity.Contains(x.RarityId.ToString()));
            }

            if (filters.ExcludeUnowned)
            {
                query = query.Where(x => x.TotalCount > 0);
            }

            if (!string.IsNullOrEmpty(filters.Text))
            {
                var textFilter = filters.Text.ToLower();
                query = query.Where(x => 
                x.Name.ToLower().Contains(textFilter)
                || x.Type.ToLower().Contains(textFilter)
                || x.Text.ToLower().Contains(textFilter)
                );
            }

            if (!string.IsNullOrEmpty(filters.SearchGroup))
            {
                switch (filters.SearchGroup)
                {
                    case "Red":
                        query = query.Where(x => x.ColorIdentity == "R" && (x.RarityId == 'C' || x.RarityId == 'U'));
                        break;
                    case "Blue":
                        query = query.Where(x => x.ColorIdentity == "U" && (x.RarityId == 'C' || x.RarityId == 'U'));
                        break;
                    case "Green":
                        query = query.Where(x => x.ColorIdentity == "G" && (x.RarityId == 'C' || x.RarityId == 'U'));
                        break;
                    case "White":
                        query = query.Where(x => x.ColorIdentity == "W" && (x.RarityId == 'C' || x.RarityId == 'U'));
                        break;
                    case "Black":
                        query = query.Where(x => x.ColorIdentity == "B" && (x.RarityId == 'C' || x.RarityId == 'U'));
                        break;
                    case "Multicolored":
                        query = query.Where(x => x.ColorIdentity.Length > 1 && (x.RarityId == 'C' || x.RarityId == 'U'));
                        break;
                    case "Colorless":
                        query = query.Where(x => x.ColorIdentity.Length == 0 && (x.RarityId == 'C' || x.RarityId == 'U'));
                        break;
                    case "Lands":
                        query = query.Where(x => x.Type.Contains("Land") && (x.RarityId == 'C' || x.RarityId == 'U'));// && !x.Type.Contains()
                        break;
                    case "RareMythic":
                        query = query.Where(x => x.RarityId == 'R' || x.RarityId == 'M');
                        break;
                }
            }


            #endregion

            var filteredResults = await query.ToListAsync();

            //Is this a dumb approach?  Trying to get the "first" record now?
            var groupedQuery = filteredResults
                .GroupBy(c => c.Name)
                .Select(g => new 
                {
                    Name = g.Key,
                    First = g.First(),
                    Details = g.Select(c => new CardSearchResultDetail()
                    {
                        CardId = c.CardId,
                        Name = c.Name,
                        CollectionNumber = c.CollectorNumber ?? 0,
                        ImageUrl = c.ImageUrl,
                        Price = c.Price,
                        PriceFoil = c.PriceFoil,
                        PriceTix = c.PriceFoil,
                        SetCode = c.SetCode,
                    }).OrderBy(c => c.CollectionNumber).ToList(),
                }).ToList();

            var results = groupedQuery.Select(x => new CardSearchResultDto()
            {
                //Id = 0, // want to start at 1
                CardId = x.Details.OrderBy(d => d.CollectionNumber).First().CardId,
                Name = x.Name,
                Cmc = x.First.Cmc,
                //ColorIdentity = x.First.ColorIdentity.ToCharArray().Select(c => c.ToString()).ToList(),
                ColorIdentity = x.First.ColorIdentity?.ToCharArray(),
                //Colors = x.First.Color.ToCharArray().Select(c => c.ToString()).ToList(),
                Colors = x.First.Color?.ToCharArray(),
                ManaCost = x.First.ManaCost,
                Type = x.First.Type,
                Details = x.Details,
            })
                .OrderBy(x => x.Name)
                .Take(500)
                .ToList();//Don't want to ever return more than 500, should add actual pagination

            return results;
        }

        /// <summary>
        /// Searches inventory cards for the Inventory container
        /// </summary>
        /// <returns></returns>
        public async Task<List<InventoryOverviewDto>> SearchInventoryCards(InventoryQueryParameter param)
        {

            //TODO - Work on this, it seems like I'm pulling in the whole view
            //Consider updating all views to return the same columns/model
            //that way I can apply the same filtering regardless of where I get my IQueryable
            IEnumerable<CardOverviewResult> query;

            switch (param.GroupBy)
            {
                case "name":
                    query = await _cardContext.InventoryCardByName.ToListAsync(); //remember this executes the query
                    break;

                case "unique":
                    query = await _cardContext.InventoryCardByUnique.ToListAsync(); //remember this executes the query
                    break;

                //case "print":
                default: //assuming group by print for default

                    query = await _cardContext.InventoryCardByPrint.ToListAsync(); //remember this executes the query;
                    break;
            }

            #region Filters

            if (!string.IsNullOrEmpty(param.Set))
            {
                query = query.Where(x => x.SetCode == param.Set.ToLower());
            }

            //if (param.StatusId > 0)
            //{
            //    throw new NotImplementedException();
            //}

            if (param.Colors != null && param.Colors.Any())
            {
                //atm I'm trying to be strict in my matching.  If a color isn't in the list, I'll exclude any card containing that color
                var excludedColors = _allColors.Where(x => !param.Colors.Contains(x)).Select(x => x).ToList();
                query = query.Where(x => !x.ColorIdentity.ToCharArray().Any(color => excludedColors.Contains(color.ToString())));
            }

            //if (!string.IsNullOrEmpty(param.Format))
            //{
            //    throw new NotImplementedException();
            //    ////var matchingLegality = _cardContext.MagicFormats.Where(x => x.Name.ToLower() == param.Format.ToLower()).FirstOrDefault();
            //    //var matchingFormatId = await GetFormatIdByName(param.Format);
            //    //cardsQuery = cardsQuery.Where(x => x.Legalities.Where(l => l.FormatId == matchingFormatId).Any());
            //}

            if (param.ExclusiveColorFilters)
            {
                query = query.Where(x => x.ColorIdentity.Length == param.Colors.Count());
            }

            if (param.MultiColorOnly)
            {
                query = query.Where(x => x.ColorIdentity.Length > 1);
            }

            if (!string.IsNullOrEmpty(param.Type))
            {
                query = query.Where(x => x.Type.ToLower().Contains(param.Type.ToLower())); //Should this be .ToLower() ?
            }

            if (param.Rarity != null && param.Rarity.Any())
            {
                query = query.Where(x => param.Rarity.Contains(x.RarityId.ToString()));
            }

            if (!string.IsNullOrEmpty(param.Text))
            {
                query = query.Where(x =>
                    (x.Text != null && x.Text.ToLower().Contains(param.Text.ToLower()))
                    ||
                    x.Name.ToLower().Contains(param.Text.ToLower())
                    ||
                    x.Type.ToLower().Contains(param.Text.ToLower())
                );
            }

            if (param.MinCount > 0)
            {
                query = query.Where(x => x.TotalCount >= param.MinCount);
            }

            if (param.MaxCount > 0)
            {
                query = query.Where(x => x.TotalCount <= param.MinCount);
            }

            #endregion

            #region Sorting

            switch (param.Sort)
            {
                case "count":
                    query = query.OrderByDescending(x => x.TotalCount);
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
                    //query = query.OrderByDescending(x => x.Id);
                    query = query.OrderBy(x => x.CollectorNumber);
                    break;
            }

            #endregion

            if (param.Take > 0)
            {
                query = query.Skip(param.Skip).Take(param.Take);//.OrderByDescending(x => x.Count);
            }

            List<InventoryOverviewDto> result = query.Select(x => new InventoryOverviewDto
            {
                CardId = x.CardId,
                Cmc = x.Cmc,
                CollectorNumber = x.CollectorNumber,
                Color = x.Color,
                ColorIdentity = x.ColorIdentity,
                DeckCount = x.DeckCount,
                Id = x.Id,
                ImageUrl = x.ImageUrl,
                InventoryCount = x.InventoryCount,
                SellCount = x.SellCount,
                TotalCount = x.TotalCount,
                TixPrice = x.TixPrice,
                PriceFoil = x.PriceFoil,
                Price = x.Price,
                Name = x.Name,
                ManaCost = x.ManaCost,
                IsFoil = x.IsFoil,
                RarityId = x.RarityId,
                SetCode = x.SetCode,
                Text = x.Text,
                Type = x.Type,
            }).ToList();

            return result;

        }

    }
}
