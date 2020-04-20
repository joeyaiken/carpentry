using Carpentry.Logic.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Carpentry.UI.Legacy.Models
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
