using System;

namespace Carpentry.Data.DataModels.QueryResults
{
    //result of CarpentryData.dbo.vwSetTotals
    public class SetTotalsResult
    {
        public int SetId { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }

        public DateTime ReleaseDate { get; set; }

        public DateTime? LastUpdated { get; set; }

        public bool IsTracked { get; set; }

        public int? InventoryCount { get; set; }
        
        public int? CollectedCount { get; set; }

        public int? TotalCount { get; set; }
    }
}
