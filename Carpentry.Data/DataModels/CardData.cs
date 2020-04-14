using System;
using System.Collections.Generic;
using System.Text;

namespace Carpentry.Data.DataModels
{
    public class CardData
    {
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
        public CardSetData Set { get; set; }

        //Card -- Rarity
        public CardRarityData Rarity { get; set; }

        //Card -- CardColorIdentity
        public List<CardColorIdentityData> CardColorIdentities { get; set; }

        public List<CardColorData> CardColors { get; set; }

        //InventoryCard -- Card
        public List<InventoryCardData> InventoryCards { get; set; }

        //variant
        public List<CardVariantData> Variants { get; set; }

        //legal sets
        public List<CardLegalityData> Legalities { get; set; }
    }
}
