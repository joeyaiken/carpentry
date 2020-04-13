using Newtonsoft.Json;

namespace Carpentry.Data.LegacyModels
{
    public class CardDto
    {
        [JsonProperty("card")]
        public Card Card { get; set; }

        [JsonProperty("data")]
        public MagicCardDto Data { get; set; }
    }
}
