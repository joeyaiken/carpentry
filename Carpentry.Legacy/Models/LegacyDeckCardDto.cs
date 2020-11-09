using Newtonsoft.Json;

namespace Carpentry.Legacy.Models
{
    //TODO - Figure out if this is used, I'd like to have a version that doesn't include an inventory card
    public class LegacyDeckCardDto
    {

        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("deckId")]
        public int DeckId { get; set; }

        [JsonProperty("categoryId")]
        public char? CategoryId { get; set; }

        [JsonProperty("inventoryCard")]
        public LegacyInventoryCardDto InventoryCard { get; set; }
    }
}
