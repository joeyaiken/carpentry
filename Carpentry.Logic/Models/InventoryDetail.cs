using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Carpentry.Logic.Models
{
    public class InventoryDetail
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("cards")]
        public List<MagicCard> Cards { get; set; }

        [JsonProperty("inventoryCards")]
        public List<InventoryCard> InventoryCards { get; set; }

        //Deck Cards
        //for now, just storing IDs under an inventory card
    }
}
