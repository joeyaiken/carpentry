using Newtonsoft.Json;

namespace Carpentry.Logic.Models
{
    //TODO - Figure out if this is used, I'd like to have a version that doesn't include an inventory card
    public class DeckCard
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("deckId")]
        public int DeckId { get; set; }

        [JsonProperty("categoryId")]
        public char? CategoryId { get; set; }

        [JsonProperty("inventoryCard")]
        public InventoryCard InventoryCard { get; set; }
    }
}
