using System;
using System.Collections.Generic;
using System.Text;

namespace Carpentry.Logic.Models
{
    public class CardSearchResultDto
    {
        public int CardId { get; set; }

        public int? Cmc { get; set; }

        //public List<char> ColorIdentity { get; set; }
        public char[] ColorIdentity { get; set; }


        //public List<char> Colors { get; set; }
        public char[] Colors { get; set; }

        public string ManaCost { get; set; }

        //public int? MultiverseId { get; set; }

        public string Name { get; set; }

        //[JsonProperty("prices")]
        //public Dictionary<string, decimal?> Prices { get; set; }

        //[JsonProperty("variants")]
        //public Dictionary<string, string> Variants { get; set; }

        //public List<string> Legalities { get; set; }

        //public string Rarity { get; set; }

        //public string Set { get; set; }

        //public string Text { get; set; }

        public string Type { get; set; }

        public List<CardSearchResultDetail> Details { get; set; }

        //public int CollectionNumber { get; set; }

        //public decimal? Price { get; set; }

        //public decimal? PriceFoil { get; set; }

        //public decimal? PriceTix { get; set; }

        //public string ImageUrl { get; set; }
    }
}
