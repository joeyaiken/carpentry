using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Carpentry.Data.DataModels
{
    public class CardData
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        public int? Cmc { get; set; }

        //public string ImageUrl { get; set; }

        //public string ImageArtCropUrl { get; set; }

        public string ManaCost { get; set; }

        public string Name { get; set; }

        //public decimal? Price { get; set; }

        //public decimal? PriceFoil { get; set; }

        public char RarityId { get; set; }

        public int SetId { get; set; }

        public string Text { get; set; }

        public string Type { get; set; }

        //Associations

        //Card -- Set
        public virtual CardSetData Set { get; set; }

        //Card -- Rarity
        public virtual CardRarityData Rarity { get; set; }

        //Card -- CardColorIdentity
        public virtual ICollection<CardColorIdentityData> CardColorIdentities { get; set; }

        public virtual ICollection<CardColorData> CardColors { get; set; }

        //InventoryCard -- Card
        public virtual ICollection<InventoryCardData> InventoryCards { get; set; }

        //variant
        public virtual ICollection<CardVariantData> Variants { get; set; }

        //legal sets
        public virtual ICollection<CardLegalityData> Legalities { get; set; }
    }
}
