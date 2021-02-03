using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Carpentry.Data.DataModels;
using Carpentry.Data.DataModels.QueryResults;
using Carpentry.Data.Models;
using Carpentry.Data.QueryParameters;
using Carpentry.Data.QueryResults;

namespace Carpentry.Data.Interfaces
{
    public interface IInventoryDataRepo
    {
        Task<int> AddInventoryCard(InventoryCardData card);
        Task AddInventoryCardBatch(List<InventoryCardData> cardBatch);
        Task UpdateInventoryCard(InventoryCardData card);
        Task DeleteInventoryCard(int inventoryCardId);
        Task<InventoryCardData> GetInventoryCard(int inventoryCardId);
        Task<IEnumerable<InventoryCardResult>> GetInventoryCardsByName(string cardName); //Used for "Get Inventory Detail"

        //CardOverviewResult | InventoryOverviewResult
        IQueryable<CardOverviewResult> QueryCardsByName();
        IQueryable<CardOverviewResult> QueryCardsByPrint();
        IQueryable<CardOverviewResult> QueryCardsByUnique();

        Task<List<TrimmingTipsResult>> GetTrimmingTips(int usedCardsToKeep = 10, int unusedCardsToKeeep = 6, string setCode = null);
        Task<int> GetTotalTrimCount(int usedCardsToKeep = 10, int unusedCardsToKeeep = 6, string setCode = null);
        Task<Dictionary<string, List<InventoryCardData>>> GetUnusedInventoryCards(IEnumerable<string> cardNames);

        Task<List<TrimmingToolResult>> TrimmingToolQuery(string setCode, int minCount, string searchGroup = null);
    }
}
