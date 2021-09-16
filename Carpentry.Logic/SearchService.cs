using Carpentry.CarpentryData;
using Carpentry.CarpentryData.Models;
using Carpentry.Logic.Models;
using Carpentry.Logic.Models.Search;
using Microsoft.EntityFrameworkCore;
using System;
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

    public class CardOverview
    {
        //public int Id { get; set; }

        //card definition properties
        public int CardId { get; set; }
        public string SetCode { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Text { get; set; }
        public string ManaCost { get; set; }
        public int? Cmc { get; set; }
        public char RarityId { get; set; }
        public string ImageUrl { get; set; }
        public int? CollectorNumber { get; set; }
        public string Color { get; set; }
        public string ColorIdentity { get; set; }
        //prices
        public decimal? Price { get; set; }
        public decimal? PriceFoil { get; set; }
        public decimal? TixPrice { get; set; }
        //counts
        //public int TotalCount { get; set; }
        //public int DeckCount { get; set; }
        //public int InventoryCount { get; set; }
        //public int SellCount { get; set; }
        //(I can add more when I actually need them)        
        //wishlist count

        //Misc
        //public bool? IsFoil { get; set; } //only populated for ByUnique, otherwise NULL
        public DateTime SetReleaseDate { get; set; }

        public List<InventoryCardOverview> InventoryCards { get; set; }
    }

    public class InventoryCardOverview
    {
        public int InventoryCardId { get; set; }
        public bool IsFoil { get; set; }
        //status (in a deck/inventory/buy/sell)
        public int Status { get; set; }
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
        public async Task<List<InventoryOverviewDto>> SearchInventoryCards(InventoryQueryParameter param)
        {
            //Okay, fuck it, filter & select to an InventoryOverviewDto, executing the query, THEN grouping after the fact
            //It will be better than selecting 100% of the veiw 100% of the time

            //Note that I can at least filter first

            //Starting at inventoryCards means I can't include unowned cards in these searches, that's bad

            var cardQuery = GetFilteredCardQuery(param);

            //So, I have a query of all cards relevant to these filters

            //
            var queryResult = await cardQuery
                .Select(c => new CardOverview
                {
                    //Id = ic.InventoryCardId,
                    //card definition props
                    CardId = c.CardId,
                    SetCode = c.Set.Code,
                    Name = c.Name,
                    Type = c.Type,
                    Text = c.Text,
                    ManaCost = c.ManaCost,
                    Cmc = c.Cmc,
                    RarityId = c.RarityId,
                    ImageUrl = c.ImageUrl,
                    CollectorNumber = c.CollectorNumber,
                    Color = c.Color,
                    ColorIdentity = c.ColorIdentity,

                    //prices
                    Price = (decimal?)c.Price,
                    PriceFoil = (decimal?)c.PriceFoil,
                    TixPrice = (decimal?)c.TixPrice,

                    InventoryCards = c.InventoryCards.Select(ic => new InventoryCardOverview()
                    {
                        InventoryCardId = ic.InventoryCardId,
                        IsFoil = ic.IsFoil,
                        //status (in a deck/inventory/buy/sell)
                        //Status = (ic.DeckCards.Count != 0) ? "Deck" : ic.Status.Name,
                        Status = (ic.DeckCards.Count != 0) ? (int)InventoryCardStatus.Deck : ic.InventoryCardStatusId
                    }).ToList(),

                    //counts
                    //TotalCount = 1,
                    //DeckCount = (c.DeckCards.Count == 0) ? 0 : 1,
                    //InventoryCount = (c.DeckCards.Count == 0 && c.InventoryCardStatusId == 1) ? 1 : 0,
                    //SellCount = (c.DeckCards.Count == 0 && c.InventoryCardStatusId == 3) ? 1 : 0,

                    //misc
                    //IsFoil = c.IsFoil,
                    SetReleaseDate = c.Set.ReleaseDate,
                })
                .ToListAsync();

            var groupedResult = GetGroupedCards(queryResult, param.GroupBy);

            //order/skip/take


            switch (param.Sort)
            {
                case "count":
                    groupedResult = groupedResult.OrderByDescending(x => x.TotalCount);
                    break;

                case "name":
                    groupedResult = groupedResult.OrderBy(x => x.Name);
                    break;

                case "cmc":
                    groupedResult = groupedResult.OrderBy(x => x.Cmc).ThenBy(x => x.Name);
                    break;

                case "price":
                    if (param.SortDescending)
                    {
                        groupedResult = groupedResult.OrderByDescending(x => x.Price).ThenBy(x => x.Name);
                    }
                    else
                    {
                        groupedResult = groupedResult.OrderBy(x => x.Price).ThenBy(x => x.Name);
                    }
                    break;

                default:
                    //query = query.OrderByDescending(x => x.Id);
                    groupedResult = groupedResult.OrderBy(x => x.CollectorNumber);
                    break;
            }

            if (param.Take > 0)
            {
                groupedResult = groupedResult.Skip(param.Skip).Take(param.Take);//.OrderByDescending(x => x.Count);
            }

            var result = groupedResult.ToList();

            return result;
        }

        private IQueryable<CardData> GetFilteredCardQuery(InventoryQueryParameter param)
        {
            var cardQuery = _cardContext.Cards.AsQueryable();

            if (!string.IsNullOrEmpty(param.Set))
            {
                cardQuery = cardQuery.Where(ic => ic.Set.Code.ToLower() == param.Set.ToLower());
            }

            if (param.Colors != null && param.Colors.Any())
            {
                //atm I'm trying to be strict in my matching.  If a color isn't in the list, I'll exclude any card containing that color
                var excludedColors = _allColors.Where(x => !param.Colors.Contains(x)).Select(x => x).ToList();
                cardQuery = cardQuery.Where(c => !c.ColorIdentity.ToCharArray().Any(color => excludedColors.Contains(color.ToString())));
            }

            if (param.ExclusiveColorFilters)
            {
                cardQuery = cardQuery.Where(c => c.ColorIdentity.Length == param.Colors.Count());
            }

            if (param.MultiColorOnly)
            {
                cardQuery = cardQuery.Where(c => c.ColorIdentity.Length > 1);
            }

            if (!string.IsNullOrEmpty(param.Type))
            {
                cardQuery = cardQuery.Where(c => c.Type.ToLower().Contains(param.Type.ToLower()));
            }

            if (param.Rarity != null && param.Rarity.Any())
            {
                cardQuery = cardQuery.Where(c => param.Rarity.Contains(c.RarityId.ToString()));
            }

            if (!string.IsNullOrEmpty(param.Text))
            {
                cardQuery = cardQuery.Where(c =>
                    (c.Text != null && c.Text.ToLower().Contains(param.Text.ToLower()))
                    ||
                    c.Name.ToLower().Contains(param.Text.ToLower())
                    ||
                    c.Type.ToLower().Contains(param.Text.ToLower())
                );
            }

            return cardQuery;
        }

        private IEnumerable<InventoryOverviewDto> GetGroupedCards(List<CardOverview> queryResult, string groupBy)
        {
            var result = new List<InventoryOverviewDto>().AsEnumerable();

            switch (groupBy)
            {
                case nameof(CardSearchGroupBy.name):
                    result = queryResult.GroupBy(c => c.Name)
                        .Select(g => new
                        {
                            MostRecentCard = g.OrderByDescending(c => c.SetReleaseDate).ThenBy(c => c.CollectorNumber).First(),
                            InventoryCards = g.SelectMany(c => c.InventoryCards).ToList(),
                        })
                        .Select(g => new InventoryOverviewDto()
                        {
                            //for tracking uniqueness
                            Id = g.MostRecentCard.CardId,
                            //card definition props
                            CardId = g.MostRecentCard.CardId,
                            SetCode = g.MostRecentCard.SetCode,
                            Name = g.MostRecentCard.Name,
                            Type = g.MostRecentCard.Type,
                            Text = g.MostRecentCard.Text,
                            ManaCost = g.MostRecentCard.ManaCost,
                            Cmc = g.MostRecentCard.Cmc,
                            RarityId = g.MostRecentCard.RarityId,
                            ImageUrl = g.MostRecentCard.ImageUrl,
                            CollectorNumber = g.MostRecentCard.CollectorNumber,
                            Color = g.MostRecentCard.Color,
                            ColorIdentity = g.MostRecentCard.ColorIdentity,
                            //prices
                            Price = g.MostRecentCard.Price,
                            PriceFoil = g.MostRecentCard.PriceFoil,
                            TixPrice = g.MostRecentCard.TixPrice,
                            //counts
                            TotalCount = g.InventoryCards.Count,
                            DeckCount = g.InventoryCards.Where(ic => ic.Status == (int)InventoryCardStatus.Deck).Count(),
                            InventoryCount = g.InventoryCards.Where(ic => ic.Status == (int)InventoryCardStatus.Inventory).Count(),
                            SellCount = g.InventoryCards.Where(ic => ic.Status == (int)InventoryCardStatus.SellList).Count(),
                            //misc
                            IsFoil = false,
                        });
                    break;
                case nameof(CardSearchGroupBy.print):
                    result = queryResult.Select(c => new InventoryOverviewDto()
                    {
                        //for tracking uniqueness
                        Id = c.CardId,
                        //card definition props
                        CardId = c.CardId,
                        SetCode = c.SetCode,
                        Name = c.Name,
                        Type = c.Type,
                        Text = c.Text,
                        ManaCost = c.ManaCost,
                        Cmc = c.Cmc,
                        RarityId = c.RarityId,
                        ImageUrl = c.ImageUrl,
                        CollectorNumber = c.CollectorNumber,
                        Color = c.Color,
                        ColorIdentity = c.ColorIdentity,
                        //prices
                        Price = c.Price,
                        PriceFoil = c.PriceFoil,
                        TixPrice = c.TixPrice,
                        //counts
                        TotalCount = c.InventoryCards.Count,
                        DeckCount = c.InventoryCards.Where(ic => ic.Status == (int)InventoryCardStatus.Deck).Count(),
                        InventoryCount = c.InventoryCards.Where(ic => ic.Status == (int)InventoryCardStatus.Inventory).Count(),
                        SellCount = c.InventoryCards.Where(ic => ic.Status == (int)InventoryCardStatus.SellList).Count(),
                        //misc
                        IsFoil = false,
                    });
                    break;
                case nameof(CardSearchGroupBy.unique):
                    //TODO - Find a way to combine these into a single linq query
                    //Get all foil cards
                    var foilCards = queryResult.Select(c => new InventoryOverviewDto()
                    {
                        //for tracking uniqueness
                        Id = c.CardId,
                        //card definition props
                        CardId = c.CardId,
                        SetCode = c.SetCode,
                        Name = c.Name,
                        Type = c.Type,
                        Text = c.Text,
                        ManaCost = c.ManaCost,
                        Cmc = c.Cmc,
                        RarityId = c.RarityId,
                        ImageUrl = c.ImageUrl,
                        CollectorNumber = c.CollectorNumber,
                        Color = c.Color,
                        ColorIdentity = c.ColorIdentity,
                        //prices
                        Price = c.Price,
                        PriceFoil = c.PriceFoil,
                        TixPrice = c.TixPrice,
                        //counts
                        TotalCount = c.InventoryCards.Where(ic => ic.IsFoil).Count(),
                        DeckCount = c.InventoryCards.Where(ic => ic.IsFoil && ic.Status == (int)InventoryCardStatus.Deck).Count(),
                        InventoryCount = c.InventoryCards.Where(ic => ic.IsFoil && ic.Status == (int)InventoryCardStatus.Inventory).Count(),
                        SellCount = c.InventoryCards.Where(ic => ic.IsFoil && ic.Status == (int)InventoryCardStatus.SellList).Count(),
                        //misc
                        IsFoil = true,
                    })
                    .Where(c => c.TotalCount > 0);

                    //get all nonfoil cards
                    var nonfoilCards = queryResult.Select(c => new InventoryOverviewDto()
                    {
                        //for tracking uniqueness
                        Id = c.CardId,
                        //card definition props
                        CardId = c.CardId,
                        SetCode = c.SetCode,
                        Name = c.Name,
                        Type = c.Type,
                        Text = c.Text,
                        ManaCost = c.ManaCost,
                        Cmc = c.Cmc,
                        RarityId = c.RarityId,
                        ImageUrl = c.ImageUrl,
                        CollectorNumber = c.CollectorNumber,
                        Color = c.Color,
                        ColorIdentity = c.ColorIdentity,
                        //prices
                        Price = c.Price,
                        PriceFoil = c.PriceFoil,
                        TixPrice = c.TixPrice,
                        //counts
                        TotalCount = c.InventoryCards.Where(ic => !ic.IsFoil).Count(),
                        DeckCount = c.InventoryCards.Where(ic => !ic.IsFoil && ic.Status == (int)InventoryCardStatus.Deck).Count(),
                        InventoryCount = c.InventoryCards.Where(ic => !ic.IsFoil && ic.Status == (int)InventoryCardStatus.Inventory).Count(),
                        SellCount = c.InventoryCards.Where(ic => !ic.IsFoil && ic.Status == (int)InventoryCardStatus.SellList).Count(),
                        //misc
                        IsFoil = false,
                    })
                    .Where(c => c.TotalCount > 0);
                    result = foilCards.Concat(nonfoilCards);
                    break;
                default: return result;

            }

            return result;
        }

        public async Task<List<InventoryOverviewDto>> SearchInventoryCards_KindaFailedAttempt(InventoryQueryParameter param)
        {
            //Okay, fuck it, filter & select to an InventoryOverviewDto, executing the query, THEN grouping after the fact
            //It will be better than selecting 100% of the veiw 100% of the time

            //Note that I can at least filter first


            /* Thought process for handling this query:
                I'm trying to be efficient and do as much data processing on the server as possible
                In reality, I currently own 30k cards, and should cut my collection down to less than that
                Even if I owned 100k cards, that's not TOO MUCH data to grab at once, it just gets a little nasty
             
                I'm really struggling with grouping with EF core
             
             */

            //var cards = from c in _cardContext.Cards


            //var cardCounts = OH RIGHT SOME PIVOT BULLSHIT

            //var cardTotals = _cardContext.InventoryCards
            //    .Select(ic => new
            //    {
            //        ic.InventoryCardId,
            //        ic.CardId,
            //        ic.IsFoil,
            //        Location = ic.DeckCards.Count != 0 ? "Deck" : ic.Status.Name,
            //    }).ToList();

            var cardTotalsQuery =
                from ic in _cardContext.InventoryCards
                select new
                {
                    ic.InventoryCardId,
                    ic.CardId,
                    ic.IsFoil,
                    Location = ic.DeckCards.Count != 0 ? "Deck" : ic.Status.Name,
                };


            var groupTest =
                from ic in cardTotalsQuery
                //Note I eventually want to group by CardId + IsFoil when getting unique by print
                //For now, this is fine
                group ic by ic.CardId into g 
                select new
                {
                    CardId = g.Key,
                    TotalCount = g.Count(),
                    //DeckCount = g.Where(ic => ic.Location == "Deck").Count(),
                };

            var cardTotals = cardTotalsQuery.ToList();

            var groupTestResult = groupTest.ToList();
            //I need to rethink how I'm querying this data entirely
            //The subject of the search is Inventory Cards (at 31k at time of writing)
            //Those records could be filtered before they are grouped, instead of grouped from the start



            //Grouping options
            //  Name
            //  Card Id
            //  Card Id + IsFoil


            var filteredCards = _cardContext.InventoryCards.AsQueryable();


            if (!string.IsNullOrEmpty(param.Set))
            {
                filteredCards = filteredCards.Where(ic => ic.Card.Set.Code == param.Set.ToLower());
            }
            //No matter what, results will be grouped by SOMETHING
            //switch (param.GroupBy)
            //{
            //    case "name":
            //        //query = query.GroupBy(ic => ic.Card.Name);
            //        break;

            //    case "unique":

            //        break;

            //    //case "print":
            //    default: //assuming group by print for default

            //        break;
            //}
            var query = from c in filteredCards
                        group c by c.Card.Name into g
                        select new
                        {

                        };

            var qResult = await query.ToListAsync();





            //query = query.Take(1000);
            var queryResult = await filteredCards.ToListAsync();







            var grouped = await filteredCards
                .GroupBy(ic => ic.Card.Name)
                //This gets me back to my previous issue
                //I want an effecient way of knowing the most recent card for a given name
                
                .Select(g => new
                {
                    //Name = g.Key,
                    //InventoryCards = g.ToList(),
                    FirstInventoryCard = g.OrderByDescending(ic => ic.Card.Set.ReleaseDate).First(),
                    //most recent print
                    //all inventory cards in the group
                })
                .ToListAsync();

            //TODO - add remaining filters, grouping, and sorting

            //var result = await query.Select(ic => new InventoryOverviewDto()
            //{
            //    //CardId = x.CardId,
            //    //Cmc = x.Cmc,
            //    //CollectorNumber = x.CollectorNumber,
            //    //Color = x.Color,
            //    //ColorIdentity = x.ColorIdentity,
            //    //DeckCount = x.DeckCount,
            //    //Id = x.Id,
            //    //ImageUrl = x.ImageUrl,
            //    //InventoryCount = x.InventoryCount,
            //    //SellCount = x.SellCount,
            //    //TotalCount = x.TotalCount,
            //    //TixPrice = x.TixPrice,
            //    //PriceFoil = x.PriceFoil,
            //    //Price = x.Price,
            //    //Name = x.Name,
            //    //ManaCost = x.ManaCost,
            //    //IsFoil = x.IsFoil,
            //    //RarityId = x.RarityId,
            //    //SetCode = x.SetCode,
            //    //Text = x.Text,
            //    //Type = x.Type,
            //}).ToListAsync();




            return new List<InventoryOverviewDto>();
            //return result;
        }

        /// <summary>
        /// Searches inventory cards for the Inventory container
        /// </summary>
        /// <returns></returns>
        public async Task<List<InventoryOverviewDto>> SearchInventoryCards_Legacy(InventoryQueryParameter param)
        {
            
            //TODO - Work on this, it seems like I'm pulling in the whole view
            //Consider updating all views to return the same columns/model
            //that way I can apply the same filtering regardless of where I get my IQueryable
            IEnumerable<CardOverviewResult> query;

            switch (param.GroupBy)
            {
                case "name":
                    //var altQuery = _cardContext.QueryCardsByName().ToListAsync();
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
