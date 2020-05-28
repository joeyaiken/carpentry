using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
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
        public int CardVariantTypeId { get; set; }

        //[HasColumnType()]
        [Column(TypeName="Decimal(6,2)")] //lets hope I don't own an individual card worth more than $9,999.99
        public decimal? Price { get; set; }

        [Column(TypeName = "Decimal(6,2)")] //lets hope I don't own an individual card worth more than $9,999.99
        public decimal? PriceFoil { get; set; }

        public string ImageUrl { get; set; }

        //associations
        public virtual CardData Card { get; set; }
        public virtual CardVariantTypeData Type { get; set; }
    }

}
