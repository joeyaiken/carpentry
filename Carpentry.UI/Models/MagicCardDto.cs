using Carpentry.Logic.Models;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Carpentry.UI.Models
{
    public class MagicCardDto
    {
        [JsonProperty("cmc")]
        public int? Cmc { get; set; }

        [JsonProperty("colorIdentity")]
        public List<string> ColorIdentity { get; set; }

        [JsonProperty("colors")]
        public List<string> Colors { get; set; }

        [JsonProperty("manaCost")]
        public string ManaCost { get; set; }

        [JsonProperty("multiverseId")]
        public int MultiverseId { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("prices")]
        public Dictionary<string, decimal?> Prices { get; set; }

        [JsonProperty("variants")]
        public Dictionary<string, string> Variants { get; set; }

        [JsonProperty("lealities")]
        public List<string> Legalities { get; set; }

        [JsonProperty("rarity")]
        public string Rarity { get; set; }

        [JsonProperty("set")]
        public string Set { get; set; }

        [JsonProperty("text")]
        public string Text { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }
    }
}
