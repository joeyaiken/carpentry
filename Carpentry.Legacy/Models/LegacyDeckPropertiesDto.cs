using Newtonsoft.Json;

namespace Carpentry.Legacy.Models
{
    public class LegacyDeckPropertiesDto
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("format")]
        public string Format { get; set; }

        [JsonProperty("notes")]
        public string Notes { get; set; }

        [JsonProperty("basicW")]
        public int BasicW { get; set; }

        [JsonProperty("basicU")]
        public int BasicU { get; set; }

        [JsonProperty("basicB")]
        public int BasicB { get; set; }

        [JsonProperty("basicR")]
        public int BasicR { get; set; }

        [JsonProperty("basicG")]
        public int BasicG { get; set; }
    }
}
