using Newtonsoft.Json;

namespace Carpentry.Logic.Models
{
    // TODO : Rename to CardOverviewDto 
    //  but actually, I think I may want a specific
    //inventory overview | inventory detail | deck overview | deck detail | Card Search Overview/Search Result Overview | Search Detail
    public class InventoryOverviewDto 
    {
        public int Id { get; set; }

        //card definition properties
        public int CardId { get; set; }
        public string SetCode { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Text { get; set; }
        public string ManaCost { get; set; }
        public int? Cmc { get; set; }
        public char RarityId { get; set; }
        public string ImageUrl { get; set; }
        public int? CollectorNumber { get; set; }
        public string Color { get; set; }
        public string ColorIdentity { get; set; }
        //prices
        public decimal? Price { get; set; }
        public decimal? PriceFoil { get; set; }
        public decimal? TixPrice { get; set; }
        //counts
        public int OwnedCount { get; set; }
        public int DeckCount { get; set; }

        public bool? IsFoil { get; set; } //only populated for ByUnique, otherwise NULL
    }
}
