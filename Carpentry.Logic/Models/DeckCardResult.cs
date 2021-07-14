using System.Collections.Generic;

namespace Carpentry.Logic.Models
{
    //This is just a DTO used to get detailed data about the cards in a deck
    //When a card is empty, this should be populated with the most recent info
    //(ThatDevQuery-like implementation)
    public class DeckCardResult
    {
        public int DeckCardId { get; set; }
        public int DeckId { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public int? InventoryCardId { get; set; }


        public string Type { get; set; }
        public string Cost { get; set; }
        public int? Cmc { get; set; }
        public string Img { get; set; }



        public string SetCode { get; set; }
        //public int SetId { get; set; }

        public bool IsFoil { get; set; }

        //public string VariantType { get; set; }
        public int? CollectorNumber { get; set; }
        public int CardId { get; set; }
        public string ColorIdentity { get; set; }

        public decimal? Price { get; set; }

        public decimal? PriceFoil { get; set; }

        public List<string> Tags { get; set; }
    }
}
