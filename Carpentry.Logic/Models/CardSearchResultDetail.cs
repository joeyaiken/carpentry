using System;
using System.Collections.Generic;
using System.Text;

namespace Carpentry.Logic.Models
{
    public class CardSearchResultDetail
    {
        public int CardId { get; set; }
        
        public string SetCode { get; set; }

        public string Name { get; set; }
        
        public int CollectionNumber { get; set; }

        public decimal? Price { get; set; }

        public decimal? PriceFoil { get; set; }

        public decimal? PriceTix { get; set; }

        public string ImageUrl { get; set; }
    }
}
