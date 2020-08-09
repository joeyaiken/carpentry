using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Carpentry.Data.DataModels.QueryResults;
using Carpentry.Data.Models;
using Carpentry.Data.QueryParameters;
using Carpentry.Data.QueryResults;

namespace Carpentry.Data.Interfaces
{
    public interface IDataQueryService
    {
        //Task<IEnumerable<CardOverviewResult>> GetInventoryOverviews(InventoryQueryParameter param);





        ////get deck inventory cards

        ////get the total # of cards in a deck (includes basic lands)


        //Task<IEnumerable<InventoryCardResult>> GetInventoryCardsByName(string cardName);

        ////Card Search queries
        //Task<IEnumerable<CardDataDto>> SearchInventoryCards(InventoryQueryParameter filters);

        //Task<IEnumerable<CardDataDto>> SearchCardSet(CardSearchQueryParameter filters);


        ////IDK why the others aren't on there yet
        //IQueryable<SetTotalsResult> QuerySetTotals();

    }
}
