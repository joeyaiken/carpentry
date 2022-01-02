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
        public int UnusedCount { get; set; }
        public int TotalCount { get; set; }
        public int AllPrintsCount { get; set; }
        public int RecommendedTrimCount { get; set; }
        public string ImageUrl { get; set; }
        // public int PendingTrimCount { get; set; }
    }
}