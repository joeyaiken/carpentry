namespace Carpentry.Data.DataModels.QueryResults
{
    public class InventoryCardByNameResult
    {
        public int CardId { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Text { get; set; }
        public string ManaCost { get; set; }
        public int? Cmc { get; set; }
        public string ImageUrl { get; set; }
        public string Color { get; set; }
        public string ColorIdentity { get; set; }
        public char RarityId { get; set; }
        public int CollectorNumber { get; set; }
        //public int SetId { get; set; }
        public string SetCode { get; set; }
        public int? MultiverseId { get; set; }
        public decimal? Price { get; set; }
        public decimal? PriceFoil { get; set; }
        public decimal? TixPrice { get; set; }
        public int OwnedCount { get; set; }
        public int DeckCount { get; set; }
    }
}
