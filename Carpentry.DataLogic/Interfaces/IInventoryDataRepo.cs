using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Carpentry.CarpentryData.Models;
using Carpentry.CarpentryData.Models.QueryResults;
using Carpentry.DataLogic.QueryResults;

namespace Carpentry.DataLogic.Interfaces
{
    [Obsolete]
    public interface IInventoryDataRepo
    {
        [Obsolete]
        Task<int> AddInventoryCard(InventoryCardData card);
        [Obsolete]
        Task AddInventoryCardBatch(List<InventoryCardData> cardBatch);
        [Obsolete]
        Task UpdateInventoryCard(InventoryCardData card);
        [Obsolete]
        Task DeleteInventoryCard(int inventoryCardId);
        [Obsolete]
        Task<InventoryCardData> GetInventoryCard(int inventoryCardId);
        [Obsolete]
        Task<IEnumerable<InventoryCardResult>> GetInventoryCardsByName(string cardName); //Used for "Get Inventory Detail"

        //CardOverviewResult | InventoryOverviewResult
        [Obsolete]
        IQueryable<CardOverviewResult> QueryCardsByName();
        [Obsolete]
        IQueryable<CardOverviewResult> QueryCardsByPrint();
        [Obsolete]
        IQueryable<CardOverviewResult> QueryCardsByUnique();

        //Task<List<TrimmingTipsResult>> GetTrimmingTips(int usedCardsToKeep = 10, int unusedCardsToKeeep = 6, string setCode = null);
        //Task<int> GetTotalTrimCount(int usedCardsToKeep = 10, int unusedCardsToKeeep = 6, string setCode = null);
        [Obsolete]
        Task<Dictionary<string, List<InventoryCardData>>> GetUnusedInventoryCards(IEnumerable<string> cardNames);

    }
}
