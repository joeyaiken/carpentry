using Newtonsoft.Json;
using System.Collections.Generic;

namespace Carpentry.Legacy.Models
{
    public class LegacyInventoryDetailDto
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("cards")]
        public List<LegacyMagicCardDto> Cards { get; set; }

        [JsonProperty("inventoryCards")]
        public List<LegacyInventoryCardDto> InventoryCards { get; set; }

        //Deck Cards
        //for now, just storing IDs under an inventory card
    }
}
