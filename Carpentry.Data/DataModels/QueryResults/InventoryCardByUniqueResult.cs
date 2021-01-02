﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Carpentry.Data.DataModels.QueryResults
{
    public class InventoryCardByUniqueResult
    {
        //public int CardId { get; set; }
        //public string SetCode { get; set; }
        //public string Name { get; set; }
        //public string Type { get; set; }
        //public string Text { get; set; }
        //public string ManaCost { get; set; }
        //public int? Cmc { get; set; }
        //public char RarityId { get; set; }
        //public int CollectorNumber { get; set; }
        //public string Color { get; set; }
        //public string ColorIdentity { get; set; }
        //public string ImageUrl { get; set; }


        public bool? IsFoil { get; set; }
        public decimal? Price { get; set; }
        //priceFoil
        //tixPrice

        public int? OwnedCount { get; set; }
        public int? DeckCount { get; set; }
    }
}
