using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Carpentry.CarpentryData.Models;
using Carpentry.CarpentryData.Models.QueryResults;
using Carpentry.DataLogic.QueryResults;

namespace Carpentry.DataLogic.Interfaces
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

        //Task<List<TrimmingTipsResult>> GetTrimmingTips(int usedCardsToKeep = 10, int unusedCardsToKeeep = 6, string setCode = null);
        //Task<int> GetTotalTrimCount(int usedCardsToKeep = 10, int unusedCardsToKeeep = 6, string setCode = null);
        Task<Dictionary<string, List<InventoryCardData>>> GetUnusedInventoryCards(IEnumerable<string> cardNames);

    }
}
