using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Carpentry.Logic.Models
{
    public class DeckCardOverview
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("cost")]
        public string Cost { get; set; }

        [JsonProperty("cmc")]
        public int? Cmc { get; set; }

        [JsonProperty("img")]
        public string Img { get; set; }

        [JsonProperty("count")]
        public int Count { get; set; }

        [JsonProperty("category")]
        public string Category { get; set; }
    }
}
