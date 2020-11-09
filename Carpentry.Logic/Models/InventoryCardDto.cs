using Newtonsoft.Json;

namespace Carpentry.Logic.Models
{
    public class InventoryCardDto
    {
        //public InventoryCardDto()
        //{
        //    DeckCards = new List<InventoryDeckCardDto>();
        //}


        [JsonProperty("cardId")]
        public int CardId { get; set; }

        [JsonProperty("isFoil")]
        public bool IsFoil { get; set; }

        [JsonProperty("statusId")]
        public int StatusId { get; set; }

        [JsonProperty("collectorNumber")]
        public int CollectorNumber { get; set; }

        //[JsonProperty("deckCards")]
        //public InventoryDeckCardDto[] DeckCards { get; set; }

        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("set")]
        public string Set { get; set; }


        //Should this be "deck cards" instead of "deck card IDs"
        //[JsonProperty("deckCardIds")]
        //public List<int> DeckCardIds { get; set; }


    }
}
