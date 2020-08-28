using System;
using System.Collections.Generic;
using System.Text;

namespace Carpentry.Data.DataModels.QueryResults
{
    public class InventoryCardByNameResult
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public string ManaCost { get; set; }
        public string Color { get; set; }
        public string ColorIdentity { get; set; }
        public int? Cmc { get; set; }
        public string ImageUrl { get; set; }
        public int OwnedCount { get; set; }
        public int DeckCount { get; set; }
    }
}
