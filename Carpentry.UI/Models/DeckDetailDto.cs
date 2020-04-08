//using Carpentry.Data.Models;
using Carpentry.Logic.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace Carpentry.UI.Models
{
    public class DeckDetailDto //: CardCollectionDto
    {
        public DeckDetailDto(DeckDetail model)
        {
            CardDetails = model.CardDetails.Select(x => new InventoryCardDto(x)).ToList();
            CardOverviews = model.CardOverviews.Select(x => new InventoryOverviewDto(x)).ToList();
            Props = new DeckPropertiesDto(model.Props);
            Stats = new DeckStatsDto(model.Stats);
        }

        public DeckDetail ToModel()
        {
            DeckDetail result = new DeckDetail
            {
                CardDetails = CardDetails.Select(x => x.ToModel()).ToList(),
                CardOverviews = CardOverviews.Select(x => x.ToModel()).ToList(),
                Props = Props.ToModel(),
                Stats = Stats.ToModel(),
            };
            return result;
        }

        //IDK if I need this since I'll have the list from the main menu, but w/e
        [JsonProperty("props")]
        public DeckPropertiesDto Props { get; set; }



        //what if cards were CardOverviewDTOs ? I need something to list in Deck Editor but want minimal logic there


        //I still need to itterate over the Deck/Inventory cards for each overview
        //  Needs to be inventory cards so I can show the variant

        [JsonProperty("cardOverviews")]
        public List<InventoryOverviewDto> CardOverviews { get; set; }

        [JsonProperty("cardDetails")]
        public List<InventoryCardDto> CardDetails { get; set; }

        [JsonProperty("stats")]
        public DeckStatsDto Stats { get; set; }

        //[JsonProperty("cards")]
        //public List<Card> Cards { get; set; }

        //[JsonProperty("data")]
        //public Dictionary<int, MagicCardDto> Data { get; set; }
    }
}
