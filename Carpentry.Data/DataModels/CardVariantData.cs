using System;
using System.Collections.Generic;
using System.Text;

namespace Carpentry.Data.DataModels
{
    //This is still partially wrong
    //A Card has several variant types, those options are stored in the "Cadr Variants" table
    //An inventory card is an instance of a card of a specific variant type
    public class CardVariantData
    {
        public int Id { get; set; }
        public int CardId { get; set; }
        public char CardVariantTypeId { get; set; }

        public decimal? Price { get; set; }
        public decimal? PriceFoil { get; set; }

        public string ImageUrl { get; set; }

        //associations
        public CardData Card { get; set; }
        public CardVariantTypeData Type { get; set; }
    }

}
