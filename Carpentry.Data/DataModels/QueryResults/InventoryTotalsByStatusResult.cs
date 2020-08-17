using System;
using System.Collections.Generic;
using System.Text;

namespace Carpentry.Data.DataModels.QueryResults
{
    public class InventoryTotalsByStatusResult
    {
        public int StatusId { get; set; }
        public string StatusName { get; set; }
        public decimal TotalPrice { get; set; }
        public int? TotalCount { get; set; }
    }
}
