using Newtonsoft.Json;
using System.Collections.Generic;

namespace Carpentry.Logic.Models
{
    public class InventoryCardDto
    {
        public InventoryCardDto()
        {
            DeckCards = new List<InventoryDeckCardDto>();
        }

        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("multiverseId")]
        public int MultiverseId { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("set")]
        public string Set { get; set; }

        [JsonProperty("isFoil")]
        public bool IsFoil { get; set; }

        [JsonProperty("variantName")]
        public string VariantType { get; set; }

        [JsonProperty("statusId")]
        public int InventoryCardStatusId { get; set; }

        //Should this be "deck cards" instead of "deck card IDs"
        //[JsonProperty("deckCardIds")]
        //public List<int> DeckCardIds { get; set; }

        [JsonProperty("deckCards")]
        public List<InventoryDeckCardDto> DeckCards { get; set; }
    }
}
