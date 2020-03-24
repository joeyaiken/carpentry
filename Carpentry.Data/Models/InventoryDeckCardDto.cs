using Newtonsoft.Json;

namespace Carpentry.Data.Models
{
    //This will be used by the "Get Inventory Detail By Name" and "Update Deck Card" opperations
    public class InventoryDeckCardDto
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("deckId")]
        public int DeckId { get; set; }
        
        [JsonProperty("deckName")]
        public string DeckName { get; set; }
        
        [JsonProperty("inventoryCardId")]
        public int InventoryCardId { get; set; }
    }
}
