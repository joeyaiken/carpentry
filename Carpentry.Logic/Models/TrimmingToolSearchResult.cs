namespace Carpentry.Logic.Models
{
    public class TrimmingToolSearchResult
    {
        public string Id { get; set; }
        public int CardId { get; set; }
        public string Name { get; set; }
        public bool? IsFoil { get; set; }
        public string PrintDisplay { get; set; }
        public decimal? Price { get; set; }
        
        
        //inventory
        public int UnusedCount { get; set; }
        //inventory & decks
        public int TotalCount { get; set; }
        //inventory & decks for all prints (does this include sell list?)
        public int AllPrintsCount { get; set; }
        
        //Recommended
        public int RecommendedTrimCount { get; set; }
        
        public string ImageUrl { get; set; }
        // public int? CollectorNumber { get; set; }
        
        // public string Type { get; set; }
        // public string ColorIdentity { get; set; }

        
        
        //prices
        // public decimal? PriceFoil { get; set; }
        // public decimal? TixPrice { get; set; }

        //counts
        // public int PrintTotalCount { get; set; }
        // public int PrintDeckCount { get; set; }
        // public int PrintInventoryCount { get; set; }
        // public int PrintSellCount { get; set; }
        //
        // public int AllTotalCount { get; set; }
        // public int AllDeckCount { get; set; }
        // public int AllInventoryCount { get; set; }
        // public int AllSellCount { get; set; }
        
        // Only used by the UI, but I want it to still be defaulted to 0 when returned
        //  (specifically only used by angular, would be really nice to refactor this out)
        public int PendingTrimCount { get; set; }
    }
}