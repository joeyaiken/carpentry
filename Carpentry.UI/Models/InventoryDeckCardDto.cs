using Carpentry.Logic.Models;
using Newtonsoft.Json;

namespace Carpentry.UI.Models
{
    //This will be used by the "Get Inventory Detail By Name" and "Update Deck Card" opperations
    public class InventoryDeckCardDto
    {
        public InventoryDeckCardDto(InventoryDeckCard model)
        {
            DeckCardCategory = model.DeckCardCategory;
            DeckId = model.DeckId;
            DeckName = model.DeckName;
            Id = model.Id;
            InventoryCardId = model.InventoryCardId;
        }

        public InventoryDeckCard ToModel()
        {
            InventoryDeckCard result = new InventoryDeckCard
            {
                DeckCardCategory = DeckCardCategory,
                DeckId = DeckId,
                DeckName = DeckName,
                Id = Id,
                InventoryCardId = InventoryCardId,
            };
            return result;
        }

        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("deckId")]
        public int DeckId { get; set; }

        [JsonProperty("deckName")]
        public string DeckName { get; set; }

        [JsonProperty("inventoryCardId")]
        public int InventoryCardId { get; set; }

        [JsonProperty("category")]
        public string DeckCardCategory { get; set; }
    }
}
