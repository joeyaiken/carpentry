﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Carpentry.CarpentryData.Models
{
    public class CardData
    {
        [Key]
        public int CardId { get; set; }
        public int? Cmc { get; set; }
        public string ManaCost { get; set; }
        public string Name { get; set; }
        public char RarityId { get; set; }
        public int SetId { get; set; }
        public string Text { get; set; }
        public string Type { get; set; }
        public int? MultiverseId { get; set; }
        //These 3 prices should be decimals, but SQLite doesn't support decimals...
        public double? Price { get; set; }
        public double? PriceFoil { get; set; }
        public double? TixPrice { get; set; }
        public string ImageUrl { get; set; }
        public int CollectorNumber { get; set; }
        public char? CollectorNumberSuffix { get; set; }
        public string CollectorNumberStr { get; set; }
        public string Color { get; set; }
        public string ColorIdentity { get; set; }

        //Associations
        public virtual CardSetData Set { get; set; }
        public virtual CardRarityData Rarity { get; set; }
        public virtual ICollection<InventoryCardData> InventoryCards { get; set; }
        public virtual ICollection<CardLegalityData> Legalities { get; set; }
    }
}
