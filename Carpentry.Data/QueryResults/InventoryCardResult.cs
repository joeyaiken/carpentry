using System;
using System.Collections.Generic;
using System.Text;

namespace Carpentry.Data.QueryResults
{
    public class InventoryCardResult
    {
        public int Id { get; set; }

        public int MultiverseId { get; set; }

        public string Name { get; set; }

        public string Set { get; set; }

        public bool IsFoil { get; set; }

        public string VariantType { get; set; }

        public int InventoryCardStatusId { get; set; }
        
        public string Type { get; set; }

        //Should this be "deck cards" instead of "deck card IDs"
        //[JsonProperty("deckCardIds")]
        //public List<int> DeckCardIds { get; set; }

        //public List<DeckCardResult> DeckCards { get; set; }
    }
}
