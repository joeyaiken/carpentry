using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Carpentry.Data.Models;
using Carpentry.Data.QueryParameters;
using Carpentry.Data.QueryResults;

namespace Carpentry.Data.Interfaces
{
    public interface IDataQueryService
    {
        //CardOverviewResult
        //
        //GetInventoryOverviews

        Task<IEnumerable<CardOverviewResult>> GetInventoryOverviews(InventoryQueryParameter param);

        Task<IEnumerable<CardOverviewResult>> GetDeckCardOverviews(int deckId);

        //get deck inventory cards

        //get the total # of cards in a deck (includes basic lands)
        Task<int> GetDeckCardCount(int deckId);

        Task<IEnumerable<InventoryCardResult>> GetDeckInventoryCards(int deckId);

        Task<IEnumerable<DeckCardStatResult>> GetDeckCardStats(int deckId);

        Task<IEnumerable<InventoryCardResult>> GetInventoryCardsByName(string cardName);

        //Card Search queries
        Task<IEnumerable<CardDataDto>> SearchInventoryCards(InventoryQueryParameter filters);

        Task<IEnumerable<CardDataDto>> SearchCardSet(CardSearchQueryParameter filters);
    }
}
