using Carpentry.CarpentryData;
using Carpentry.CarpentryData.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Carpentry.DataLogic
{
	[Obsolete]
	public class InventoryDataRepo
	{
		private readonly CarpentryDataContext _cardContext;
		public InventoryDataRepo(CarpentryDataContext cardContext)
		{
			_cardContext = cardContext;
		}

		//I want to submit a list of card names, and get a collection of inventory cards for each name
		//TODO - Consider renaming to 'GetInventoryCardsByName'
		[Obsolete]
		public async Task<Dictionary<string, List<InventoryCardData>>> GetUnusedInventoryCards(IEnumerable<string> cardNames)
		{
			//TODO - consider filtering out inventory cards that are already in a deck
			//  I don't want to do that, but DO want details on deck cards

			var result = (await _cardContext.InventoryCards
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
