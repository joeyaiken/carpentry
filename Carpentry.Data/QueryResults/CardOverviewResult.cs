using System;
using System.Collections.Generic;
using System.Text;

namespace Carpentry.Data.QueryResults
{
    public class CardOverviewResult
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Type { get; set; }

        public string Cost { get; set; }

        public int? Cmc { get; set; }

        public string Img { get; set; }

        public int Count { get; set; }

        //category / status / group
        //public string Description { get; set; }


        public string Category { get; set; }


        public decimal? Price { get; set; }

        public bool? IsFoil { get; set; }

        public string Variant { get; set; }

    }
}
