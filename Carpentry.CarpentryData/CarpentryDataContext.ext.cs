using Carpentry.CarpentryData.Models;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
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

		//Replaces vwGetSetTotals
		public IQueryable<SetTotalsResult> GetSetTotals()
        {
			return Sets.Select(set => new SetTotalsResult
			{
				SetId = set.SetId,
				Code = set.Code,
				Name = set.Name,
				ReleaseDate = set.ReleaseDate,
				LastUpdated = set.LastUpdated,
				IsTracked = set.IsTracked,
				InventoryCount = set.Cards.SelectMany(c => c.InventoryCards).Count(),
				CollectedCount = set.Cards.Where(c => c.InventoryCards.Count() > 0).Count(),
				TotalCount = set.Cards.Count(),
			});
		}

		//TODO - consider moving directly to a service class
		//Replaces vwInventoryTotalsByStatus
		public IQueryable<InventoryTotalsByStatusResult> GetInventoryTotalsByStatus()
        {
			return CardStatuses.Select(cs => new InventoryTotalsByStatusResult
			{
				StatusId = cs.CardStatusId,
				StatusName = cs.Name,
				TotalCount = cs.Cards.Count(),
				TotalPrice = cs.Cards.Sum(ic => ic.IsFoil ? ic.Card.PriceFoil : ic.Card.Price) ?? 0,
			});
		}

		public IQueryable<DeckData> ExampleDeckQuery() => Decks.Where(d => d.Format.Name == "commander");

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





			//Lets try this again...

			//The goal is to get information about card definitions, grouped by name, and inventory information about those cards
			//Note that it should end up including unowned cards, not just owned ones

			//One approach: For eac inv card, group by name, then select info to a CardOverviewResult
			//	This won't catch unowned cards

			//var query = Cards.Select(c => new
			//         {
			//	Card = c,
			//         })


			//need each card
			//need all inventory cards

			/*
			 How does the view work again?...
				Selects every card (all 31k), and left joins on vwCardTotals
					vwCardTotals 
					
				Selects the most recent card (All cards joined by name, ranked by Set.ReleaseDate>CollectorNumber )
					Get the most recent print of every named card

			 vwCardTotals...
				Oh fuck it uses a pivot table
				And pivots on the result of a case statement
				(was this a dumb idea?...)
				
				So, this view gets the 
			 */


			//Start with all cards
			//Get the inventory counts for each card, the rest can be manipulated later
			var totalsByCard = Cards.Select(c => new
			{
				//No, what do I REALLY need?

				c.CardId,
				c.Name,
				//DeckCount
				DeckCount = c.InventoryCards.Where(ic => ic.DeckCards.Count != 0).Count(),

				//InventoryCount
				InventoryCount = c.InventoryCards
					.Where(ic => ic.InventoryCardStatusId == 1 && ic.DeckCards.Count == 0)
					.Count(),

				BuyCount = c.InventoryCards
					.Where(ic => ic.InventoryCardStatusId == 2 && ic.DeckCards.Count == 0)
					.Count(),

				//SellCount
				SellCount = c.InventoryCards
					.Where(ic => ic.InventoryCardStatusId == 3 && ic.DeckCards.Count == 0)
					.Count(),

				//TotalCount
				TotalCount = c.InventoryCards.Count(),


				//Card = c,
				//InventoryCards = c.InventoryCards,//What do I actually need from invCards?
			});

			var test = totalsByCard.ToList();

			var totalsByName = totalsByCard
				.GroupBy(c => c.Name, c => c).ToList()
				.Select(g => new
				{
					Name = g.Key,
					DeckCount = g.Sum(c => c.DeckCount),
					InventoryCount = g.Sum(c => c.InventoryCount),
					BuyCount = g.Sum(c => c.BuyCount),
					SellCount = g.Sum(c => c.SellCount),
					TotalCount = g.Sum(c => c.TotalCount),
				}).ToList();

			//var hmm = test.Where(c => c.DeckCount != 0).ToList();

			var resTestBp = 1;



			var qyery = Cards
				.GroupBy(c => c.Name, c => c)
				//So the issue is with this...
				.Select(g => new
				{
					Name = g.Key,
                    NewestPrint = g.OrderByDescending(c => c.Set.ReleaseDate).First(),
                    //InventoryCards = g.SelectMany(c => c.InventoryCards),
                })//.Include(c => c.NewestPrint.Set)
				.ToList();
			
			var p1 = 1;

			//var queryResult = qyery
			//	.Select(c => new CardOverviewResult()
			//	{
			//		//public int Id { get; set; }

			//		////card definition properties
			//		//public int CardId { get; set; }
			//		CardId = c.NewestPrint.CardId,
			//		//public string SetCode { get; set; }
			//		SetCode = c.NewestPrint.Set.Code,
			//		Name = c.Name,
			//		Type = c.NewestPrint.Type,
			//		Text = c.NewestPrint.Text,
			//		ManaCost = c.NewestPrint.ManaCost,
			//		Cmc = c.NewestPrint.Cmc,
			//		RarityId = c.NewestPrint.RarityId,
			//		ImageUrl = c.NewestPrint.ImageUrl,
			//		CollectorNumber = c.NewestPrint.CollectorNumber,
			//		Color = c.NewestPrint.Color,
			//		ColorIdentity = c.NewestPrint.ColorIdentity,
			//		////prices
			//		//public decimal? Price { get; set; }
			//		//public decimal? PriceFoil { get; set; }
			//		//public decimal? TixPrice { get; set; }
			//		////counts
			//		//public int TotalCount { get; set; }
			//		//public int DeckCount { get; set; }
			//		//public int InventoryCount { get; set; }
			//		//public int SellCount { get; set; }
			//		////(I can add more when I actually need them)        
			//		////wishlist count

			//		//public bool? IsFoil { get; set; } //only populated for ByUnique, otherwise NULL





			//	}).ToList();


			int breakPoint = 1;


			//InventoryCards.GroupBy(c => c.Card.Name)








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

		public async Task EnsureDatabaseCreated(bool includeViews = true)
		{
			//Returns true if the DB was just created, false if the DB already existed
			var wasFreshlyCreated = await Database.EnsureCreatedAsync();
			//Only need to seed reference values if the DB was just created
			if (!wasFreshlyCreated) return;

			if (includeViews)
			{
				await ExecuteSqlScript("vwAllInventoryCards");
				await ExecuteSqlScript("vwCardTotals");
				await ExecuteSqlScript("vwInventoryCardsByName");
				await ExecuteSqlScript("vwInventoryCardsByPrint");
				await ExecuteSqlScript("vwInventoryCardsByUnique");
				await ExecuteSqlScript("vwInventoryTotalsByStatus");
				await ExecuteSqlScript("vwSetTotals");
				await ExecuteSqlScript("spGetInventoryTotals");
			}
			
			//Create default records
			await EnsureDefaultRecordsExist();
		}

		private async Task EnsureDefaultRecordsExist()
		{
			_logger.LogWarning("Adding default records");

			await EnsureDbCardStatusesExist();
			await EnsureDbRaritiesExist();
			await EnsureDbMagicFormatsExist();
			await EnsureDbDeckCardCategoriesExist();

			_logger.LogInformation("Finished adding default records");
		}

		private async Task EnsureDbCardStatusesExist()
		{
			/*
			 Statuses:
			 1 - Inventory/Owned
			 2 - Buylist
			 3 - SellList
			 */

			List<InventoryCardStatusData> allStatuses = new List<InventoryCardStatusData>()
			{
				new InventoryCardStatusData { CardStatusId = 1, Name = "Inventory" },
				new InventoryCardStatusData { CardStatusId = 2, Name = "Buy List" },
				new InventoryCardStatusData { CardStatusId = 3, Name = "Sell List" },
			};

			for (int i = 0; i < allStatuses.Count(); i++)
			{
				var status = allStatuses[i];

				var existingRecord = CardStatuses.FirstOrDefault(x => x.Name == status.Name);
				if (existingRecord == null)
				{
					_logger.LogWarning($"Adding card status {status.Name}");
					CardStatuses.Add(status);
				}
			}
			await SaveChangesAsync();

			//var statusTasks = allStatuses.Select(s => _coreDataRepo.TryAddInventoryCardStatus(s));

			//await Task.WhenAll(statusTasks);

			_logger.LogInformation("Finished adding card statuses");
		}

		private async Task EnsureDbRaritiesExist()
		{
			List<CardRarityData> allRarities = new List<CardRarityData>()
			{
				new CardRarityData
				{
					RarityId = 'M',
					Name = "mythic",
				},
				new CardRarityData
				{
					RarityId = 'R',
					Name = "rare",
				},
				new CardRarityData
				{
					RarityId = 'U',
					Name = "uncommon",
				},
				new CardRarityData
				{
					RarityId = 'C',
					Name = "common",
				},
				new CardRarityData
				{
					RarityId = 'S',
					Name = "special",
				},
			};

			for (int i = 0; i < allRarities.Count(); i++)
			{
				var rarity = allRarities[i];

				var existingRecord = Rarities.FirstOrDefault(x => x.RarityId == rarity.RarityId);
				if (existingRecord == null)
				{
					_logger.LogWarning($"Adding card rarity {rarity.Name}");
					Rarities.Add(rarity);
				}
			}
			await SaveChangesAsync();

			//var tasks = allRarities.Select(r => _coreDataRepo.TryAddCardRarity(r));

			//await Task.WhenAll(tasks);

			_logger.LogInformation("Finished adding card rarities");
		}

		private async Task EnsureDbMagicFormatsExist()
		{
			//Should I just comment out formats I don't care about?
			List<MagicFormatData> allFormats = new List<MagicFormatData>()
			{
				new MagicFormatData { Name = "standard" },
				//new MagicFormat { Name = "future" },
				//new MagicFormat { Name = "historic" },
				new MagicFormatData { Name = "pioneer" },
				new MagicFormatData { Name = "modern" },
				//new MagicFormat { Name = "legacy" },
				new MagicFormatData { Name = "pauper" },
				//new MagicFormat { Name = "vintage" },
				//new MagicFormat { Name = "penny" },
				new MagicFormatData { Name = "commander" },
				new MagicFormatData { Name = "brawl" },
				new MagicFormatData { Name = "jumpstart" },
				new MagicFormatData { Name = "sealed" },
				//new MagicFormat { Name = "duel" },
				//new MagicFormat { Name = "oldschool" },
			};

			for (int i = 0; i < allFormats.Count(); i++)
			{
				var format = allFormats[i];

				var existingRecord = MagicFormats.FirstOrDefault(x => x.Name == format.Name);
				if (existingRecord == null)
				{
					MagicFormats.Add(format);
					_logger.LogWarning($"Adding format {format.Name}");
				}
			}
			await SaveChangesAsync();

			//var tasks = allFormats.Select(x => _coreDataRepo.TryAddMagicFormat(x));

			//await Task.WhenAll(tasks);

			_logger.LogInformation("Finished adding formats");
		}

		private async Task EnsureDbDeckCardCategoriesExist()
		{
			List<DeckCardCategoryData> allCategories = new List<DeckCardCategoryData>()
			{
				//null == mainboard new DeckCardCategory { Id = '', Name = "" },
				new DeckCardCategoryData { DeckCardCategoryId = 'c', Name = "Commander" },
				new DeckCardCategoryData { DeckCardCategoryId = 's', Name = "Sideboard" },
				//Companion

				//new DeckCardCategory { Id = '', Name = "" },
				//new DeckCardCategory { Id = '', Name = "" },
			};

			//Try Add DeckCardCategories
			for (int i = 0; i < allCategories.Count(); i++)
			{
				var category = allCategories[i];

				var existingRecord = DeckCardCategories.FirstOrDefault(x => x.DeckCardCategoryId == category.DeckCardCategoryId);
				if (existingRecord == null)
				{
					DeckCardCategories.Add(category);
					_logger.LogWarning($"Adding category {category.Name}");
				}

			}
			await SaveChangesAsync();

			//var tasks = allCategories.Select(x => _coreDataRepo.TryAddDeckCardCategory(x));

			//await Task.WhenAll(tasks);

			_logger.LogInformation("Finished adding card categories");
		}

		private async Task ExecuteSqlScript(string scriptName)
		{
			try
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
			catch (Exception ex)
			{
				_logger.LogInformation($"Error attempting to add DB object {scriptName}", ex);
				throw ex;
			}
		}





	}
}
