using Newtonsoft.Json;

namespace Carpentry.Data.LegacyModels
{
    public class Card
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("multiverseId")]
        public int MultiverseId { get; set; }

        [JsonProperty("isFoil")]
        public bool IsFoil { get; set; }

        [JsonProperty("cardStatusId")]
        public int CardStatusId { get; set; }

        [JsonProperty("deckId")]
        public int? DeckId { get; set; }
    }
}
