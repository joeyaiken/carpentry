using Newtonsoft.Json;
using System.Collections.Generic;

namespace Carpentry.Logic.Models
{
    public class InventoryDetailDto
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("cards")]
        public List<MagicCardDto> Cards { get; set; }

        [JsonProperty("inventoryCards")]
        public List<InventoryCardDto> InventoryCards { get; set; }

        //Deck Cards
        //for now, just storing IDs under an inventory card
    }
}
