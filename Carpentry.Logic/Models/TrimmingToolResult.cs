namespace Carpentry.Logic.Models
{
    public class TrimmingToolResult
    {
        public int Id { get; set; }

        //card definition properties
        public int CardId { get; set; }
        public string SetCode { get; set; }
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public int? CollectorNumber { get; set; }
        public string CollectorNumberStr { get; set; }
        public string Type { get; set; }
        public string ColorIdentity { get; set; }

        public bool? IsFoil { get; set; } //only populated for ByUnique, otherwise NULL
        
        //prices
        public decimal? Price { get; set; }
        public decimal? PriceFoil { get; set; }
        public decimal? TixPrice { get; set; }

        //counts
        public int PrintTotalCount { get; set; }
        public int PrintDeckCount { get; set; }
        public int PrintInventoryCount { get; set; }
        public int PrintSellCount { get; set; }

        public int AllTotalCount { get; set; }
        public int AllDeckCount { get; set; }
        public int AllInventoryCount { get; set; }
        public int AllSellCount { get; set; }
    }
}
