using System;
using System.Collections.Generic;
using System.Text;

namespace Carpentry.Data.DataModels.QueryResults
{
    public class InventoryCardByUniqueResult
    {
        public int CardId { get; set; }

        public string SetCode { get; set; }

        public string Name { get; set; }

        public string Type { get; set; }

        public string ManaCost { get; set; }

        public int? Cmc { get; set; }

        public char RarityId { get; set; }

        public int? CollectorNumber { get; set; }

        public bool? IsFoil { get; set; }

        public decimal? Price { get; set; }

        public int? CardCount { get; set; }

        public string ImageUrl { get; set; }
    }
}
