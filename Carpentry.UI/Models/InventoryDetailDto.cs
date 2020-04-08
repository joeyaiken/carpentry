using Carpentry.Logic.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Carpentry.UI.Models
{
    public class InventoryDetailDto
    {
        public InventoryDetailDto(InventoryDetail model)
        {
            Cards = Cards;
            InventoryCards = InventoryCards;
            Name = Name;
        }

        public InventoryDetail ToModel()
        {
            InventoryDetail inventoryDetail = new InventoryDetail
            {
                Cards = Cards.Select(x => x.ToModel()).ToList(),
                InventoryCards = InventoryCards.Select(x => x.ToModel()).ToList(),
                Name = Name,
            };
            return inventoryDetail;
        }

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
