using Newtonsoft.Json;

namespace Carpentry.Legacy.Models
{
    public class LegacyFilterOptionDto
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("value")]
        public string Value { get; set; }
    }
}
