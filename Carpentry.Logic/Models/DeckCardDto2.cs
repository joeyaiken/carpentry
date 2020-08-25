using Newtonsoft.Json;

namespace Carpentry.Logic.Models
{
    //TODO - Figure out if this is used, I'd like to have a version that doesn't include an inventory card
    public class DeckCardDto2
    {
        [JsonProperty("categoryId")]
        public char? CategoryId { get; set; }

        //[JsonProperty("deckId")]
        //public int DeckId { get; set; }

        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("inventoryCardId")]
        public int InventoryCardId { get; set; }

        [JsonProperty("cardId")]
        public int CardId { get; set; }

        [JsonProperty("isFoil")]
        public bool IsFoil { get; set; }

        [JsonProperty("inventoryCardStatusId")]
        public int InventoryCardStatusId { get; set; }
    }
}
