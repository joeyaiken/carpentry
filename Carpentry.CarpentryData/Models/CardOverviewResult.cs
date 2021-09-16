using System;
using System.Collections.Generic;

namespace Carpentry.CarpentryData.Models
{
    //public class InventoryCardOverview
    //{

    //}

    public class CardOverviewResult
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
        public int TotalCount { get; set; }
        public int DeckCount { get; set; }
        public int InventoryCount { get; set; }
        public int SellCount { get; set; }
        //(I can add more when I actually need them)        
        //wishlist count
        
        //Misc
        public bool? IsFoil { get; set; } //only populated for ByUnique, otherwise NULL
        public DateTime SetReleaseDate { get; set; }



        //public List<InventoryCardOverview> InventoryCards { get; set; }

    }
}
