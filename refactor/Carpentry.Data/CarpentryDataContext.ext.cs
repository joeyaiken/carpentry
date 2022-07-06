using Carpentry.Data.QueryResults;

namespace Carpentry.Data;

public partial class CarpentryDataContext
{
    public IQueryable<SetTotalsResult> GetSetTotals()
    {
        return Sets.Select(set => new SetTotalsResult
        {
            SetId = set.SetId,
            Code = set.Code,
            Name = set.Name,
            ReleaseDate = set.ReleaseDate,
            // LastUpdated = set.LastUpdated,
            IsTracked = set.IsTracked,
            // InventoryCount = set.Cards.SelectMany(c => c.InventoryCards).Count(),
            // CollectedCount = set.Cards.Count(c => c.InventoryCards.Any()),
            // TotalCount = set.Cards.Count,
        });
    }
}