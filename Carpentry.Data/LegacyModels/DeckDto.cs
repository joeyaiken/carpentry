using Newtonsoft.Json;
using System.Collections.Generic;

namespace Carpentry.Data.Models
{
    public class DeckDto //: CardCollectionDto
    {
        //IDK if I need this since I'll have the list from the main menu, but w/e
        [JsonProperty("props")]
        public DeckProperties Props { get; set; }



        //what if cards were CardOverviewDTOs ? I need something to list in Deck Editor but want minimal logic there


        //I still need to itterate over the Deck/Inventory cards for each overview
        //  Needs to be inventory cards so I can show the variant

        [JsonProperty("cardOverviews")]
        public List<InventoryOverviewDto> CardOverviews { get; set; }

        [JsonProperty("cardDetails")]
        public List<InventoryCardDto> CardDetails { get; set; }

        [JsonProperty("stats")]
        public DeckStats Stats { get; set; }

        //[JsonProperty("cards")]
        //public List<Card> Cards { get; set; }

        //[JsonProperty("data")]
        //public Dictionary<int, MagicCardDto> Data { get; set; }
    }
}
