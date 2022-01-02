namespace Carpentry.Logic.Models
{
	// Need something to map the SQL query to
    public class TrimmingToolQueryResult
    {
        public int Id { get; set; }
        public int CardId { get; set; }
        public string SetCode { get; set; }
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public string CollectorNumberStr { get; set; }
        public bool? IsFoil { get; set; }
        public decimal? Price { get; set; }
        public int PrintDeckCount { get; set; }
        public int PrintInventoryCount { get; set; }
        public int AllDeckCount { get; set; }
        public int AllInventoryCount { get; set; }
    }
}
