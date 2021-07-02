
namespace Carpentry.CarpentryData.Models.QueryResults
{
    public class InventoryTotalsByStatusResult
    {
        public int StatusId { get; set; }
        public string StatusName { get; set; }
        public decimal TotalPrice { get; set; }
        public int? TotalCount { get; set; }
    }
}
