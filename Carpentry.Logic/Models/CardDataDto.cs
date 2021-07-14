using System.Collections.Generic;

namespace Carpentry.Logic.Models
{
    //TODO - Determine if this should be kept or trashed
    public class CardDataDto
    {
        //Honestly this will be similar to a MagicCard / MagicCardDto
        //I just want something decent to pass to-from the data layer that doesn't contain IDs

        //I honestly think I just want to tweak the magicCardDto

        public int CardId { get; set; }

        //a flat representation of a DB card definition

        //Will also need a list of something to track variants

        public int? Cmc { get; set; }
        public List<string> ColorIdentity { get; set; }
        public List<string> Colors { get; set; }
        public string ManaCost { get; set; }
        public int? MultiverseId { get; set; }
        public string Name { get; set; }
        //public Dictionary<string, decimal?> Prices { get; set; }
        //public Dictionary<string, string> Variants { get; set; }
        public List<string> Legalities { get; set; }
        public string Rarity { get; set; }
        
        
        //public int SetId { get; set; }
        public string Set { get; set; }


        public string Text { get; set; }
        public string Type { get; set; }


        public int CollectorNumber { get; set; }
        public decimal? Price { get; set; }
        public decimal? PriceFoil { get; set; }
        public decimal? TixPrice { get; set; }
        public string ImageUrl { get; set; }

        //public List<CardVariantDto> Variants { get; set; }

        //thought: Do I keep normal image(and maybe price) here, and variants in additional models?
    }


    public class CardVariantDto
    {
        public string Name { get; set; }

        public decimal? Price { get; set; }

        public decimal? PriceFoil { get; set; }

        public string Image { get; set; }
        //price
        //price foil
        //variant name?

    }
}
