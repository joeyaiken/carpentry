using Carpentry.Data.DataContext;
using Carpentry.Data.DataModels;
using Carpentry.Data.DataModels.QueryResults;
using Carpentry.Data.Exceptions;
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
				CardId = card.CardId,
				Cmc = card.Cmc,
				ManaCost = card.ManaCost,
				MultiverseId = card.CardId,
				Name = card.Name,
				ImageUrl = card.ImageUrl,
				Price = card.Price,
				TixPrice = card.TixPrice,
				PriceFoil = card.PriceFoil,
				CollectorNumber = card.CollectorNumber,

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
		
		
		//NOTE - This is the most recent query for adding cards to a deck
		private async Task<IQueryable<CardData>> QueryFilteredCards(InventoryQueryParameter filters)
		{
			var cardsQuery = _cardContext.Cards.AsQueryable();

			

			if (!string.IsNullOrEmpty(filters.Set))
			{
				var matchingSetId = _cardContext.Sets.Where(x => x.Code.ToLower() == filters.Set.ToLower()).Select(x => x.SetId).FirstOrDefault();
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
			return format.FormatId;
		}

		#endregion


		public async Task<InventoryCardData> GetInventoryCard(int inventoryCardId)
		{
			var result = await _cardContext.InventoryCards.Where(x => x.InventoryCardId == inventoryCardId).FirstOrDefaultAsync();
			return result;
		}

		//public async Task<InventoryCardData> GetInventoryCard(string setCode, int collectorNumber)
		//{
		//    var result = await _cardContext.InventoryCards
		//        .Where(x => x.Card.Set.Code == setCode && x.Card.CollectorNumber == collectorNumber)
		//        .FirstOrDefaultAsync();
		//    return result;
		//}


		/// <summary>
		/// Adds a new card to the inventory
		/// Does not handle adding deck cards
		/// </summary>
		/// <param name="dto"></param>
		/// <returns></returns>
		public async Task<int> AddInventoryCard(InventoryCardData cardToAdd)
		{
			var matchingCard = _cardContext.Cards.FirstOrDefault(x => x.CardId == cardToAdd.CardId);
			var first6Card = _cardContext.Cards.FirstOrDefault();
			_cardContext.InventoryCards.Add(cardToAdd);
			await _cardContext.SaveChangesAsync();

			return cardToAdd.InventoryCardId;
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
			//todo - actually check if exists? I could just let it error
			var existingCard = await _cardContext.InventoryCards.FirstOrDefaultAsync(c => c.InventoryCardId == cardToUpdate.InventoryCardId);

			if(existingCard == null)
			{
				throw new CardNotFoundException("Could not find a matching inventory card to update");
			}

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

			var cardToRemove = _cardContext.InventoryCards.First(x => x.InventoryCardId == id);

			_cardContext.InventoryCards.Remove(cardToRemove);

			await _cardContext.SaveChangesAsync();
		}

		public IQueryable<InventoryCardByNameResult> QueryCardsByName()
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

            return _cardContext.InventoryCardByName.AsQueryable();
        }

		public IQueryable<InventoryCardByPrintResult> QueryCardsByPrint()
		{
			return _cardContext.InventoryCardByPrint.AsQueryable();
		}

		public IQueryable<InventoryCardByUniqueResult> QueryCardsByUnique()
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



            return _cardContext.InventoryCardByUnique.AsQueryable();
        }

		/// <summary>
		/// This loads cards for "Get Inventory Detail" 
		/// </summary>
		/// <param name="cardName"></param>
		/// <returns></returns>
		public async Task<IEnumerable<InventoryCardResult>> GetInventoryCardsByName(string cardName)
		{
			List<InventoryCardResult> inventoryCards = await _cardContext.Cards.Where(x => x.Name == cardName)
				.SelectMany(x => x.InventoryCards)
				.Select(x => new InventoryCardResult()
				{
					Id = x.InventoryCardId,
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
	}
}
