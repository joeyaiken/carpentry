using System;
using System.Collections.Generic;
using System.Text;

namespace Carpentry.Data.DataModels.QueryResults
{
    public class TrimmingTipsResult
    {
        public int CardId { get; set; }
        public bool IsFoil { get; set; }
        public string SetCode { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public decimal Price { get; set; }
        public int OwnedCount { get; set; }
        public int DeckCount { get; set; }
        public int TotalOwnedCount { get; set; }
        public int TotalDeckCount { get; set; }
        public int RecomendTrimming { get; set; }
        public string Reason { get; set; }
    }
}
