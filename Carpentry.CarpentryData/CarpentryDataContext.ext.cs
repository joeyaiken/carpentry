using Carpentry.CarpentryData.Models;
using Carpentry.CarpentryData.Models.QueryResults;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Carpentry.CarpentryData
{
    /// <summary>
    /// Common queries for the Carpentry database
    /// </summary>
    public partial class CarpentryDataContext
    {

        public IQueryable<DeckData> ExampleDeckQuery() => Decks.Where(d => d.Format.Name == "commander");

        public async Task ExecuteSqlScript(string scriptName)
        {
            string codeBase = Assembly.GetExecutingAssembly().CodeBase;

            UriBuilder uri = new UriBuilder(codeBase);
            string path = Uri.UnescapeDataString(uri.Path);
            var baseDir = Path.GetDirectoryName(path) + $"\\DataScripts\\{scriptName}.sql";
            var scriptContents = File.ReadAllText(baseDir);

#pragma warning disable CS0618 // Type or member is obsolete
            await Database.ExecuteSqlRawAsync(scriptContents);
#pragma warning restore CS0618 // Type or member is obsolete
        }

		//Attempt at replacing a view with linq query
		public IQueryable<CardOverviewResult> QueryCardsByUnique()
		{
			/*
			 SELECT	c.CardId
			,s.Code AS SetCode
			,c.Name
			,c.Type
			,c.ManaCost
			,c.Cmc
			,c.RarityId
			,c.CollectorNumber
			,c.Color
			,c.ColorIdentity
			,Totals.IsFoil
			,CASE WHEN Totals.IsFoil = 1
				THEN PriceFoil
				ELSE Price
			END AS Price
			,Totals.CardCount AS OwnedCount
			,Totals.DeckCount
			,c.ImageUrl
	FROM (
		SELECT	ic.CardId
				,ic.IsFoil
				,COUNT(ic.InventoryCardId) as CardCount
				,SUM(CASE WHEN dc.DeckCardId IS NULL THEN 0 ELSE 1 END) AS DeckCount
		FROM	InventoryCards ic
	LEFT JOIN	DeckCards dc
			ON	ic.InventoryCardId = dc.InventoryCardId
		GROUP BY ic.CardId, ic.IsFoil

	) AS Totals
	JOIN		Cards c
		ON		c.CardId = Totals.CardId
	JOIN		Rarities r
		ON		c.RarityId = r.RarityId
	JOIN		Sets s
		ON		c.SetId = s.SetId
			 
			 */


			//var totals =
			//    from ic in _cardContext.InventoryCards
			//    join dc in _cardContext.DeckCards
			//    on ic.InventoryCardId equals dc.DeckCardId into icdc
			//    from dc in icdc.DefaultIfEmpty()
			//    group new
			//    {
			//        ic.CardId,
			//        ic.IsFoil,
			//        ic.InventoryCardId,
			//        dc.DeckCardId,
			//    }
			//    by new { ic.CardId, ic.IsFoil } into countGroup
			//    select new
			//    {
			//        countGroup.Key.CardId,
			//        countGroup.Key.IsFoil,
			//        CardCount = countGroup.Count(),
			//        DeckCount = countGroup.Where(g => g.DeckCardId != 0).Count(),
			//    };

			//var query =
			//    from t in totals

			//    join c in _cardContext.Cards
			//    on t.CardId equals c.CardId

			//    join s in _cardContext.Sets
			//    on c.SetId equals s.SetId

			//    select new InventoryCardByUniqueResult()
			//    {
			//        CardId = c.CardId,
			//        SetCode = s.Code,
			//        Name = c.Name,
			//        Type = c.Type,
			//        ManaCost = c.ManaCost,
			//        Cmc = c.Cmc,
			//        RarityId = c.RarityId,
			//        CollectorNumber = c.CollectorNumber,
			//        Color = c.Color,
			//        ColorIdentity = c.ColorIdentity,
			//        IsFoil = t.IsFoil,
			//        Price = (t.IsFoil ? c.PriceFoil : c.Price),
			//        OwnedCount = t.CardCount,
			//        DeckCount = t.DeckCount,
			//        ImageUrl = c.ImageUrl,
			//    };

			//var res = query.Take(100).ToList();

			//return query;

			return InventoryCardByUnique.AsQueryable();
		}

		//Attempt at replacing a view with linq query
		public IQueryable<CardOverviewResult> QueryCardsByName()
		{
			/*
			SELECT	RecentCard.CardId
			,RecentCard.Name
			,RecentCard.Type
			,RecentCard.ManaCost
			,RecentCard.Cmc
			,RecentCard.ImageUrl
			,RecentCard.Color
			,RecentCard.ColorIdentity
			,Counts.OwnedCount
			,Counts.DeckCount
	FROM	(

		SELECT		c.Name
					,COUNT(ic.InventoryCardId) AS OwnedCount
					,SUM(CASE WHEN dc.DeckCardId IS NULL THEN 0 ELSE 1 END) AS DeckCount
		FROM		Cards c
		LEFT JOIN	InventoryCards ic
				ON	ic.CardId = c.CardId
		LEFT JOIN	DeckCards dc
				ON	ic.InventoryCardId = dc.InventoryCardId
		GROUP BY	c.Name


	) AS Counts
	JOIN	(
		SELECT	ROW_NUMBER() OVER(PARTITION BY c.Name ORDER BY s.ReleaseDate DESC) AS CardRank
				--,s.Name
				,c.CardId AS CardId
				,c.Name
				,c.Type
				,c.ManaCost
				,c.Cmc
				,c.ImageUrl
				,c.Color
				,c.ColorIdentity
		FROM	Cards c
		JOIN	Sets s
			ON	c.SetId = s.SetId
	) AS RecentCard
		ON	Counts.Name = RecentCard.Name
		AND	RecentCard.CardRank = 1
			 */


			//var mostRecentPrintByName =
			//    from c in _cardContext.Cards
			//    orderby c.Set.ReleaseDate descending, c.CollectorNumber
			//    group c by c.Name into g
			//    select g.First();


			//var countsByName = (
			//    from c in _cardContext.Cards

			//    join ic in _cardContext.InventoryCards
			//    on c.CardId equals ic.CardId into cic

			//    from ic in cic.DefaultIfEmpty()

			//    join dc in _cardContext.DeckCards
			//    on ic.InventoryCardId equals dc.InventoryCardId into dcg
			//    from dc in dcg.DefaultIfEmpty()
			//    group new
			//    {
			//        Name = c.Name,
			//        InventoryCardId = ic.InventoryCardId,
			//        DeckCardId = dc.DeckCardId,
			//    } by c.Name into aGroup
			//    select new
			//    {
			//        Name = aGroup.Key,
			//        OwnedCount = aGroup.Count(),
			//        DeckCount = aGroup.Where(abc => abc.DeckCardId != 0).Count()
			//    });


			//var inventoryCardsByName =
			//    from counts in countsByName
			//    join print in mostRecentPrintByName
			//    on counts.Name equals print.Name
			//    select new InventoryCardByNameResult()
			//    {
			//        Name = print.Name,
			//        CardId = print.CardId,
			//        Cmc = print.Cmc,
			//        Color = print.Color,
			//        ColorIdentity = print.ColorIdentity,
			//        DeckCount = counts.DeckCount,
			//        ImageUrl = print.ImageUrl,
			//        ManaCost = print.ManaCost,
			//        OwnedCount = counts.OwnedCount,
			//        Type = print.Type,
			//    };

			//from ic in cic.DefaultIfEmpty()

			////join dc in _cardContext.DeckCards
			////on ic.InventoryCardId equals dc.InventoryCardId into dcg
			////from icd in cic.DefaultIfEmpty()

			//select new
			//{
			//    c.Name,
			//    ic.InventoryCardId,
			//    dc.
			//};

			////group c by c.Name into g
			//select new
			//{
			//    //c.Name
			//    Name = g.Key,
			//    //,
			//    //COUNT(ic.InventoryCardId) AS OwnedCount
			//    OwnedCount = g.Count(),
			//    //,
			//    //SUM(CASE WHEN dc.DeckCardId IS NULL THEN 0 ELSE 1 END) AS DeckCount
			//    //DeckCount = g.Select(x => x.dc)
			//};


			//select new
			//{
			//	MostRecentId = g.Select(c => c.CardId).Max(),
			//	g.Key,
			//	OwnedCount = g.Count(),
			//};








			//var inventoryCardsByName = 
			//    from count in countsByName
			//    join c in _cardContext.Cards
			//    on count.MostRecentId equals c.CardId
			//    join cv in _cardContext.


			//return inventoryCardsByName;

			return InventoryCardByName.AsQueryable();
		}

		//I want to submit a list of card names, and get a collection of inventory cards for each name
		public async Task<Dictionary<string, List<InventoryCardData>>> GetInventoryCardsByName(IEnumerable<string> cardNames)
		{
			var result = (await InventoryCards
				.Where(ic => cardNames.Contains(ic.Card.Name))
				.Include(ic => ic.Card).ThenInclude(c => c.Set)
				.Include(ic => ic.DeckCards).ThenInclude(dc => dc.Deck)
				.ToListAsync())
				.GroupBy(ic => ic.Card.Name)
				.ToDictionary(g => g.Key, g => g.ToList());

			return result;
		}

	}
}
